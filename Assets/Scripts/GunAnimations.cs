using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class GunAnimations : MonoBehaviour
{
    private static Action OnImpact;

    [SerializeField] private ParticleSystem impactDustVFX;

    [SerializeField] private float _yLandVelocityCheck = -10f;

    private Vector2 _velocityBeforePhysicsUpdate;
    private Rigidbody _rigidBody;
    private CinemachineImpulseSource _impulseSource;
    private GunHandler gunHandler;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        gunHandler = GetComponent<GunHandler>();
    }


    private void FixedUpdate()
    {
        if (gunHandler != null && gunHandler.IsInDialogue)
            return; // Skip updating velocity if in dialogue

        _velocityBeforePhysicsUpdate = _rigidBody.velocity;
    }

    private void PlayImpactDustVFX()
    {
        impactDustVFX.Play();
        Debug.Log("PlayAnimation");
    }

    private void ImpactScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void OnEnable()
    {
        OnImpact += PlayImpactDustVFX;
        OnImpact += ImpactScreenShake;
    }

    private void OnDisable()
    {
        OnImpact -= PlayImpactDustVFX;
        OnImpact -= ImpactScreenShake;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gunHandler != null && gunHandler.IsInDialogue)
            return; // Skip impact logic if in dialogue

        if (_velocityBeforePhysicsUpdate.y < _yLandVelocityCheck)
        {
            OnImpact?.Invoke();
        }
    }
}
