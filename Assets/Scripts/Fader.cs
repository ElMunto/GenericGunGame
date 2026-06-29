using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.Mathematics;


public class Fader : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private CanvasGroup _canvasGroup;

    public float FadeDuration => _fadeDuration;

    public void FadeIn()
    {
        StartCoroutine (FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 0, _fadeDuration));
    }

    public void FadeOut()
    {
         StartCoroutine (FadeCanvasGroup(_canvasGroup, _canvasGroup.alpha, 1, _fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}
