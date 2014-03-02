﻿using UnityEngine;
using System.Collections;

public class terrain_gravity : MonoBehaviour
{
	public GameObject Item;
	public Rigidbody Item_rigidbody;
	public Transform Item_transform;
	private Ray Lulz;

	// Use this for initialization lulz
	void Start ()
	{
		Item_rigidbody.useGravity=false;
	}
	
	// Update is called once per frame i <3 this thingy
	void Update ()
	{
		//this is to check if the terrain is near (underneath was the plan but why not near eh?
		Lulz.origin=(Item_transform.position);
		Lulz.direction=
			(
				new Vector3
				(
				Item_transform.position.x,
				Item_transform.position.y-2,
				Item_transform.position.z
				)
			);

	}
}
