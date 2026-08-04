using UnityEngine;

/// <summary>
/// Keyboard and Mouse input implementation
/// Default controls:
/// - WASD/Arrow Keys: Movement
/// - Mouse: Turret rotation
/// - Left Click: Fire
/// - Right Click: Special ability
/// - Space: Brake
/// </summary>
public class KeyboardMouseInput : MonoBehaviour, IInputProvider
{
    private Vector2 _movementInput;
    private float _rotationInput;
    private bool _fireInput;
    private bool _specialInput;
    private bool _brakeInput;

    public Vector2 GetMovement() => _movementInput;
    public float GetRotation() => _rotationInput;
    public bool GetFire() => _fireInput;
    public bool GetSpecial() => _specialInput;
    public bool GetBrake() => _brakeInput;

    public void Update()
    {
        // Movement input (WASD or Arrow Keys)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _movementInput = new Vector2(horizontal, vertical).normalized;

        // Turret rotation (Mouse X)
        _rotationInput = Input.GetAxis("Mouse X");

        // Fire (Left Mouse Button)
        _fireInput = Input.GetMouseButton(0);

        // Special ability (Right Mouse Button)
        _specialInput = Input.GetMouseButton(1);

        // Brake (Space)
        _brakeInput = Input.GetKey(KeyCode.Space);
    }

    /// <summary>
    /// Get aim direction from mouse position for turret aiming
    /// </summary>
    /// <param name="camera">Main camera reference</param>
    /// <returns>World space aim direction</returns>
    public Vector3 GetAimDirection(Camera camera)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            return (hit.point - transform.position).normalized;
        }
        
        return ray.direction;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Ensure this component is on the same GameObject as InputManager
        if (GetComponent<InputManager>() == null)
        {
            Debug.LogWarning("KeyboardMouseInput should be on the same GameObject as InputManager");
        }
    }
#endif
}
