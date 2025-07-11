using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [Header("Particle Pool")]
    public ParticleInstance particleInstancePrefab;

    private ObjectPool<ParticleInstance> particlePool;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 

        particlePool = new ObjectPool<ParticleInstance>(
            createFunc: () => Instantiate(particleInstancePrefab, this.transform.position, Quaternion.identity, transform),
            actionOnGet: (particles) => particles.gameObject.SetActive(true),
            actionOnRelease: (particles) => StopParticle(particles),
            actionOnDestroy: (particles) => Destroy(particles.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 50
        );
    }

    public void PlayParticle(GameObject prefab, Vector3 position)
    {
        ParticleInstance particles = particlePool.Get();
        particles.Play(prefab, position);
        StartCoroutine(EndPlay(particles));
    }
    public void StopParticle(ParticleInstance particles)
    {
        particles.Stop();
        particles.gameObject.SetActive(false);
    }

    private IEnumerator EndPlay(ParticleInstance particles)
    {
        // wait for some time... should probably also have something for if it loops like on fire
        yield return new WaitForSeconds(3f);

        particlePool.Release(particles);
    }
}
