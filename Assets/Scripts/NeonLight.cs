using System.Collections;
using UnityEngine;

public class NeonLight : MonoBehaviour
{
    [Header("Toggle Settings")]
    [Tooltip("The first child object to toggle on/off.")]
    public GameObject firstChild;

    [Tooltip("The second child object to toggle on/off.")]
    public GameObject secondChild;

    [Tooltip("How many seconds to wait before switching which child is active.")]
    public float switchFrequency = 1f;

    void Start()
    {
        if (firstChild == null || secondChild == null)
        {
            Debug.LogWarning("NeonLight requires two child objects assigned in the inspector.");
            return;
        }

        firstChild.SetActive(true);
        secondChild.SetActive(false);
        StartCoroutine(ToggleChildren());
    }

    IEnumerator ToggleChildren()
    {
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Max(0.01f, switchFrequency));

            bool firstIsActive = firstChild.activeSelf;
            firstChild.SetActive(!firstIsActive);
            secondChild.SetActive(firstIsActive);
        }
    }
}
