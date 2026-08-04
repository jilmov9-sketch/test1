using UnityEngine;

/// <summary>
/// Base Tank class - Core tank behavior and properties
/// Handles movement, turret rotation, and basic stats
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TankController : MonoBehaviour
{
    [Header("Tank Stats")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float turretRotateSpeed = 15f;
    [SerializeField] private int maxHealth = 100;
    
    [Header("Components")]
    [SerializeField] private Transform turretPivot;
    [SerializeField] private Transform barrelPivot;
    
    [Header("Audio")]
    [SerializeField] private AudioClip engineSound;
    [SerializeField] private AudioClip fireSound;
    
    // Component references
    private Rigidbody _rb;
    private AudioSource _audioSource;
    
    // State
    private Vector2 _movementInput;
    private float _rotationInput;
    private bool _fireInput;
    private bool _brakeInput;
    private int _currentHealth;
    private bool _isDead;
    
    // Properties
    public int CurrentHealth => _currentHealth;
    public bool IsDead => _isDead;
    public bool IsMoving { get; private set; }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        _currentHealth = maxHealth;
        _isDead = false;
    }

    private void Update()
    {
        if (_isDead) return;
        
        GetInput();
        RotateTurret();
    }

    private void FixedUpdate()
    {
        if (_isDead) return;
        
        MoveTank();
    }

    /// <summary>
    /// Get input from InputManager
    /// </summary>
    private void GetInput()
    {
        if (InputManager.Instance == null) return;
        
        _movementInput = InputManager.Instance.Movement;
        _rotationInput = InputManager.Instance.Rotation;
        _fireInput = InputManager.Instance.FirePressed;
        _brakeInput = InputManager.Instance.Brake;
    }

    /// <summary>
    /// Move the tank based on input
    /// </summary>
    private void MoveTank()
    {
        if (_brakeInput)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
            IsMoving = false;
            return;
        }
        
        // Calculate movement direction
        Vector3 movement = transform.forward * _movementInput.y + transform.right * _movementInput.x;
        
        // Apply movement force
        if (movement.magnitude > 0.1f)
        {
            _rb.AddForce(movement.normalized * moveSpeed, ForceMode.Acceleration);
            IsMoving = true;
        }
        else
        {
            // Apply drag when no input
            _rb.velocity *= 0.95f;
            IsMoving = false;
        }
        
        // Rotate tank body based on horizontal input
        if (Mathf.Abs(_movementInput.x) > 0.1f || Mathf.Abs(_movementInput.y) > 0.1f)
        {
            float targetAngle = Mathf.Atan2(_movementInput.x, _movementInput.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Rotate turret based on mouse/controller input
    /// </summary>
    private void RotateTurret()
    {
        if (turretPivot == null) return;
        
        // Rotate turret with right stick or mouse X
        if (Mathf.Abs(_rotationInput) > 0.1f)
        {
            turretPivot.Rotate(0, _rotationInput * turretRotateSpeed * Time.deltaTime, 0);
        }
        
        // For keyboard/mouse, aim at mouse position
        if (InputManager.Instance.CurrentInputType == InputManager.InputType.KeyboardMouse)
        {
            AimAtMousePosition();
        }
    }

    /// <summary>
    /// Aim turret at mouse position (keyboard/mouse only)
    /// </summary>
    private void AimAtMousePosition()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null || turretPivot == null) return;
        
        Vector3 aimDirection = InputManager.Instance.GetAimDirection(mainCamera);
        aimDirection.y = 0; // Keep turret level
        
        if (aimDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(aimDirection);
            turretPivot.rotation = Quaternion.Slerp(turretPivot.rotation, targetRotation, turretRotateSpeed * 2f * Time.deltaTime);
        }
    }

    /// <summary>
    /// Fire weapon - to be overridden by subclasses
    /// </summary>
    public virtual void Fire()
    {
        if (_isDead) return;
        
        Debug.Log($"{gameObject.name} fired!");
        
        // Play fire sound
        if (fireSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(fireSound);
        }
        
        // This will be implemented in WeaponSystem
    }

    /// <summary>
    /// Take damage
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    /// <param name="attacker">Who dealt the damage</param>
    public void TakeDamage(int damage, GameObject attacker)
    {
        if (_isDead) return;
        
        _currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {_currentHealth}/{maxHealth}");
        
        if (_currentHealth <= 0)
        {
            Die(attacker);
        }
    }

    /// <summary>
    /// Handle tank death
    /// </summary>
    private void Die(GameObject killer)
    {
        _isDead = true;
        _rb.isKinematic = true;
        
        Debug.Log($"{gameObject.name} was destroyed by {killer?.name ?? "unknown"}");
        
        // Disable tank visuals, spawn explosion effect, etc.
        // This will be expanded in the full implementation
    }

    /// <summary>
    /// Respawn the tank
    /// </summary>
    /// <param name="position">Spawn position</param>
    /// <param name="rotation">Spawn rotation</param>
    public void Respawn(Vector3 position, Quaternion rotation)
    {
        _currentHealth = maxHealth;
        _isDead = false;
        _rb.isKinematic = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        
        transform.position = position;
        transform.rotation = rotation;
        
        gameObject.SetActive(true);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Draw movement direction
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
        
        // Draw turret direction
        if (turretPivot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(turretPivot.position, turretPivot.forward * 3f);
        }
    }
#endif
}
