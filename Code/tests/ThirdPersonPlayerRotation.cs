using Sandbox;
using System;

public sealed class ThirdPersonPlayerRotation : Component
{
    [Property] public GameObject CameraObject { get; set; }
    [Property] public float RotationSpeed { get; set; } = 10f;

    protected override void OnUpdate()
    {
        if ( CameraObject == null ) return;

        // 1. Get the current eye angles / rotation of your camera
        Angles cameraAngles = CameraObject.WorldRotation.Angles();

        // 2. Isolate the Yaw (horizontal) angle and zero out pitch/roll 
        // This ensures the player doesn't lean up or down when looking at the sky
        Angles targetYaw = new Angles( 0, cameraAngles.yaw, 0 );

        // 3. Convert the angles to a Quaternion Rotation
        Rotation targetRotation = Rotation.From( targetYaw );

        // 4. Smoothly interpolate the player's base toward the camera's horizontal angle
        // Use Lerp or Slerp for a smooth turn, or set directly for an instant snap
        GameObject.WorldRotation = Rotation.Lerp( GameObject.WorldRotation, targetRotation, Time.Delta * RotationSpeed );
    }
}
