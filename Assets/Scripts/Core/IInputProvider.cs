using UnityEngine;

/// <summary>
/// Abstract input provider interface for multi-platform support
/// Supports Keyboard, Gamepad, and Touch inputs through unified interface
/// </summary>
public interface IInputProvider
{
    Vector2 GetMovement();
    float GetRotation();
    bool GetFire();
    bool GetSpecial();
    bool GetBrake();
    void Update();
}
