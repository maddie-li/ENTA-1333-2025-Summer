using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Main Audio Source")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("SFX")] public AudioClip sfxTest;
    [Header("Music")] public AudioClip musicTest;

    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    public void PlaySFX(SFX type)
    {
        int index = (int)type;
        if (index >= 0 && index < sfxClips.Length)
        {
            sfxSource.PlayOneShot(sfxClips[index]);
        }
    }

    public void StopMusic() => musicSource.Stop();
    public void StopSFX() => sfxSource.Stop();
}
