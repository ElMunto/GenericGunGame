using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    public static Action OnShoot;

    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float dialogueStopDuration = 0.5f; // Duration to smoothly stop when entering dialogue
    [SerializeField] private ParticleSystem _smokeSystem;
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField] private float _bulletSpeed = 12;
    [SerializeField] private float _torque = 120;
    [SerializeField] private float _maxTorqueBonus = 150;

    [SerializeField] private float _maxAngularVelocity = 10;

    [SerializeField] private float _forceAmount = 600;
    [SerializeField] private float _maxUpAssist = 30;

    [SerializeField] private float _smokeLength = 0.5f;

    [SerializeField] private float _maxY = 10;
    [SerializeField] private float nudgePower = 0.5f;

    public int currentClip, maxClipSize = 10, currentAmmo, maxAmmoSize = 100;

    private CinemachineImpulseSource _impulseSource;
    private Rigidbody _rb;
    private float _lastFired;
    private bool _fire;

    // For banking velocity during dialogue
    private Vector3 _storedVelocity = Vector3.zero;
    private Vector3 _storedAngularVelocity = Vector3.zero;

    public bool IsPaused { get; set; } = false;

    // Constraints setup
    private readonly RigidbodyConstraints baseConstraints =
        RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    private readonly RigidbodyConstraints dialogueConstraints =
        RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY |
        RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    private bool _isInDialogue;
    public bool IsInDialogue
    {
        get => _isInDialogue;
        set
        {
            if (_isInDialogue != value)
            {
                _isInDialogue = value;
                if (_isInDialogue)
                {
                    // Bank velocities and start smooth stop
                    if (_rb != null)
                    {
                        _storedVelocity = _rb.velocity;
                        _storedAngularVelocity = _rb.angularVelocity;
                        StopAllCoroutines();
                        StartCoroutine(SmoothStopCoroutine(dialogueStopDuration));
                    }
                }
                else
                {
                    // Restore velocities
                    if (_rb != null)
                    {
                        _rb.velocity = _storedVelocity;
                        _rb.angularVelocity = _storedAngularVelocity;
                    }
                }
                UpdateConstraints();
            }
        }
    }

    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _rb = GetComponent<Rigidbody>();
        UpdateConstraints();
    }
    private IEnumerator SmoothStopCoroutine(float duration)
    {
        if (_rb == null) yield break;
        Vector3 initialVelocity = _rb.velocity;
        Vector3 initialAngularVelocity = _rb.angularVelocity;
        Vector3 initialPosition = _rb.position;
        Vector3 targetPosition = _rb.position + initialVelocity * duration;
        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            _rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, t);
            _rb.angularVelocity = Vector3.Lerp(initialAngularVelocity, Vector3.zero, t);
            Vector3 nextPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            _rb.MovePosition(nextPosition);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.MovePosition(targetPosition);
    }

    private void UpdateConstraints()
    {
        if (_rb == null) return;
        if (_isInDialogue)
        {
            _rb.constraints = dialogueConstraints;
            // Do not immediately zero velocity/angVel here; SmoothStopCoroutine handles gradual stop
        }
        else
        {
            _rb.constraints = baseConstraints;
        }
    }

    void Update()
    {
        if (IsInDialogue || IsPaused) return;

        // Clamp max velocity
        _rb.angularVelocity = new Vector3(0, 0, Mathf.Clamp(_rb.angularVelocity.z, -_maxAngularVelocity, _maxAngularVelocity));

        if (_smokeSystem.isPlaying && _lastFired + _smokeLength < Time.time) _smokeSystem.Stop();
    }

    private void OnEnable()
    {
        OnShoot += GunScreenShake;
    }

    private void OnDisable()
    {
        OnShoot -= GunScreenShake;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (IsPaused || IsInDialogue) return;
        if (context.performed)
        {
            if (currentClip > 0)
            {
                var hitsTarget = Physics.Raycast(_spawnPoint.position, _spawnPoint.forward, float.PositiveInfinity, _targetLayer);

                var bullet = Instantiate(_bulletPrefab, _spawnPoint.position, _spawnPoint.rotation);
                bullet.Init(_spawnPoint.forward * _bulletSpeed, hitsTarget);
                _smokeSystem.Play();
                _lastFired = Time.time;

                var assistPoint = Mathf.InverseLerp(0, _maxY, _rb.position.y);
                var assistAmount = Mathf.Lerp(_maxUpAssist, 0, assistPoint);
                var forceDir = -transform.forward * _forceAmount + Vector3.up * assistAmount;
                if (_rb.position.y > _maxY) forceDir.y = Mathf.Min(0, forceDir.y);
                _rb.AddForce(forceDir);

                var angularPoint = Mathf.InverseLerp(0, _maxAngularVelocity, Mathf.Abs(_rb.angularVelocity.z));
                var amount = Mathf.Lerp(0, _maxTorqueBonus, angularPoint);
                var torque = _torque + amount;

                var dir = Vector3.Dot(_spawnPoint.forward, Vector3.right) < 0 ? Vector3.back : Vector3.forward;
                _rb.AddTorque(dir * torque);

                currentClip--;

                OnShoot.Invoke();
            }
        }
    }

    // Fix Nudge or scrap

    public void NudgeRight()
    {
        if (IsPaused || IsInDialogue) return;
        Vector3 nudgeDir = Vector3.right;
        _rb.AddForce(nudgeDir * nudgePower, ForceMode.Impulse);
    }

    public void NudgeLeft()
    {
        if (IsPaused || IsInDialogue) return;
        Vector3 nudgeDir = Vector3.left;
        _rb.AddForce(nudgeDir * nudgePower, ForceMode.Impulse);
    }

    public void ContinueDialogue(InputAction.CallbackContext context)
    {
        Debug.Log("ContinueDialogue");
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
        Debug.Log("ScreenShake");
    }

    public void Reload()
    {
        int reloadAmount = maxClipSize - currentClip;
        reloadAmount = (currentAmmo - reloadAmount) >= 0 ? reloadAmount : currentAmmo;
        currentClip += reloadAmount;
        currentAmmo -= reloadAmount;
        Debug.Log("Reload");
    }

    public void AddAmmo(int ammoAmount)
    {
        currentAmmo += ammoAmount;
        if (currentAmmo > maxAmmoSize)
        {
            currentAmmo = maxAmmoSize;
        }
    }

    public void RestartLevel()
    {
        Fader fade = FindFirstObjectByType<Fader>();
        fade.FadeInAndOut();
    }
}