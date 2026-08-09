using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingPlatformBehaviour : MonoBehaviour
{
    [Header("Track")]
    public Transform[] waypoints;
    public bool loop = true;
    public float speed = 2f;
    public bool smoothMovement = false;
    public float smoothTime = 0.3f;

    [Header("Gizmos")]
    public Color gizmoColor = Color.cyan;
    public float gizmoRadius = 0.1f;

    private int currentIndex = 0;
    private Vector3 currentVelocity = Vector3.zero;
    private Rigidbody platformRigidbody;

    private void Awake()
    {
        platformRigidbody = GetComponent<Rigidbody>();
        if (platformRigidbody != null)
        {
            platformRigidbody.isKinematic = true;
            platformRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            platformRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
    }

    private void FixedUpdate()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        if (waypoints[currentIndex] == null)
            return;

        Vector3 currentPosition = platformRigidbody != null ? platformRigidbody.position : transform.position;
        Vector3 targetPosition = waypoints[currentIndex].position;
        Vector3 moveDirection = targetPosition - currentPosition;
        float step = speed * Time.fixedDeltaTime;

        if (moveDirection.sqrMagnitude <= step * step)
        {
            MovePlatform(targetPosition);
            currentVelocity = Vector3.zero;
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = loop ? 0 : waypoints.Length - 1;
            }
        }
        else
        {
            Vector3 nextPosition = smoothMovement
                ? Vector3.SmoothDamp(currentPosition, targetPosition, ref currentVelocity, smoothTime, speed, Time.fixedDeltaTime)
                : currentPosition + moveDirection.normalized * step;

            MovePlatform(nextPosition);
        }
    }

    private void MovePlatform(Vector3 position)
    {
        if (platformRigidbody != null)
            platformRigidbody.MovePosition(position);
        else
            transform.position = position;
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = gizmoColor;

        Vector3? previous = null;
        Vector3 firstPoint = Vector3.zero;
        bool firstPointSet = false;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null)
                continue;

            Vector3 point = waypoints[i].position;
            Gizmos.DrawSphere(point, gizmoRadius);

            if (!firstPointSet)
            {
                firstPoint = point;
                firstPointSet = true;
            }

            if (previous.HasValue)
            {
                Gizmos.DrawLine(previous.Value, point);
            }

            previous = point;
        }

        if (loop && previous.HasValue && firstPointSet)
        {
            Gizmos.DrawLine(previous.Value, firstPoint);
        }
    }
}
