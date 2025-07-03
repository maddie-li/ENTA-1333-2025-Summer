using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance { get; private set; }

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayParticle(ParticleType.Explosion, this.transform.position);
    }

    public void PlayParticle(ParticleType type, Vector3 position)
    {
        ParticleInstance particles = particlePool.Get();
        particles.Play(type, position);
        StartCoroutine(EndPlay(particles));
    }
    public void StopParticle(ParticleInstance particles)
    {
        particles.Stop();
        particles.gameObject.SetActive(false);
    }

    private IEnumerator EndPlay(ParticleInstance particles)
    {
        // wait for some time
        yield return new WaitForSeconds(3f);

        particlePool.Release(particles);
    }
}
