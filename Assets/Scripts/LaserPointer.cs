using UnityEngine;

public class LaserPointer : MonoBehaviour
{
    [SerializeField] private LineRenderer lr;
    public float laserRange = 100f; // Max distance


    void Update()
    {
        // Start position is this object's position
        lr.SetPosition(0, transform.position);

        RaycastHit hit;
        // Raycast forward from the gun tip
        if (Physics.Raycast(transform.position, transform.forward, out hit, laserRange))
        {
            // If we hit something, set end position to hit point
            lr.SetPosition(1, hit.point);
        }
        else
        {
            // If we hit nothing, extend to max range
            lr.SetPosition(1, transform.position + (transform.forward * laserRange));
        }
    }
}
