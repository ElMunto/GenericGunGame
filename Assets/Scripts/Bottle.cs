using System;
using UnityEngine;
using Yarn.Unity;


public class Bottle : MonoBehaviour
{
    public static Action OnSmashed;

    [SerializeField] GameObject intactBottle;
    [SerializeField] GameObject brokenBottle;
    [SerializeField] SphereCollider sphereCollider;


    private void Awake()
    {
        sphereCollider.enabled = true;
        intactBottle.SetActive(true);
        brokenBottle.SetActive(false);
    }

    public void AddExplosionForce()
    {
        // maybe add for effect?
        Debug.Log("PEW!");
    }

    //Check to see if been hit by bullet
    private void OnTriggerEnter(Collider player)
    {
        OnSmashed.Invoke();
        sphereCollider.enabled = false;
        intactBottle.SetActive(false);
        brokenBottle.SetActive(true);
        AddExplosionForce();

        Debug.Log("Hit");

    }

}
