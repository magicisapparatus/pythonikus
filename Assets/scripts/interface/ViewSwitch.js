#pragma strict

var FPScamera : Camera;
var TPcamera : Camera;

private var camswitch : boolean = false;

function Start()
{
	FPScamera.camera.enabled = true;
	TPcamera.camera.enabled = false;
}

function Update()
{
	if(Input.GetKeyDown("c"))
	{
		camswitch = !camswitch;
	}
	
	if(camswitch == true)
	{
		FPScamera.camera.enabled = false;
		TPcamera.camera.enabled = true;
	}
	
	else
	{
		FPScamera.camera.enabled = true;
		TPcamera.camera.enabled = false;
	}
}