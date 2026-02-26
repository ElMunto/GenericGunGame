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

    private void UpdateConstraints()
    {
        if (_rb == null) return;
        if (_isInDialogue)
        {
            _rb.constraints = dialogueConstraints;
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
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