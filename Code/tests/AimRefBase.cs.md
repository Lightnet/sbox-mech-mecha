using Sandbox;

public class AimRefBase : Component
{
    // Properties exposed to the Inspector
    [Property] public CameraComponent Camera { get; set; }
    [Property] public float MaxDistance { get; set; } = 5000f;
    [Property] public bool ShowDebugDraw { get; set; } = true;
    [Property] public Color LineColor { get; set; } = Color.Cyan;

    // Track the results globally for other weapon/interaction systems to access
    public SceneTraceResult LastTraceResult { get; private set; }
    public bool DidHit => LastTraceResult.Hit;

    protected override void OnUpdate()
    {
        // Fallback if the camera reference is missing
        var mainCamera = Camera ?? Scene.Camera;
        if ( mainCamera == null ) return;

        // Formulate a forward Ray precisely out of the center of the camera
        // (Alternatively, use ScreenPixelToRay via viewport center coordinates)
        Ray lookRay = new Ray( mainCamera.WorldPosition, mainCamera.WorldRotation.Forward );

        // Run the Scene-based physics trace
        LastTraceResult = Scene.Trace
            .Ray( lookRay, MaxDistance )
            .Size( 0f ) // Pass a thickness value (e.g. 2f) if you want a box/sphere sweep instead of a pixel-perfect line
            .IgnoreGameObjectHierarchy( GameObject ) // Ignore the local player avatar hierarchy
            .WithoutTags( "player" ) // Customize collision layer tags as needed
            .Run();

        // Render visual debug updates onto the screen layout
        if ( ShowDebugDraw )
        {
            DrawDebugVisuals( lookRay );
        }
    }

    private void DrawDebugVisuals( Ray ray )
    {
        // Use the native s&box engine overlay to trace and display the contact points
        // It automatically handles drawing hit normal indicators and impact spheres
        Gizmo.Draw.Color = LineColor;
        
        Vector3 traceEndPoint = DidHit ? LastTraceResult.HitPosition : ray.Project( MaxDistance );

        // Draw the core vector line
        Gizmo.Draw.Line( ray.Position, traceEndPoint );

        if ( DidHit )
        {
            // Draw a small solid wireframe box/sphere at the point of impact
            Gizmo.Draw.LineSphere( LastTraceResult.HitPosition, 4f );
            
            // Draw a tiny normal vector line jumping away from the surface
            Gizmo.Draw.Line( LastTraceResult.HitPosition, LastTraceResult.HitPosition + ( LastTraceResult.Normal * 20f ) );
        }
    }
}
