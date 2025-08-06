using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
// Serializable class to set up effect groups
public class FXData
{
    public FXType fxType;
    public SFXType soundType;
    public List<AudioClip> audioClips;
    public GameObject particlePrefab;

    // Allows you to add multiple sounds to one effect and get them randomly
    public AudioClip audioClip => GetRandomClip();

    // Lambda expressions for flags
    public bool HasAudio => audioClips != null && audioClips.Count > 0;
    public bool HasParticle => particlePrefab != null;

    public AudioClip GetRandomClip()
    {
        if (!HasAudio) return null;
        return audioClips[Random.Range(0, audioClips.Count)];
    }
}