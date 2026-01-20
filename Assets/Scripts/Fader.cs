using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.Mathematics;


public class Fader : MonoBehaviour
{
    [SerializeField] private float _fadeTime = 1.5f;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;


    [SerializeField ]private Image _image;
    private CinemachineVirtualCamera _virtualCam;

    private void Start()
    {
        
    }

    private void Awake()
    {
        //StartCoroutine(FadeIn()); Fad in on scene load
        //_image = GetComponent<Image>();
        //_virtualCam = FindFirstObjectByType<CinemachineVirtualCamera>();
    }

    public void FadeInAndOut()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        yield return StartCoroutine(FadeRoutine(1f));
        //Respawn();
        StartCoroutine(FadeRoutine(0f));
    }


    private IEnumerator FadeRoutine(float targetAlpha)
    {
        float elapsedTime = 0f;
        float startValue = _image.color.a;

        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startValue, targetAlpha, elapsedTime / _fadeTime);
            _image.color = new Color(_image.color.r, _image.color.b, _image.color.g, newAlpha);
            yield return null;
        }

        _image.color = new Color(_image.color.r, _image.color.b, _image.color.g, targetAlpha);
    }

    private void Respawn()
    {

        Transform player = Instantiate(_playerPrefab, _respawnPoint.position, Quaternion.identity).transform;
        //_virtualCam.Follow = player;
    }


}
