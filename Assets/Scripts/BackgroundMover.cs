using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple parallax background mover for two layers relative to the camera.
/// - Use `layerNear` and `layerFar` to assign the two background transforms.
/// - Parallax factors are multipliers of the camera delta from its start position.
/// - Smoothing optional.
/// </summary>
public class BackgroundMover : MonoBehaviour
{
    [Header("Camera")]
    public Transform cameraTransform;
    public bool useMainCamera = true;

    [Header("Layers")]
    public Transform layerNear;
    public Transform layerFar;

    [Header("Parallax Factors")]
    [Tooltip("How much the near layer follows the camera. 1 = same speed, 0 = static.")]
    public Vector2 nearParallax = new Vector2(0.9f, 0f);
    [Tooltip("How much the far layer follows the camera. 1 = same speed, 0 = static.")]
    public Vector2 farParallax = new Vector2(0.3f, 0f);

    [Header("Smoothing")]
    public bool smooth = true;
    [Tooltip("Higher = snappier movement when smoothing is enabled.")]
    public float smoothSpeed = 8f;

    private Vector3 initialCamPos;
    private Vector3 initialNearPos;
    private Vector3 initialFarPos;

    private void Start()
    {
        if (useMainCamera && (cameraTransform == null))
            cameraTransform = Camera.main ? Camera.main.transform : null;

        if (cameraTransform == null)
        {
            var cam = FindObjectOfType<Camera>();
            if (cam != null) cameraTransform = cam.transform;
        }

        initialCamPos = cameraTransform != null ? cameraTransform.position : Vector3.zero;

        if (layerNear != null) initialNearPos = layerNear.position;
        if (layerFar != null) initialFarPos = layerFar.position;
    }

    private void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 camDelta = cameraTransform.position - initialCamPos;

        if (layerNear != null)
        {
            Vector3 target = initialNearPos + new Vector3(camDelta.x * nearParallax.x, camDelta.y * nearParallax.y, 0f);
            if (smooth)
                layerNear.position = Vector3.Lerp(layerNear.position, target, 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime));
            else
                layerNear.position = target;
        }

        if (layerFar != null)
        {
            Vector3 target = initialFarPos + new Vector3(camDelta.x * farParallax.x, camDelta.y * farParallax.y, 0f);
            if (smooth)
                layerFar.position = Vector3.Lerp(layerFar.position, target, 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime));
            else
                layerFar.position = target;
        }
    }
}
