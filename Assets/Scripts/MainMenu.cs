using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Level Buttons")]
    public Button[] buttons;
    [Tooltip("Time Text for each level button (same order as buttons)")]
    public TextMeshProUGUI[] levelTimeTexts;
    [System.Serializable]
    public class LevelBottleImages
    {
        public List<Image> bottleImages;
    }
    [Tooltip("Bottle Images for each level button (list of 3 per level, same order as buttons)")]
    public List<LevelBottleImages> levelBottleImages;

    [Header("Music")]
    [SerializeField] private SoundSO _mainMenuMusic;
    private AudioManager _audioManager;

    private void Start()
    {
        Time.timeScale = 1;
        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager != null && _mainMenuMusic != null)
        {
            _audioManager.PlayMusic(_mainMenuMusic);
        }
        UpdateLevelSelectUI();
    }

    private void OnEnable()
    {
        UpdateLevelSelectUI();
    }

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    // Call this in Start to update the UI
    private void UpdateLevelSelectUI()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            // Time and bottles only visible if level is unlocked and completed
            bool isUnlocked = i < unlockedLevel;
            bool isCompleted = PlayerPrefs.HasKey($"Level{i+1}_BestTime");

            if (levelTimeTexts != null && i < levelTimeTexts.Length && levelTimeTexts[i] != null)
            {
                levelTimeTexts[i].gameObject.SetActive(isUnlocked && isCompleted);
                if (isUnlocked && isCompleted)
                {
                    float bestTime = PlayerPrefs.GetFloat($"Level{i+1}_BestTime", 0);
                    levelTimeTexts[i].text = FormatTime(bestTime);
                }
            }

            if (levelBottleImages != null && i < levelBottleImages.Count && levelBottleImages[i] != null && levelBottleImages[i].bottleImages != null)
            {
                int bottles = PlayerPrefs.GetInt($"Level{i+1}_Bottles", 0);
                for (int b = 0; b < levelBottleImages[i].bottleImages.Count; b++)
                {
                    var img = levelBottleImages[i].bottleImages[b];
                    if (img != null)
                        img.gameObject.SetActive(isUnlocked && isCompleted);
                    if (isUnlocked && isCompleted && img != null)
                        img.color = b < bottles ? Color.white : Color.gray;
                }
            }
        }
    }

    // Helper to format time as mm:ss.ff
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        float seconds = time % 60f;
        return $"{minutes:00}:{seconds:00.00}";
    }

    public void OpenLevel(int levelId)
    {
        if (_audioManager != null)
        {
            _audioManager.StopMusic();
        }
        string levelName = "Level " + levelId;
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    // Call this after completing a level
    public static void UpdateLevelStats(int level, float time, int bottles)
    {
        // Only update if better time or more bottles
        bool hasTime = PlayerPrefs.HasKey($"Level{level}_BestTime");
        float bestTime = PlayerPrefs.GetFloat($"Level{level}_BestTime", float.MaxValue);
        int bestBottles = PlayerPrefs.GetInt($"Level{level}_Bottles", 0);

        bool update = false;
        if (!hasTime || time < bestTime)
        {
            PlayerPrefs.SetFloat($"Level{level}_BestTime", time);
            update = true;
        }
        if (bottles > bestBottles)
        {
            PlayerPrefs.SetInt($"Level{level}_Bottles", bottles);
            update = true;
        }
        if (update)
            PlayerPrefs.Save();
    }
}
