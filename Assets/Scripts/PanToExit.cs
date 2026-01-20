using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;

public class PanToExit : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject player;
    [SerializeField] GameObject levelExit;

    [YarnCommand("LookAtExit")]
    public void LookAtExit()
    {
        virtualCamera.Follow = levelExit.transform;
        Debug.Log("LookingAtExit");
    }

    [YarnCommand("LookAtPlayer")]
    public void LookAtPlayer()
    {
        virtualCamera.Follow = player.transform;
        Debug.Log("LookingAtPlayer");
    }
}
