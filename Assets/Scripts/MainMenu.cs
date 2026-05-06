using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{

    public Button[] buttons;

    [Header("Music")]
    [SerializeField] private SoundSO _mainMenuMusic;
    private AudioManager _audioManager;

    //public GameObject loadingScreen;
    //public TextMeshProUGUI loadingText;



    private void Start()
    {
        Time.timeScale = 1;
        _audioManager = FindObjectOfType<AudioManager>();
        if (_audioManager != null && _mainMenuMusic != null)
        {
            _audioManager.PlayMusic(_mainMenuMusic);
        }
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

    /*public IEnumerator LoadMain()
    {
        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mainMenuScene);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= .9f)
            {
                loadingText.text = "Press any button to continue";
                loadingIcon.SetActive(false);

                if(Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true;

                    Time.timescale = 1f;
                }
            }

            yield return null;
        }
    }*/
}
