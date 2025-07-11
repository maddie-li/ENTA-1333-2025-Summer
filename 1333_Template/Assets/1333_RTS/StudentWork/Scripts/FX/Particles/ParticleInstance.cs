using UnityEngine;

public class ParticleInstance : MonoBehaviour
{
    private GameObject currentInstance;

    public void Play(GameObject prefab, Vector3 position)
    {
        currentInstance = Instantiate(prefab, position, Quaternion.identity, transform);
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
