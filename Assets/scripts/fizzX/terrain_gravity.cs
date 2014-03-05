using UnityEngine;
using System.Collections;

using Cubiquity;

public class terrain_gravity : MonoBehaviour
{
	public GameObject Item;
	public float Range=3500f;
	private Ray Lulz;
	private RaycastHit hitObject;
	public TerrainVolume volume;

	// Use this for runtime initialization lulz
	void Start ()
	{
        //start with no gravirty
		Item.rigidbody.useGravity=false;


        //and check if a terrain volume is being used with this script :3
        if (volume==null)
        {
            Debug.LogError("This 'ClickToCarveTerrainVolume' script should be attached to a game object with a TerrainVolume component");
        }
	}
	
	// Update is called once per frame i <3 this thingy
	void Update ()
	{
		//this is to check if the terrain is near (underneath was the plan but why not near eh?
		Lulz.origin=(Item.transform.position);
		Lulz.direction=
			(
				new Vector3
				(
				Item.transform.position.x,
				(-Item.transform.position.y),
				Item.transform.position.z
				)
			);
		//this ends the part getting the ray where it will check if its over
		//the things with the terrain voxels


		// Perform the raycasting.
		PickSurfaceResult pickResult;
		bool hit=Picking.PickSurface(volume,Lulz,Range,out pickResult);
		
		// If we hit a solid voxel then enable the gravity
		
		if(hit)
		{
			Item.rigidbody.useGravity=true;
            Debug.DrawLine(Lulz.origin, Lulz.direction);
		}

		else
		{
			Item.rigidbody.useGravity=false;
		}
	}
}
