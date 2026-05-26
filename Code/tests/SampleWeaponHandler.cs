using Sandbox;


// Model Renderer (Skinned)
// create attachments [x] check, by default false

public sealed class SampleWeaponHandler : Component
{

	// Reference to the character's SkinnedModelRenderer
    [Property] public SkinnedModelRenderer CharacterRenderer { get; set; }
    
    // Reference to the weapon prefab or game object
    [Property] public GameObject WeaponPrefab { get; set; }

    [Property] public string BoneName { get; set; } = "Hand_R";

    private GameObject _instantiatedWeapon;

    protected override void OnStart()
    {
        if (CharacterRenderer == null || WeaponPrefab == null) return;

        // Clone the prefab
		// var spawnedObject = WeaponPrefab.Clone();
		// // Find the bone on the SkinnedModelRenderer
		// var skinnedRenderer = Components.Get<SkinnedModelRenderer>( FindMode.InChildren );
		// if ( skinnedRenderer != null && skinnedRenderer.TryGetBoneTransform( BoneName, out var boneTransform ) )
		// {
        //     Log.Info($"test {boneTransform}");
		// 	// Attach the spawned object to the bone's transform
		// 	// spawnedObject.Transform.SetParent( boneTransform );
        //     spawnedObject.Parent = CharacterRenderer.GameObject;
		// 	spawnedObject.LocalPosition = Vector3.Zero;
		// 	spawnedObject.LocalRotation = Rotation.Identity;
		// }



        // 1. Spawn the weapon game object
        _instantiatedWeapon = WeaponPrefab.Clone();

        // 2. Set the character renderer as the bone/attachment source
        // _instantiatedWeapon.Parent = CharacterRenderer.GameObject;
        // Log.Info("TEST");

        // // Note: If "hand_R_weapon" is an attachment point rather than a bone name,
        // // ensure it is properly exposed or look for the bone itself (e.g., "hand_R").
        // // var attachmentBone = CharacterRenderer.GetBoneObject( "hand_R_weapon" );
        // // var attachmentBone = CharacterRenderer.GetBoneObject("Hand_R");
        var attachmentBone = CharacterRenderer.GetAttachmentObject("hand_r_weapon");
        // var isAttachment = CharacterRenderer.GetAttachment("hand_r_weapon");

        // // CharacterRenderer.get

        // Log.Info($"isAttachment { isAttachment }");
        // Log.Info($"attachmentBone { attachmentBone }");

        if (attachmentBone != null)
        {
            // 3. Parent the weapon directly to the bone object
            _instantiatedWeapon.Parent = attachmentBone;

            // 4. Reset local transform to zero out offsets and snap perfectly into place
            _instantiatedWeapon.LocalPosition = Vector3.Zero;
            _instantiatedWeapon.LocalRotation = Rotation.Identity;
        }
        else
        {
            Log.Warning( "Could not find attachment or bone: hand_R_weapon" );
            
            // Fallback: parent to root character if bone isn't found
            _instantiatedWeapon.Parent = CharacterRenderer.GameObject;
        }



        // // 3. Lock it to the specific attachment point created in ModelEditor
        // if (_instantiatedWeapon.Components.TryGet<ModelRenderer>(out var weaponRenderer))
        // {
        //     // Directs s&box to snap the object's origin to the named attachment 
        //     // and continuously inherit all bones' transformations during animations.
        //     _instantiatedWeapon.LocalPosition = Vector3.Zero;
        //     _instantiatedWeapon.LocalRotation = Rotation.Identity;
            
        //     // Assign the attachment property via the SetParent/SetBone framework
        //     // dependent on your weapon system base structure:
        //     _instantiatedWeapon.Tags.Add( "held" ); 
        // }
        
        // Alternative approach using native component hierarchy pairing:
        // You can pin components directly to named attachments using:
        // _instantiatedWeapon.WorldPosition = CharacterRenderer.GetAttachment( "hand_R_weapon" )?.Position ?? Vector3.Zero;
    }

	protected override void OnUpdate()
	{

        if(Input.Pressed("Attack1")){
            if(_instantiatedWeapon !=null){
                var sampleWeapon = _instantiatedWeapon.GetComponent<SampleWeapon01>();

                if(sampleWeapon.IsValid()){
                    sampleWeapon.FireProjectile();
                }
            }
        }
	}

}
