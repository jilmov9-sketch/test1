using UnityEngine;

/// <summary>
/// Gamepad input implementation for controller support
/// Supports Xbox, PS4/PS5, and generic controllers
/// Default controls:
/// - Left Stick: Movement
/// - Right Stick: Turret rotation
/// - Right Trigger (R2): Fire
/// - Left Trigger (L2): Special ability
/// - A/Cross: Brake
/// </summary>
public class GamepadInput : MonoBehaviour, IInputProvider
{
    [Header("Gamepad Settings")]
    [SerializeField] private float deadZone = 0.1f;
    [SerializeField] private float aimSensitivity = 1.5f;
    
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
        // Movement input (Left Stick)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        _movementInput = ApplyDeadZone(new Vector2(horizontal, vertical));

        // Turret rotation (Right Stick X axis)
        float rightStickX = Input.GetAxis("RightStickX");
        _rotationInput = Mathf.Abs(rightStickX) > deadZone ? rightStickX * aimSensitivity : 0f;

        // Fire (Right Trigger - R2/RT)
        float rightTrigger = Input.GetAxis("RightTrigger");
        _fireInput = rightTrigger > deadZone;
        
        // Alternative fire button (X/Square on some controllers)
        if (!_fireInput && Input.GetButtonDown("Fire1"))
        {
            _fireInput = true;
        }

        // Special ability (Left Trigger - L2/LT)
        float leftTrigger = Input.GetAxis("LeftTrigger");
        _specialInput = leftTrigger > deadZone;

        // Brake (A/Cross button)
        _brakeInput = Input.GetButton("Jump");
    }

    /// <summary>
    /// Apply dead zone to input vector to prevent drift
    /// </summary>
    private Vector2 ApplyDeadZone(Vector2 input)
    {
        if (input.magnitude < deadZone)
        {
            return Vector2.zero;
        }
        
        // Normalize and scale to maintain consistent speed
        return input.normalized * Mathf.Clamp01((input.magnitude - deadZone) / (1f - deadZone));
    }

    /// <summary>
    /// Check if gamepad is connected
    /// </summary>
    public static bool IsGamepadConnected()
    {
        string[] joystickNames = Input.GetJoystickNames();
        return joystickNames.Length > 0 && !string.IsNullOrEmpty(joystickNames[0]);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        deadZone = Mathf.Clamp01(deadZone);
        aimSensitivity = Mathf.Max(0.1f, aimSensitivity);
    }
#endif
}
