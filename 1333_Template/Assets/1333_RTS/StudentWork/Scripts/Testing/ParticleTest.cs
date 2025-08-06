using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] GameObject particleSystemPrefab;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayParticles();
    }

    private void PlayParticles()
    {
        //Debug.Log($"Playing particles {particleSystemPrefab}");
        GameObject instance = Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
        ParticleSystem ps = instance.GetComponent<ParticleSystem>();
        ps.Play();

        float totalDuration = ps.main.duration + ps.main.startLifetime.constantMax;
        Destroy(instance, totalDuration);
    }


}
