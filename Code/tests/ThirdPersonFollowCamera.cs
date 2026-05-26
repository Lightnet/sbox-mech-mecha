using Sandbox;
using System;

public sealed class ThirdPersonFollowCamera : Component
{
    [Property, Title("Target")]
    public GameObject Target { get; set; }

    [Property, Title("Distance"), Range(50f, 500f)]
    public float Distance { get; set; } = 200f;

    [Property, Title("Height Offset")]
    public float HeightOffset { get; set; } = 150f;

    [Property, Title("Lateral Offset")]
    public float LateralOffset { get; set; } = 0f;

    [Property, Title("Mouse Sensitivity")]
    public float MouseSensitivity { get; set; } = 0.15f;

    [Property, Title("Pitch Min"), Range(-70f, 70f)]
    public float PitchMin { get; set; } = -50f;

    [Property, Title("Pitch Max"), Range(-70f, 70f)]
    public float PitchMax { get; set; } = 60f;

    [Property, Title("Smooth Speed")]
    public float SmoothSpeed { get; set; } = 12f;

    private CameraComponent _camera;
    
    // Camera state
    private float _yaw = 0f;
    private float _pitch = 15f;

    protected override void OnStart()
    {
        _camera = Components.Get<CameraComponent>(FindMode.InSelf) ?? GameObject.AddComponent<CameraComponent>();
        _camera.IsMainCamera = true;

        // Initialize from target's current rotation
        if (Target != null)
            _yaw = Target.WorldRotation.Yaw();
    }

    protected override void OnUpdate()
    {
        if (Target == null) return;

        HandleMouseInput();
        UpdateCameraPosition();
    }

    private void HandleMouseInput()
    {
        // Only rotate when right mouse button is held (common in third-person games)
        // if (!Input.Pressed( "Attack1" ))
        //     return;

        // Get mouse movement
        // float mouseX = Input.Mouse.Delta.X * MouseSensitivity;
        // float mouseY = Input.Mouse.Delta.Y * MouseSensitivity;

        var lookInput = Input.AnalogLook;

        // float mouseX = lookInput.pitch * MouseSensitivity;
        // float mouseY = lookInput.yaw * MouseSensitivity;

        // float mouseY = lookInput.pitch * MouseSensitivity * -1;
        float mouseY = lookInput.pitch * MouseSensitivity ;
        float mouseX = lookInput.yaw * MouseSensitivity * -1;

        _yaw -= mouseX;           // Negative for natural feel
        _pitch = Math.Clamp(_pitch + mouseY, PitchMin, PitchMax);
    }

    private void UpdateCameraPosition()
    {
        Vector3 targetFeet = Target.WorldPosition;
        Vector3 targetPos = targetFeet + Vector3.Up * HeightOffset;

        // Build rotation from yaw + pitch
        Rotation cameraRot = Rotation.FromYaw(_yaw) * Rotation.FromPitch(_pitch);

        // Calculate position behind the camera
        Vector3 behind = cameraRot.Backward * Distance;   // Backward = -Forward
        Vector3 side = cameraRot.Right * LateralOffset;

        Vector3 desiredPos = targetPos + behind + side;

        Rotation desiredRot = cameraRot;

        // Smooth movement
        WorldPosition = Vector3.Lerp(WorldPosition, desiredPos, SmoothSpeed * Time.Delta);
        WorldRotation = Rotation.Lerp(WorldRotation, desiredRot, SmoothSpeed * Time.Delta * 1.8f);
    }
}