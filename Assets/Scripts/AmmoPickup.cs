using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AmmoPickup : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    private void OnTriggerEnter(Collider other)
    {
        GunHandler gunHandler = other.gameObject.GetComponent<GunHandler>();
        if (gunHandler)
        {
            gunHandler.AddAmmo(gunHandler.maxAmmoSize);
            dialogueRunner.StartDialogue("Reloading");
            Destroy(gameObject);
        }
    }
}
