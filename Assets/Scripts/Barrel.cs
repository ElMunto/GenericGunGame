using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
