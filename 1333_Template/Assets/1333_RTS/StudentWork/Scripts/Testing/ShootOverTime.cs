using System;
using UnityEngine;
using UnityEngine.Pool;

public class ShootOverTime : MonoBehaviour
{
	[Header("Projectile")]
	[SerializeField] private ArcingProjectile ProjectilePrefab;
	[SerializeField] private Transform ProjectileStartPoint;

	// Replace this with some kind of targeted enemy position
	[SerializeField] private Transform DebugEndPoint;

	[Header("Flight Settings")]
	[SerializeField] [Min(0.1f)] private float ProjectileSpeed = 5f; // units / second
	[SerializeField] private float ArcHeight = 2f; // world units

	[Header("Pooling")]
	private ObjectPool<ArcingProjectile> projectilePool;

    private void Awake()
    {
        projectilePool = new ObjectPool<ArcingProjectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnGet,
            actionOnRelease: OnRelease,
            actionOnDestroy: OnDestroy,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 100
            );
    }

    /// <summary>
    ///     Fires the projectile
    /// </summary>
    public void Shoot()
	{
        /*// Instantiate with generic overload so no GetComponent is needed.
		var proj =
			Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);

		proj.Launch(transform.position, DebugEndPoint.position, ProjectileSpeed, ArcHeight);*/

        var proj = projectilePool.Get();
        proj.transform.position = ProjectileStartPoint.position;
        proj.transform.rotation = Quaternion.identity;
        proj.Launch(ProjectileStartPoint.position, DebugEndPoint.position, ProjectileSpeed, ArcHeight);

    }

    private void OnGet(ArcingProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnRelease(ArcingProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroy(ArcingProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    public ArcingProjectile CreateProjectile()
	{
        var proj = Instantiate(ProjectilePrefab);
        return proj;
    }
}
