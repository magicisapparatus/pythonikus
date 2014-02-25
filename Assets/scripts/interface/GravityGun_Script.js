#pragma strict

var catchRange = 30.0;
var holdDistance = 4.0;
var minForce = 1000;
var maxForce = 10000;
var forceChargePerSec = 3000;
var layerMask : LayerMask = -1;
     
enum GravityGunState { Free, Catch, Occupied, Charge, Release};
private var gravityGunState : GravityGunState = 0;
private var rigid : Rigidbody = null;
private var currentForce = minForce;
     
function FixedUpdate() 
{
   if(gravityGunState == GravityGunState.Free) 
   {
     if(Input.GetButton("Fire1")) 
     {
     	var hit : RaycastHit;
        if(Physics.Raycast(transform.position, transform.forward, hit, catchRange, layerMask)) 
        {
        	if(hit.rigidbody) 
        	{
        		rigid = hit.rigidbody;
            	gravityGunState = GravityGunState.Catch;
         	}
        }
      }
    }

		else if(gravityGunState == GravityGunState.Catch) 
        {
            rigid.MovePosition(transform.position + transform.forward * holdDistance);
            if(!Input.GetButton("Fire1"))
                gravityGunState = GravityGunState.Occupied;
        }
        
        else if(gravityGunState == GravityGunState.Occupied) 
        {
            rigid.MovePosition(transform.position + transform.forward * holdDistance);
            if(Input.GetButton("Fire1"))
            gravityGunState = GravityGunState.Charge;
        }
        
        else if(gravityGunState == GravityGunState.Charge) 
        {
            rigid.MovePosition(transform.position + transform.forward * holdDistance);
            if(currentForce < maxForce) {
                currentForce += forceChargePerSec * Time.deltaTime;
         }
         
         else 
         {
                currentForce = maxForce;
         }
         
         if(!Input.GetButton("Fire1"))
         gravityGunState = GravityGunState.Release;     
        }
        
        else if(gravityGunState == GravityGunState.Release) 
        {
            rigid.AddForce(transform.forward * currentForce);
            currentForce = minForce;
            gravityGunState = GravityGunState.Free;
        }
    }
     
    @script ExecuteInEditMode()