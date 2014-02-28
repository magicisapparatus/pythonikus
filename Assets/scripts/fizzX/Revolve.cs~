using UnityEngine;
using System.Collections;

public class Revolve : MonoBehaviour 
{
	GameObject sun;
	public Transform center;
	public Vector3 axis=Vector3.up;
	public Vector3 desiredPosition;
	public float radius=2.0f;
	public float radiusSpeed=0.5f;
	public float rotationSpeed=80.0f;
	
	// Use this for initialization
	void Start () 
	{
		sun=GameObject.FindWithTag("");
		transform.position=(transform.position-center.position).normalized*
			radius+center.position;
		radius=2.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.RotateAround(center.position,axis,rotationSpeed*Time.deltaTime);
		desiredPosition=(transform.position-center.position).normalized*
			radius+center.position;
		transform.position=Vector3.MoveTowards (transform.position,desiredPosition,
			Time.deltaTime*radiusSpeed);
	}
}
