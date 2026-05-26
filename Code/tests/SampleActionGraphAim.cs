using Sandbox;

public sealed class SampleActionGraphAim : Component
{
	// Reference your model renderer in the Inspector
  [Property] public SkinnedModelRenderer Model { get; set; }

	// A float variable to track speed
  [Property] public float AimDegree { get; set; } = 0.0f;

	protected override void OnUpdate()
	{
		if (Model == null) return;
		// Set the float parameter inside your model's AnimGraph directly
		// Model.SetAnimParameter() replace with Model.Set()
    Model.Set("aim_degree", AimDegree);
	}
}
