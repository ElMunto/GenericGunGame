using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class AmmoPickup : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    [Tooltip("Should picking up this ammo start the reload dialogue?")]
    public bool startReloadDialogue = true;

    [Tooltip("How much ammo to add on pickup?")]
    public int ammoAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        GunHandler gunHandler = other.gameObject.GetComponent<GunHandler>();
        if (gunHandler)
        {
            gunHandler.AddAmmo(ammoAmount);
            if (startReloadDialogue && dialogueRunner != null)
            {
                dialogueRunner.StartDialogue("Reloading");
            }
            Destroy(gameObject);
        }
    }
}
