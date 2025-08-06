using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    public List<FXData> fxTypes;

    // Assigns the serialisable effect group to the effect enum
    private Dictionary<FXType, FXData> fxDict;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        fxDict = new Dictionary<FXType, FXData>();
        foreach (FXData fx in fxTypes)
        {
            fxDict[fx.fxType] = fx;
        }
    }

    // PLAYS FX
    public void DoFX(FXType fxType, Vector3? pos = null)
    {
        //Debug.Log($"doing FX {fxType}");

        if (fxDict.TryGetValue(fxType, out FXData data))
        {
            if (data.HasAudio && data.audioClip)
            {
                if (data.soundType == SFXType.UI)
                {
                    AudioManager.Instance.PlayUISFX(data.audioClip);
                }
                else
                {
                    AudioManager.Instance.PlaySFX(data.audioClip);
                }
            }

            if (data.HasParticle && pos.HasValue)
            {
                ParticleManager.Instance.PlayParticle(data.particlePrefab, pos.Value);
            }
        }
    }

}
