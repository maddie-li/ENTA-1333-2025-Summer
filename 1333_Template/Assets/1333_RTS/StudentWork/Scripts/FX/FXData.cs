using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FXData
{
    public FXType fxType;
    public SFXType soundType;
    public List<AudioClip> audioClips;
    public GameObject particlePrefab;
    public AudioClip audioClip => GetRandomClip();

    public bool HasAudio => audioClips != null && audioClips.Count > 0;
    public bool HasParticle => particlePrefab != null;

    public AudioClip GetRandomClip()
    {
        if (!HasAudio) return null;
        return audioClips[Random.Range(0, audioClips.Count)];
    }
}