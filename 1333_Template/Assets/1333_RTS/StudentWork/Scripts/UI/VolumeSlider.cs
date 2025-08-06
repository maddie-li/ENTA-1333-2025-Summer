using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum VolumeChannels
{
    Master,
    SFX,
    Music
}

public class VolumeSlider : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private VolumeChannels channel;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = slider.maxValue;
    }

    public void SetVolume(float sliderValue)
    {
        float curvedValue = Mathf.Pow(sliderValue, 3f);

        float dB;
        if (curvedValue <= 0.0001f)
            dB = -80f; 
        else
            dB = 20f * Mathf.Log10(curvedValue);

        mixer.SetFloat(channel.ToString(), dB);
    }

}
