using Sandbox;

// sample weapon for fire test

// fire_point

public sealed class SampleWeapon01 : Component
{

	[Property] public GameObject PrefabProjectile { get; set; }

	[Property] public SkinnedModelRenderer ModelRenderer { get; set; }

	private GameObject _instantiatedProjectile;

	protected override void OnUpdate()
	{
		if(Input.Pressed("Attack1")){
			// FireProjectile();
		}
	}

	public void FireProjectile(){
		Log.Info("FIRED...");

		if (ModelRenderer == null || PrefabProjectile == null) return;

		var attachmentBone = ModelRenderer.GetAttachmentObject("fire_point");

		if (attachmentBone != null)
    {
			// Clone the prefab directly using the bone's complete WorldTransform
			_instantiatedProjectile = PrefabProjectile.Clone( attachmentBone.WorldTransform );

			// _instantiatedProjectile = PrefabProjectile.Clone();
			// _instantiatedProjectile.WorldPosition = attachmentBone.WorldPosition;
			// _instantiatedProjectile.WorldRotation = attachmentBone.WorldRotation;



		}else{
			Log.Warning("EMPTY ATTACH");
		}

		
	}
}
