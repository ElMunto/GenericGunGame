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

    public void FadeIn()
    {
        
    }

    public void FadeOut()
    {
        
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            yield return null;
        }
        cg.alpha = end;
    }
}
