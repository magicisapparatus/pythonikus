//********************************************************//
//************ MADE BY DEATROCKER AND OMA ****************//
//********************************************************//

private var hit : RaycastHit;
private var rayDistance : float;
private var contactPoint : Vector3;
var PickupSound: AudioClip;
	
function OnTriggerEnter (other : Collider) {     //При касании коллайдера с тегом 
    if (other.CompareTag ("Flashlight")) {        //Flashlight
	   audio.PlayOneShot(PickupSound);        //Играем звук поднятия
       SendMessage("Igotflash");                //Сообщаем, что подняли фонарик
	   BroadcastMessage("Igotflash");
	   Destroy(other.gameObject);                //Уничтожаем модель
	} 
}