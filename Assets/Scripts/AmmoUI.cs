using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public GunHandler gunHandler;
    public Text text;


    // Start is called before the first frame update
    void Start()
    {
        UpdateAmmoText();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmoText();
    }

    public void UpdateAmmoText()
    {
        text.text = $"{gunHandler.currentClip} / {gunHandler.maxClipSize} | {gunHandler.currentAmmo} / {gunHandler.maxAmmoSize}";
    }

    //update to TMPro

}
