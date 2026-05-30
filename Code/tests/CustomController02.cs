using Sandbox;

public sealed class CustomController02 : Component
{
    [Property] public float WalkSpeed { get; set; } = 160.0f;
    [Property] public float JumpForce { get; set; } = 500.0f;
    [Property] public float Gravity { get; set; } = 800.0f;

    private CharacterController controller;
    private bool wishJump = false;

    protected override void OnAwake()
    {
        controller = Components.Get<CharacterController>();
    }

    protected override void OnUpdate()
    {
        // Always check button presses in OnUpdate so inputs are never missed
        if (Input.Pressed("Jump"))
        {
            wishJump = true;
        }
    }

    protected override void OnFixedUpdate()
    {
        if (controller == null) return;

        // Apply Gravity only when airborne
        if (!controller.IsOnGround)
        {
            controller.Velocity += Vector3.Down * Gravity * Time.Delta;
        }

        // Handle horizontal movement and jump processing
        BuildMoveInput();

        // Move the controller using physics tracing
        controller.Move();
    }

    private void BuildMoveInput()
    {
        // 1. Calculate camera relative direction
        Rotation cameraRotation = Scene.Camera.Transform.Rotation;
        Rotation forwardRotation = Rotation.FromYaw(cameraRotation.Yaw());

        Vector3 wishDirection = forwardRotation * Input.AnalogMove.Normal;
        Vector3 wishVelocity = wishDirection * WalkSpeed;

        // 2. Set horizontal movement while preserving current vertical velocity (gravity)
        controller.Velocity = new Vector3(wishVelocity.x, wishVelocity.y, controller.Velocity.z);

        // 3. Handle Jump safely using Punch
        if (wishJump)
        {
            wishJump = false; // Reset the flag immediately

            if (controller.IsOnGround)
            {
                // Punch breaks ground adhesion and smoothly pushes the player upward
                controller.Punch(Vector3.Up * JumpForce);
                Log.Info($"JUMP SUCCESSFUL: New velocity is {controller.Velocity}");
            }
        }
    }
}
