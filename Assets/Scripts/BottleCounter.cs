using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BottleCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private int levelBottleTotal;

    private int count = 0;

    public static BottleCounter instance;

    private void Awake()
    {
        instance = this;
        //text = GetComponent<TMP_Text>();
    }

    private void OnEnable() => Bottle.OnSmashed += OnSmashedCount;

    private void OnDisable() => Bottle.OnSmashed -= OnSmashedCount;

    private void OnSmashedCount()
    {
        count++;
        text.text = $"{count} / {levelBottleTotal}";
    }
}
