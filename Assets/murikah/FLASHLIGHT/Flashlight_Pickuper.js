//********************************************************//
//************ MADE BY DEATROCKER AND OMA ****************//
//********************************************************//

private var hit : RaycastHit;
private var rayDistance : float;
private var contactPoint : Vector3;
var PickupSound: AudioClip;
	
function OnTriggerEnter (other : Collider) {     //��� ������� ���������� � ����� 
    if (other.CompareTag ("Flashlight")) {        //Flashlight
	   audio.PlayOneShot(PickupSound);        //������ ���� ��������
       SendMessage("Igotflash");                //��������, ��� ������� �������
	   BroadcastMessage("Igotflash");
	   Destroy(other.gameObject);                //���������� ������
	} 
}