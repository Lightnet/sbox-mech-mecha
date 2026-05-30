using Sandbox;

public sealed class SampleRearWeapon01 : Component
{

  [Property] public GameObject PrefabProjectile { get; set; }
  [Property] public SkinnedModelRenderer ModelRenderer { get; set; }

  [Property] public GameObject PHFirePoint { get; set; }

  [Property] public string hot_key { get; set; } = "RearLeftWeapon1";

  private GameObject _instantiatedProjectile;


  protected override void OnStart()
	{

  }

  protected override void OnUpdate()
	{
    if(Input.Pressed(hot_key)){
			FireProjectile();
		}
  }

  public void FireProjectile(){
		Log.Info("REAR FIRED...");
    if (PHFirePoint == null || PrefabProjectile == null) return;

    _instantiatedProjectile = PrefabProjectile.Clone( PHFirePoint.WorldTransform );
  }
}