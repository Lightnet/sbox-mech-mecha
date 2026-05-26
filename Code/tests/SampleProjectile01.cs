using Sandbox;

public sealed class SampleProjectile01 : Component
{
	// Adjust this to change how fast the bullet travels
	[Property] public float Speed { get; set; } = 1000.0f;
	[Property] public float LifeSpan { get; set; } = 3.0f;

	private TimeSince _spawnedTime;
	protected override void OnStart()
	{
		// Reset the timer when the projectile is spawned
		_spawnedTime = 0;
	}

	protected override void OnUpdate()
	{
			// Move the bullet forward based on its local rotation
			// We multiply by Time.Delta to ensure speed is consistent regardless of framerate
			WorldPosition += Transform.Local.Forward * Speed * Time.Delta;
			// If the elapsed time is greater than our lifespan, delete the object
			if ( _spawnedTime > LifeSpan )
			{
				GameObject.Destroy();
			}
	} 
}
