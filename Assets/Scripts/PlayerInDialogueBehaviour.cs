using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerInDialogueBehaviour : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    public Image image;
    public DialogueRunner dialogueRunner;

    [SerializeField] public GunHandler gunHandler;

    private void Start()
    {
        if (gunHandler == null)
            Debug.LogWarning("GunHandler not found in parent hierarchy!");

        // Subscribe to dialogue events
        if (dialogueRunner != null)
        {
            dialogueRunner.onDialogueStart.AddListener(OnDialogueStart);
            dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        }
    }

    [YarnCommand("enablePortrait")]
    public void EnablePortrait()
    {
        image.enabled = true;
        playerInput.enabled = false;
    }

    [YarnCommand("disablePortrait")]
    public void DisablePortrait()
    {
        image.enabled = false;
        playerInput.enabled = true;
    }

    private void OnDialogueStart()
    {
        playerInput.enabled = false;
        if (gunHandler != null)
            gunHandler.IsInDialogue = true;
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
            timer.PauseTimer();
    }

    private void OnDialogueComplete()
    {
        playerInput.enabled = true;
        if (gunHandler != null)
            gunHandler.IsInDialogue = false;
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
            timer.ResumeTimer();
    }
}
