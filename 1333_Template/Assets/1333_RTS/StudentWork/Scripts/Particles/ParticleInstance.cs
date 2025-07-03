using UnityEngine;

public class ParticleInstance : MonoBehaviour
{
    public GameObject[] particlePrefabs;

    private GameObject currentInstance;

    public void Play(ParticleType type, Vector3 position)
    {
        int variantIndex = (int)type;

        StopCurrentParticles();

        if (variantIndex < 0 || variantIndex >= particlePrefabs.Length)
        {
            Debug.LogWarning("particle system Index not in range");
            return;
        }

        currentInstance = Instantiate(particlePrefabs[variantIndex], position, Quaternion.identity, transform);
        var ps = currentInstance.GetComponent<ParticleSystem>();

        ps.Play();
    }

    public void StopCurrentParticles()
    {
        if (currentInstance != null)
        {
            var ps = currentInstance.GetComponent<ParticleSystem>();
            if (ps != null)
                ps.Stop();

            Destroy(currentInstance);
            currentInstance = null;
        }
    }

    public void Stop()
    {
        StopCurrentParticles();
    }
}
