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

    public enum SfxTracks
    {
        Yippee,
        Valid,
        Invalid,
        TakeDamage,
        Die,
        Destroy
    }

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

    public void PlaySFX()
    {

        sfxSource.clip = sfxTest;
        sfxSource.Play(); 
    }

    public void StopMusic() => musicSource.Stop();
    public void StopSFX() => sfxSource.Stop();
}
