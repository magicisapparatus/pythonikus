//********************************************************//
//************ MADE BY DEATROCKER AND OMA ****************//
//***********VISIT www.deatrocker.ucoz.com****************//
//********************************************************//

//���������� ��� ������� ������� ��������
var dropPosition : GameObject; //������� �������
var flashModel : Rigidbody;     //������ �������� (��, ��� ����� ���������)
var dropSound: AudioClip;     //���� �������

//���������� ��� ��������
var linkedLight : Light;    //���� ������������� � ������ (���������� ����� ������ ��������)
var switchon:AudioClip;    //���� ���������
var switchoff:AudioClip;   //���� ����������
var abble:boolean;         //���������� ������� ������� � ��� ��������
var is_on:boolean=false;   //���������� ������� ��������� �������� ��� ��� ����

var maxPower : float = 100;   //������������ ���������� ������ ��������
private var currentPower : float;    //������� �������� ������
var speed : float = 5.0;                //�������� �������� ������
private var alpha : float;               
private var duration : float = 0.2;     
private var baseIntensity : float; 

function Start(){                     //��� ������ ���� ������ ��������� ��������
currentPower=maxPower;
baseIntensity=linkedLight.light.intensity;
}

function Update () {                   //� ������ ����� ���������...

    if(Input.GetButtonUp("Flash") && abble==true){ //���� ������ ������ ���������� �� �������, � ����� ���� ������� � ��� ����
		if (is_on)                    //���� ������� �������, ���������
		{
			flash_off();           //�������� ������� ���������� ��������
			is_on=false;             //������� ��������
		}
		else                          //���� ������� ���������, ��������
		{
			flash_on();               //�������� ������� ��������� ��������
			is_on=true;               //������� �������
linkedLight.light.intensity = baseIntensity; //��� ������ ���� ��� =)
		}	
   }
   
   if(Input.GetButtonUp("Drop_Flash") && abble==true){  //���� ������ ������� ������� ��������
   DropFlash();                                    //�������� ������� ������� ��������
   }
   
//� ������ ������� ���������� ������, ������� ����� ������  
if(currentPower < maxPower/4 && linkedLight.enabled){ 
                var phi : float = Time.time / duration * 2 * Mathf.PI;
                var amplitude : float = Mathf.Cos( phi ) * .5 + baseIntensity;
                linkedLight.light.intensity = amplitude + Random.Range(0.1, 1.0) ;
        }
        linkedLight.light.color = Color(alpha/maxPower, alpha/maxPower, alpha/maxPower, alpha/maxPower);
        alpha = currentPower;  
		
		if (is_on==true) {  //���� ������� �������
			if(currentPower > 0.0) currentPower -= Time.deltaTime * speed;  //���� ������� > 0 ,����� ������� �������
	        if(currentPower <= 0.0) {flash_off(); is_on=false;}   //���� ������� <=0 ,����� ��������� �������
		}
		if (is_on==false) {       //���� ������� ��������
		if(currentPower < maxPower) currentPower += Time.deltaTime * speed/2;  //���������� ����� (�������� �������)
		}

}

function flash_on (){   //������� ��������� ��������
audio.PlayOneShot(switchon);     //������ ���� ���������
linkedLight.enabled =true;        //�������� ���� ��������
}

function flash_off(){     //������� ���������� ��������
audio.PlayOneShot(switchoff);        //������ ���� ����������
linkedLight.enabled =false;          //��������� ���� ��������
}

function Igotflash(){   //����� �� ��������� �������, �� ��� ��������
abble=true;                      //������� ��������
}

function DropFlash(){             //����� ������� �������
audio.PlayOneShot(dropSound);      //������ ���� �������
flash_off();                       //�������� ������� ���������� ��������
is_on=false;                       //��������� �������
abble=false;                       //������� ����������
var object : Rigidbody = Instantiate(flashModel, dropPosition.transform.position, dropPosition.transform.rotation);
	object.velocity = transform.TransformDirection(new Vector3(0, 0, 8));   //����������� �������� �������� (������)
}

//If you want to display percentage of charge just uncomment all lines which under
//function OnGUI () {
//        GUI.Label (Rect(70, Screen.height - 75,150,60), "Battery:   " + currentPower.ToString("F0") + "%");
//}