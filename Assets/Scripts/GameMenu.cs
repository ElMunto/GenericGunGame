using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Fader _fader;
    [SerializeField] private DialogueRunner _dialogueRunner;
    [SerializeField] private string _startNodeName = "Beep";

    private int nextSceneToLoad;

    private void Start()
    {
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (_fader != null)
        {
            _fader.FadeIn(() =>
            {
                if (_dialogueRunner != null)
                {
                    _dialogueRunner.StartDialogue(_startNodeName);
                }
                else
                {
                    Debug.LogWarning("GameMenu Start: dialogue runner reference is missing");
                }
            });
        }
        else if (_dialogueRunner != null)
        {
            _dialogueRunner.StartDialogue(_startNodeName);
        }
    }

    // Start is called before the first frame update
    public void BackToMainMenu()
    {
        FadeAndLoadScene("MainMenu");
    }

    public void NextLevel()
    {
        FadeAndLoadScene(nextSceneToLoad);
    }

    private void FadeAndLoadScene(string sceneName)
    {
        if (_fader != null)
        {
            _fader.FadeOut(() => SceneManager.LoadScene(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void FadeAndLoadScene(int sceneBuildIndex)
    {
        if (_fader != null)
        {
            _fader.FadeOut(() => SceneManager.LoadScene(sceneBuildIndex));
        }
        else
        {
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}
