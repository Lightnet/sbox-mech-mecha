using Sandbox;

public sealed class CustomController : Component
{
    // Expose speed and jump force properties to the Inspector
    [Property] public float WalkSpeed { get; set; } = 160.0f;
    [Property] public float JumpForce { get; set; } = 300.0f;
    [Property] public float Gravity { get; set; } = 800.0f;

    // Grab the built-in Character Controller
    private CharacterController controller;

    private bool isJump = false;
    protected override void OnAwake()
    {
        controller = Components.Get<CharacterController>();
    }

    protected override void OnFixedUpdate()
    {
        if (controller == null) return;

        // Apply Gravity
        // if (!controller.IsGrounded)
        if (!controller.IsOnGround)
        {
            controller.Velocity += Vector3.Down * Gravity * Time.Delta;
        }

        // Handle Input & Movement
        BuildMoveInput();

        // Move the controller using physics tracing
        controller.Move();
    }

    private void BuildMoveInput()
    {
        // Get the camera's world rotation
        Rotation cameraRotation = Scene.Camera.Transform.Rotation;

        // Strip pitch and roll so moving forward doesn't push you into the ground
        Rotation forwardRotation = Rotation.FromYaw(cameraRotation.Yaw());

        // Multiply the rotation by the local analog input vector
        Vector3 wishDirection = forwardRotation * Input.AnalogMove.Normal;

        // Scale by speed to get the final wish velocity
        Vector3 wishVelocity = wishDirection * WalkSpeed;

        // Apply the horizontal movement while preserving your current gravity/jump Z velocity
        controller.Velocity = new Vector3(wishVelocity.x, wishVelocity.y, controller.Velocity.z);

        // Handle Jump (Kept clean for standard velocity pushing)
        // if (controller.IsOnGround && Input.Pressed("Jump"))
        if (Input.Pressed("Jump"))
        {
            isJump = true;
            Log.Info("JUMP...");
            // controller.Velocity = controller.Velocity.WithZ(JumpForce);
            // Log.Info($"JUMP applied. New velocity: {controller.Velocity}");
        }

        // if((controller.IsOnGround == true) && (isJump == true)){
        //     isJump = false;
        // }
        // controller.Velocity = controller.Velocity.WithZ(JumpForce);
        if(isJump == true){
            Log.Info("JUMP.here...");
            // controller.Velocity = controller.Velocity.WithZ(JumpForce);// does not work here
        }

        controller.Velocity = controller.Velocity.WithZ(JumpForce); // work here
        



        // Get WASD/Controller movement wish direction
        // Vector3 wishVelocity = Input.AnalogMove.Normal;
        
        // Ensure we move relative to our camera's rotation (yaw)
        // wishVelocity = wishVelocity.EulerAngles.WithPitch(0).ToRotation() * wishVelocity;
        // wishVelocity *= WalkSpeed;

        // Set the wish velocity for the controller
        // controller.WishVelocity = wishVelocity;
        // controller.Velocity = wishVelocity;
        // controller.Accelerate(Vector3.Up * 2000);


        // Handle Jump
        // if (controller.IsOnGround && Input.Pressed("Jump"))
        // {
        //     // controller.PunchVelocity(Vector3.Up * JumpForce);
        //     // controller.Punch( Vector3.Up * JumpForce );
        //     // controller.Velocity = controller.Velocity.WithZ( JumpForce * 100);
        //     Log.Info($"JUMP {controller.Velocity}");
        //     // controller.Jump(JumpForce);

        //     // controller.Accelerate(Vector3.Up * 300);
        //     controller.Accelerate(Vector3.Up * 20000); 
            
        // }

        // if(controller.IsOnGround == false && Input.Pressed("Jump")){
        //     controller.Accelerate(Vector3.Up * 20000); 
        // }
    }
}
