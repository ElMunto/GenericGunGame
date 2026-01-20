using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public GameObject LevelCompletePanel;

    private void Start()
    {
        //set panel to not active at start of level
        LevelCompletePanel.SetActive(false);
    }

    //Check to see if been hit by bullet
    private void OnTriggerEnter(Collider player)
    {
        Debug.Log("Player has Exit Level");
        //play Exit Animation
        //play Exit Sound/Music
        //either pause game or disable controls / warp out gun/player
        UnlockNewLevel();
        LevelCompletePanel.SetActive(true);

        //pause timer
    }

    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    //unlock next level
    //Confetti animation ?
    //Wait for animation to finish?
    //Pop up congrats Screen
}
