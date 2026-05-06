using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer theMixer;

    [SerializeField] private SoundSO _gunShoot;
    [SerializeField] private SoundSO _bottleBreak;
    [SerializeField] private SoundSO _gunReload;

    // UI Sound Effects
    [Header("UI Sounds")]
    [SerializeField] private SoundSO _uiHighlight;
    [SerializeField] private SoundSO _uiClick;


    [Header("Music")]
    [SerializeField] private SoundSO _mainMenuMusic;
    private AudioSource _musicSource;

    public static System.Action OnReload;

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

    public void PlayMusic(SoundSO musicSO)
    {
        if (_musicSource != null)
        {
            if (_musicSource.clip == musicSO.Clip && _musicSource.isPlaying)
                return; // Already playing
            _musicSource.Stop();
            Destroy(_musicSource.gameObject);
        }
        GameObject musicObject = new GameObject("Music Audio Source");
        _musicSource = musicObject.AddComponent<AudioSource>();
        _musicSource.clip = musicSO.Clip;
        _musicSource.loop = true;
        _musicSource.volume = musicSO.Volume;
        _musicSource.pitch = musicSO.Pitch;
        _musicSource.Play();
        DontDestroyOnLoad(musicObject);
    }

    public void StopMusic()
    {
        if (_musicSource != null)
        {
            _musicSource.Stop();
            Destroy(_musicSource.gameObject);
            _musicSource = null;
        }
    }

    private void OnEnable()
    {
        GunHandler.OnShoot += Gun_OnShoot;
        Bottle.OnSmashed += Bottle_OnSmashed;
        OnReload += Gun_OnReload;
    }

    private void OnDisable()
    {
        GunHandler.OnShoot -= Gun_OnShoot;
        Bottle.OnSmashed -= Bottle_OnSmashed;
        OnReload -= Gun_OnReload;
    }

    // Make PlaySound public for UI
    public void PlaySound(SoundSO soundSO)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = soundSO.Clip;
        audioSource.loop = soundSO.Loop;
        audioSource.volume = soundSO.Volume;

        float pitch = soundSO.Pitch;
        if (soundSO.RandomizePitch)
        {
            float minPitch = Mathf.Clamp(soundSO.Pitch - soundSO.RandomPitchRangeModifier, 0.1f, 3f);
            float maxPitch = Mathf.Clamp(soundSO.Pitch + soundSO.RandomPitchRangeModifier, 0.1f, 3f);
            pitch = Random.Range(minPitch, maxPitch);
        }
        audioSource.pitch = pitch;

        audioSource.Play();

        if (!audioSource.loop && audioSource.clip != null)
        {
            Destroy(soundObject, audioSource.clip.length / Mathf.Abs(audioSource.pitch));
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

    private void Gun_OnReload()
    {
        PlaySound(_gunReload);
    }

    // Public methods for UI events
    public void PlayUIHighlight()
    {
        if (_uiHighlight != null)
            PlaySound(_uiHighlight);
    }

    public void PlayUIClick()
    {
        if (_uiClick != null)
            PlaySound(_uiClick);
    }
}

