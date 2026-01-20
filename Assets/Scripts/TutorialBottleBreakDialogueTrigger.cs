using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialBottleBreakDialogueTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;

    private void OnTriggerEnter(Collider player)
    {
        dialogueRunner.StartDialogue("BreakingBottles");
    }
}
