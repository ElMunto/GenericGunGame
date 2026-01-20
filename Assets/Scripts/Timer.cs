using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float elapsedTime;
    bool isPaused = false; // Add a flag to control pausing

    // Update is called once per frame
    void Update()
    {
        if (isPaused) return; // Skip updating the timer if paused

        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        float milliseconds = (elapsedTime % 1) * 1000;
        timerText.text = string.Format("{0:00}:{1:00}:{2:00.00}", minutes, seconds, milliseconds / 10);
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void ResumeTimer()
    {
        isPaused = false;
    }
}
