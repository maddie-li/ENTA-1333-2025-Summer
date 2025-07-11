using UnityEngine;
using static UnityEngine.ParticleSystem;
[System.Serializable]
public class FXData
{
    public FXType fxType;
    public SFXType soundType;
    public AudioClip audioClip;
    public GameObject particlePrefab;
    public bool HasAudio => audioClip != null;
    public bool HasParticle => particlePrefab != null;
}
