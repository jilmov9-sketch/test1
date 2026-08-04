using UnityEngine;

/// <summary>
/// Input Manager - Centralized input handling system
/// Manages input providers and provides unified access to input data
/// Supports runtime switching between Keyboard/Mouse and Gamepad
/// </summary>
public class InputManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private bool autoSwitchToGamepad = true;
    
    private IInputProvider _currentInput;
    private KeyboardMouseInput _keyboardInput;
    private GamepadInput _gamepadInput;
    
    private static InputManager _instance;
    
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("InputManager");
                _instance = go.AddComponent<InputManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    // Input state
    public Vector2 Movement { get; private set; }
    public float Rotation { get; private set; }
    public bool Fire { get; private set; }
    public bool FirePressed { get; private set; }
    public bool FireReleased { get; private set; }
    public bool Special { get; private set; }
    public bool Brake { get; private set; }
    
    public IInputProvider CurrentInput => _currentInput;
    public InputType CurrentInputType { get; private set; }

    public enum InputType
    {
        KeyboardMouse,
        Gamepad,
        Touch
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeInputs();
    }

    private void InitializeInputs()
    {
        // Create keyboard/mouse input
        GameObject keyboardObj = new GameObject("KeyboardMouseInput");
        keyboardObj.transform.SetParent(transform);
        _keyboardInput = keyboardObj.AddComponent<KeyboardMouseInput>();
        
        // Create gamepad input
        GameObject gamepadObj = new GameObject("GamepadInput");
        gamepadObj.transform.SetParent(transform);
        _gamepadInput = gamepadObj.AddComponent<GamepadInput>();
        
        // Default to keyboard/mouse
        SetInputType(InputType.KeyboardMouse);
    }

    private void Update()
    {
        // Auto-switch to gamepad if enabled and gamepad is detected
        if (autoSwitchToGamepad && GamepadInput.IsGamepadConnected() && CurrentInputType != InputType.Gamepad)
        {
            SetInputType(InputType.Gamepad);
        }
        else if (!GamepadInput.IsGamepadConnected() && CurrentInputType != InputType.KeyboardMouse)
        {
            SetInputType(InputType.KeyboardMouse);
        }
        
        // Update current input provider
        _currentInput.Update();
        
        // Read input values
        Movement = _currentInput.GetMovement();
        Rotation = _currentInput.GetRotation();
        
        // Track fire button press/release events
        bool currentFire = _currentInput.GetFire();
        FirePressed = currentFire && !Fire;
        FireReleased = !currentFire && Fire;
        Fire = currentFire;
        
        Special = _currentInput.GetSpecial();
        Brake = _currentInput.GetBrake();
    }

    /// <summary>
    /// Manually set the input type
    /// </summary>
    public void SetInputType(InputType type)
    {
        CurrentInputType = type;
        
        switch (type)
        {
            case InputType.KeyboardMouse:
                _currentInput = _keyboardInput;
                break;
            case InputType.Gamepad:
                _currentInput = _gamepadInput;
                break;
            default:
                Debug.LogWarning($"Input type {type} not implemented yet");
                _currentInput = _keyboardInput;
                break;
        }
        
        Debug.Log($"Switched to input type: {type}");
    }

    /// <summary>
    /// Get aim direction for turret (only available for KeyboardMouse input)
    /// </summary>
    public Vector3 GetAimDirection(Camera camera)
    {
        if (_keyboardInput != null && CurrentInputType == InputType.KeyboardMouse)
        {
            return _keyboardInput.GetAimDirection(camera);
        }
        
        // For gamepad, use forward direction or implement right-stick aiming
        return transform.forward;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Ensure singleton instance
        if (_instance == null)
        {
            _instance = this;
        }
    }
#endif
}
