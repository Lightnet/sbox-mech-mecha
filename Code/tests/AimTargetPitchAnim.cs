using Sandbox;
using System;

public sealed class AimTargetPitchAnim : Component
{

  // Properties exposed to the Inspector
  [Property] public CameraComponent Camera { get; set; }
  [Property] public float MaxDistance { get; set; } = 5000f;
  [Property] public bool ShowDebugDraw { get; set; } = true;
  [Property] public Color LineColor { get; set; } = Color.Cyan;

  [Property] public float TargetPitch { get; set; } = 0f;

  [Property] public GameObject WeaponHandle { get; set; }

  [Property] public SkinnedModelRenderer Model { get; set; }


  // Track the results globally for other weapon/interaction systems to access
  public SceneTraceResult LastTraceResult { get; private set; }
  public bool DidHit => LastTraceResult.Hit;

  protected override void OnUpdate()
  {
    // Fallback if the camera reference is missing
    var mainCamera = Camera ?? Scene.Camera;
    if ( mainCamera == null ) return;

    // Formulate a forward Ray precisely out of the center of the camera
    Ray lookRay = new Ray( mainCamera.WorldPosition, mainCamera.WorldRotation.Forward );

    // Run the Scene-based physics trace
    LastTraceResult = Scene.Trace
      .Ray( lookRay, MaxDistance )
      .IgnoreGameObjectHierarchy( GameObject )
      .WithoutTags( "player" )
      .Run();
    
    // DETERMINE THE TARGET POSITION
    // If it hits nothing, project the point out into space along the camera's forward vector
    Vector3 finalAimPosition = DidHit ? LastTraceResult.HitPosition : lookRay.Project( MaxDistance );

    // Render visual debug updates onto the screen layout using our final calculated target
    if ( ShowDebugDraw )
    {
      DrawDebugVisuals( lookRay.Position, finalAimPosition );
    }

    if (Model == null) return;

    if ( DidHit ){
      // LastTraceResult.HitPosition
      // dir =  LastTraceResult.HitPosition - WeaponHandle.WorldPosition

      // get pitch 
      if ( WeaponHandle == null ) return;

      // Direction from weapon muzzle/handle to target
      Vector3 dir = (finalAimPosition - WeaponHandle.WorldPosition).Normal;

      // Pitch relative to world (positive = looking up)
      // float pitch = MathF.Asin( dir.z ) * MathX.RadToDeg;   // or -dir.z depending on coordinate convention
      float pitch = MathX.RadianToDegree( MathF.Asin( dir.z ) ); 

      // If you want pitch relative to the weapon's current forward (local)
      // Vector3 localDir = WeaponHandle.WorldRotation.Inverse * dir;
      // Log.Info($"pitch {pitch}");
      Model.Set("aim_degree", pitch);
    }else{
      // endPos
      // dir =  endPos - WeaponHandle.WorldPosition

      // get pitch 

      if ( WeaponHandle == null ) return;
      Vector3 dir = (finalAimPosition - WeaponHandle.WorldPosition).Normal;
      float pitch = MathX.RadianToDegree( MathF.Asin( dir.z ) ); 
      // Log.Info($"pitch {pitch}");
      Model.Set("aim_degree", pitch);
    }


  }

  private void DrawDebugVisuals( Vector3 startPos, Vector3 endPos )
  {
    Gizmo.Draw.Color = LineColor;
    
    // This will now always draw straight ahead to the MaxDistance if nothing is hit
    Gizmo.Draw.Line( startPos, endPos );

    // Only draw impact indicators if a physical surface was actually struck
    if ( DidHit )
    {
      Gizmo.Draw.LineSphere( LastTraceResult.HitPosition, 4f );
      Gizmo.Draw.Line( LastTraceResult.HitPosition, LastTraceResult.HitPosition + ( LastTraceResult.Normal * 20f ) );
    }else{
      Gizmo.Draw.LineSphere( endPos, 4f );
    }
  }

  // end class AimTargetPitch
}