using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class LevelExit : MonoBehaviour
{
    public GameObject LevelCompletePanel;

    // Reference to the score board
    private LevelCompleteScoreBoard scoreBoard;

    private void Start()
    {
        //set panel to not active at start of level
        LevelCompletePanel.SetActive(false);

        // Find the score board component
        if (LevelCompletePanel != null)
            scoreBoard = LevelCompletePanel.GetComponent<LevelCompleteScoreBoard>();
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

        // Pause timer and get timer/bottle count
        Timer timer = FindObjectOfType<Timer>();
        if (timer != null)
            timer.PauseTimer();

        BottleCounter bottleCounter = BottleCounter.instance;

        // Format time string
        string formattedTime = "";
        if (timer != null)
        {
            float elapsed = GetElapsedTime(timer);
            formattedTime = FormatTime(elapsed);
        }

        // Get bottle count
        int smashed = 0;
        int total = 0;
        if (bottleCounter != null)
        {
            smashed = GetBottleCount(bottleCounter);
            total = GetBottleTotal(bottleCounter);
        }

        // Set values on score board
        if (scoreBoard != null)
        {
            scoreBoard.SetTime(formattedTime);
            scoreBoard.SetBottlesSmashed(smashed, total);
        }
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

    // Helper to get elapsed time from Timer
    private float GetElapsedTime(Timer timer)
    {
        // Use reflection since elapsedTime is private
        var field = typeof(Timer).GetField("elapsedTime", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            return (float)field.GetValue(timer);
        return 0f;
    }

    // Helper to format time
    private string FormatTime(float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime % 1) * 1000);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    // Helper to get smashed bottle count
    private int GetBottleCount(BottleCounter counter)
    {
        var field = typeof(BottleCounter).GetField("count", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            return (int)field.GetValue(counter);
        return 0;
    }

    // Helper to get total bottle count
    private int GetBottleTotal(BottleCounter counter)
    {
        var field = typeof(BottleCounter).GetField("levelBottleTotal", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
            return (int)field.GetValue(counter);
        return 0;
    }
}
