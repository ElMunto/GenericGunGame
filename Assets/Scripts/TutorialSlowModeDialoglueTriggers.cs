using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TutorialSlowModeDialoglueTriggers : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;

    public DialogueRunner dialogueRunner;

    public bool slowModeDialogueCalled = false;

    private void Start()
    {
        slowModeDialogueCalled = false;
    }

    private void OnTriggerEnter(Collider player)
    {
        if (!slowModeDialogueCalled)
        {
            slowModeDialogueCalled = true;
            dialogueRunner.StartDialogue("SlowMode");
        }

    }

}
