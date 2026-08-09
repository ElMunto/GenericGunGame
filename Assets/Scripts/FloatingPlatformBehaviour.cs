using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentIndex];
        Vector3 targetPosition = target.position;
        Vector3 moveDirection = (targetPosition - transform.position);
        float step = speed * Time.deltaTime;

        if (moveDirection.sqrMagnitude <= step * step)
        {
            transform.position = targetPosition;
            currentVelocity = Vector3.zero;
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = loop ? 0 : waypoints.Length - 1;
            }
        }
        else
        {
            if (smoothMovement)
            {
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime, speed, Time.deltaTime);
            }
            else
            {
                currentVelocity = Vector3.zero;
                transform.position += moveDirection.normalized * step;
            }
        }
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
