using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class PauseGame : MonoBehaviour
{

    public DialogueRunner dialogueRunner;

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    TimeManager timeManager;
    [SerializeField]
    PlayerInput playerInput;

    [YarnCommand("pauseGame")]
    public void Pause()
    {
        pauseMenu.SetActive(true);
        timeManager.enabled = false;
        playerInput.enabled = false;
        Time.timeScale = 0;
    }

    [YarnCommand("continueGame")]
    public void Continue()
    {
        pauseMenu.SetActive(false);
        timeManager.enabled = true;
        playerInput.enabled = true;
        Time.timeScale = 1;
    }
}



