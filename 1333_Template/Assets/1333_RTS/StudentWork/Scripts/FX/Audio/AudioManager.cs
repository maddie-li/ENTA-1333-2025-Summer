using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Main Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource uiSource;

    [Header("SFX")] public AudioClip sfxTest;
    [Header("Music")] public AudioClip musicTest;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        PlayMusic();
    }

    public void PlayMusic()
    {
        musicSource.clip = musicTest;
        musicSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlayUISFX(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }

    public void StopMusic() => musicSource.Stop();
    public void StopSFX() => sfxSource.Stop();
}
