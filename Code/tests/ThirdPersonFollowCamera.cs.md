using Sandbox;

public sealed class ThirdPersonFollowCamera : Component
{
    [Property, Title("Target")]
    public GameObject Target { get; set; } // Usually your player/pawn

    [Property, Title("Distance"), Range(50f, 500f)]
    public float Distance { get; set; } = 200f;

    [Property, Title("Height Offset (above feet)")]
    public float HeightOffset { get; set; } = 80f; // Adjust based on player height

    [Property, Title("Lateral Offset (for over-shoulder)")]
    public float LateralOffset { get; set; } = 0f; // Positive = right shoulder

    [Property, Title("Pitch (look down angle)")]
    public float Pitch { get; set; } = 15f; // Degrees, positive looks down

    [Property, Title("Smooth Speed")]
    public float SmoothSpeed { get; set; } = 8f;

    private CameraComponent _camera;

    protected override void OnStart()
    {
        _camera = Components.Get<CameraComponent>(FindMode.InSelf);
        if (_camera == null)
        {
            _camera = GameObject.AddComponent<CameraComponent>();
        }

        _camera.IsMainCamera = true;
    }

    protected override void OnUpdate()
    {
        if (Target == null) return;

        Vector3 targetFeet = Target.WorldPosition;
        Vector3 targetPos = targetFeet + Vector3.Up * HeightOffset;

        Rotation targetYaw = Rotation.FromYaw(Target.WorldRotation.Yaw());
        Vector3 behind = targetYaw.Forward * -Distance;
        Vector3 side   = targetYaw.Right * LateralOffset;
        Vector3 upBias = Vector3.Up * (Distance * 0.25f);   // adjustable

        Vector3 desiredPos = targetPos + behind + side + upBias;

        Rotation desiredRot = targetYaw * Rotation.FromPitch(Pitch);

        // Smooth
        WorldPosition = Vector3.Lerp( WorldPosition, desiredPos, SmoothSpeed * Time.Delta );
        WorldRotation = Rotation.Lerp( WorldRotation, desiredRot, SmoothSpeed * Time.Delta * 1.8f );
    }
}