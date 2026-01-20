using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class ResetLevel : MonoBehaviour
{
    private PlayerControls playercontrols;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartAgain();
        }

    }

    private void StartAgain()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
