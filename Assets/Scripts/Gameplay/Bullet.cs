using UnityEngine;

/// <summary>
/// Bullet/Projectile class for tank weapons
/// Handles projectile movement, collision detection, and damage application
/// Optimized with object pooling compatibility
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    [SerializeField] private float speed = 50f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private LayerMask targetLayers;
    
    [Header("Effects")]
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private AudioClip hitSound;
    
    private Rigidbody _rb;
    private GameObject _owner;
    private float _spawnTime;
    private bool _hasHit;

    // Properties
    public int Damage => damage;
    public GameObject Owner => _owner;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        // Configure rigidbody for projectile physics
        _rb.isKinematic = false;
        _rb.useGravity = false;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void OnEnable()
    {
        _hasHit = false;
        _spawnTime = Time.time;
        
        // Reset velocity
        if (_rb != null)
        {
            _rb.velocity = transform.forward * speed;
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        // Auto-destroy after lifetime
        if (Time.time - _spawnTime > lifetime)
        {
            DestroyBullet();
        }
    }

    /// <summary>
    /// Initialize bullet with owner information
    /// </summary>
    /// <param name="owner">The tank that fired this bullet</param>
    /// <param name="position">Spawn position</param>
    /// <param name="rotation">Spawn rotation</param>
    public void Initialize(GameObject owner, Vector3 position, Quaternion rotation)
    {
        _owner = owner;
        transform.position = position;
        transform.rotation = rotation;
        
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Initialize with custom direction
    /// </summary>
    public void Initialize(GameObject owner, Vector3 position, Vector3 direction)
    {
        _owner = owner;
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(direction);
        
        if (_rb != null)
        {
            _rb.velocity = direction.normalized * speed;
        }
        
        gameObject.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hasHit) return;
        
        // Check if we hit a valid target
        if (((1 << collision.gameObject.layer) & targetLayers.value) == 0)
        {
            // Hit environment or non-target
            SpawnHitEffect(collision.contacts[0].point);
            DestroyBullet();
            return;
        }
        
        // Apply damage to target
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage, _owner);
        }
        
        // Also check parent objects for IDamageable
        Transform parent = collision.transform.parent;
        while (parent != null && damageable == null)
        {
            damageable = parent.GetComponent<IDamageable>();
            parent = parent.parent;
        }
        
        if (damageable != null)
        {
            damageable.TakeDamage(damage, _owner);
        }
        
        SpawnHitEffect(collision.contacts[0].point);
        _hasHit = true;
        DestroyBullet();
    }

    /// <summary>
    /// Spawn hit effect at impact point
    /// </summary>
    private void SpawnHitEffect(Vector3 position)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, position, Quaternion.identity);
        }
        
        // Play hit sound
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, position, 0.5f);
        }
    }

    /// <summary>
    /// Return bullet to pool
    /// </summary>
    private void DestroyBullet()
    {
        ObjectPool<Bullet> pool = FindObjectOfType<ObjectPool<Bullet>>();
        if (pool != null)
        {
            pool.Return(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Draw bullet trajectory preview
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
        
        // Draw sphere for collision radius
        Collider col = GetComponent<Collider>();
        if (col is SphereCollider sphere)
        {
            Gizmos.color = new Color(1, 1, 0, 0.3f);
            Gizmos.DrawWireSphere(transform.position, sphere.radius);
        }
    }
#endif
}

/// <summary>
/// Interface for objects that can take damage
/// </summary>
public interface IDamageable
{
    void TakeDamage(int damage, GameObject attacker);
}
