using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Toggle fullscreenTog, vsyncTog;

    [SerializeField] ResItem[] resolutions;

    private int selectedResolution;

    public TextMeshProUGUI resolutionLabel;

    public AudioMixer theMixer;

    public Slider mastSlider, musicSlider, sfxSlider;

    public TextMeshProUGUI mastLable, musicLable, sfxLable;

    public AudioSource sfxLoop;

    // Start is called before the first frame update
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if(QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        //search for res in list
        bool foundRes = false;
        for(int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                selectedResolution = i;

                UpdateResLable();
            }
        }

        if (!foundRes)
        {
            resolutionLabel.text = Screen.width.ToString() + " x " + Screen.height.ToString();
        }

        if (PlayerPrefs.HasKey("Master"))
        {
            theMixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
            mastSlider.value = PlayerPrefs.GetFloat("Master");            
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            theMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
            musicSlider.value = PlayerPrefs.GetFloat("Music");           
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            theMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
            sfxSlider.value = PlayerPrefs.GetFloat("SFX");           
        }

        mastLable.text = (mastSlider.value + 80).ToString();
        musicLable.text = (musicSlider.value + 80).ToString();
        sfxLable.text = (sfxSlider.value + 80).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResLeft()
    {
        selectedResolution--;
        if(selectedResolution < 0)
        {
            selectedResolution = 0;
        }

        UpdateResLable();
    }

    public void ResRight()
    {
        selectedResolution++;
        if(selectedResolution > resolutions.Length - 1)
        {
            selectedResolution = resolutions.Length - 1;
        }

        UpdateResLable();
    }

    public void UpdateResLable()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }

    public void ApplyGraphics()
    {
        //apply fullscreen
        //Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        //set resolution
        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

    public void SetMasterVol()
    {
        mastLable.text = (mastSlider.value + 80).ToString();

        theMixer.SetFloat("Master", mastSlider.value);

        PlayerPrefs.SetFloat("Master", mastSlider.value);
    }

    public void SetMusicVol()
    {
        musicLable.text = (musicSlider.value + 80).ToString();

        theMixer.SetFloat("Music", musicSlider.value);

        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    public void SetSFXVol()
    {
        sfxLable.text = (sfxSlider.value + 80).ToString();

        theMixer.SetFloat("SFX", sfxSlider.value);

        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
    }

    public void PlaySFXLoop()
    {
        sfxLoop.Play();
    }

    public void StopSFXLoop()
    {
        sfxLoop.Stop();
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
