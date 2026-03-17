using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteScoreBoard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI bottlesSmashed;

    //freeze player controler

    //update time

    //upadte number of bottles smashed

    // Set the time taken (formatted string)
    public void SetTime(string formattedTime)
    {
        if (timeText != null)
            timeText.text = formattedTime;
    }

    // Set the number of bottles smashed (e.g., "5 / 10")
    public void SetBottlesSmashed(string bottleCount)
    {
        if (bottlesSmashed != null)
            bottlesSmashed.text = bottleCount;
    }
    // Optionally, overload for int values
    public void SetBottlesSmashed(int smashed, int total)
    {
        if (bottlesSmashed != null)
            bottlesSmashed.text = $"{smashed} / {total}";
    }
}
