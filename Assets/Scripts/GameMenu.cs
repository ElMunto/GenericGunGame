using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameMenu : MonoBehaviour
{
    //public GameObject mainMenu, optionsMenu, gunSelectMenu, shopMenu, levelSelectMenu, pauseMenu, inGameOptionsMenu;

    //public GameObject mainMenuFirstButton, optionsMenuFirstButton, gunSelectMenuFirstButton,
        //shopMenuFirstButton, levelSelectMenuFirstButton, pauseMenuFirstButton, inGameOptionsMenuFirstButton;

    private int nextSceneToLoad;

    private void Start()
    {
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Start is called before the first frame update
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextSceneToLoad);
    }
}
