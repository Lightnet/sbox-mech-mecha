using Sandbox;
using System;

public sealed class MechaController : Component
{
    [RequireComponent] public Rigidbody Body { get; set; }

    [Property, Title("Movement"), Category("Mecha")]
    public float MoveSpeed { get; set; } = 800f;

    [Property, Title("Turn Speed")]
    public float TurnSpeed { get; set; } = 120f; // Degrees per second-ish via torque

    [Property]
    public float JumpForce { get; set; } = 1200f;

    [Property]
    public float BoostForce { get; set; } = 2000f;//50000

    // Ground check
    [Property]
    public float GroundDistance { get; set; } = 1.2f;

    private bool IsGrounded => Scene.Trace.Ray(WorldPosition, WorldPosition + Vector3.Down * GroundDistance)
        .IgnoreGameObject(GameObject)
        .Run()
				.Hit;

    protected override void OnUpdate()
    {
			// Log.Info($"Body {Body}");
        if (Body is null) return;

        HandleMovement();
        HandleJumpAndBoost();
    }

    private void HandleMovement()
    {
        var input = Input.AnalogMove; // WASD / Left Stick

				Log.Info($"input {input}");

        // Forward movement in local space
        var forwardForce = WorldRotation.Forward * input.y * MoveSpeed;
        var strafeForce = WorldRotation.Right * input.x * MoveSpeed * 0.7f; // less strafe power

        Body.ApplyForce(forwardForce + strafeForce);

        // Turning (torque)
        if (MathF.Abs(input.y) > 0.1f || MathF.Abs(input.x) > 0.1f)
        {
            var turnInput = -input.x; // A/D or left stick X for turning
            Body.ApplyTorque(WorldRotation.Up * turnInput * TurnSpeed * 15f);
        }

        // Optional: simple velocity dampening when no input (feels more "heavy")
        if (input.Length < 0.1f)
        {
            var vel = Body.Velocity;
            vel *= 0.92f; // air/ground friction
            Body.Velocity = vel;
        }
    }

    private void HandleJumpAndBoost()
    {
        if (Input.Pressed("jump") && IsGrounded)
        {
            Body.ApplyImpulse(Vector3.Up * JumpForce);
        }

        // Hold Shift (or whatever you map) for boost
        if (Input.Down("run"))
        {
            Body.ApplyForce(WorldRotation.Forward * BoostForce);
        }
    }

    // Optional: visual feedback or stabilization
    protected override void OnFixedUpdate()
    {
        // Keep the mecha somewhat upright (optional stabilizer)
        // You can also use UprightJoint component from the editor for this
    }
}