using System.Collections;
using System.Collections.Generic;
using RTS_1333;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TimescaleSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;

    public void SetTimescale(float scale)
    {
        scale = Mathf.Clamp(scale, 1f, slider.maxValue);

        Time.timeScale = scale;
        slider.value = scale;
        text.text = $"Game Speed {scale.ToString("F0")}x";
    }

    public void TimePause()
    {
        Time.timeScale = 0f;
    }

    public void TimePlay()
    {
        Time.timeScale = slider.value;
    }
}
