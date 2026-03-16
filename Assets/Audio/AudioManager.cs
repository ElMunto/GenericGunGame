using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer theMixer;

    [SerializeField] private SoundSO _gunShoot;

    [SerializeField] private SoundSO _bottleBreak;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Master"))
        {
            theMixer.SetFloat("Master", PlayerPrefs.GetFloat("Master"));
        }

        if (PlayerPrefs.HasKey("Music"))
        {
            theMixer.SetFloat("Music", PlayerPrefs.GetFloat("Music"));
        }

        if (PlayerPrefs.HasKey("SFX"))
        {
            theMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFX"));
        }
    }

    private void OnEnable()
    {
        GunHandler.OnShoot += Gun_OnShoot;
        Bottle.OnSmashed += Bottle_OnSmashed;
    }

    private void OnDisable()
    {
        GunHandler.OnShoot -= Gun_OnShoot;
        Bottle.OnSmashed -= Bottle_OnSmashed;
    }

    private void PlaySound(SoundSO soundSO)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = soundSO.Clip;
        audioSource.Play();

        if (!audioSource.loop && audioSource.clip != null)
        {
            Destroy(soundObject, audioSource.clip.length);
        }
    }

    private void Gun_OnShoot()
    {
        PlaySound(_gunShoot);
    }

    private void Bottle_OnSmashed()
    {
        PlaySound(_bottleBreak);
    }
}

