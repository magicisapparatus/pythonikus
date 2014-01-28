//********************************************************//
//************ MADE BY DEATROCKER AND OMA ****************//
//***********VISIT www.deatrocker.ucoz.com****************//
//********************************************************//

//Переменные для события выброса фонарика
var dropPosition : GameObject; //Позиция выброса
var flashModel : Rigidbody;     //Модель фонарика (то, что будет выброшено)
var dropSound: AudioClip;     //Звук выброса

//Переменные для фонарика
var linkedLight : Light;    //Свет прикрепленный к игроку (являющийся якобы светом фонарика)
var switchon:AudioClip;    //Звук включения
var switchoff:AudioClip;   //Звук выключения
var abble:boolean;         //Переменная условия наличия у нас фонарика
var is_on:boolean=false;   //Переменная условия состояния фонарика вкл или выкл

var maxPower : float = 100;   //Максимальное количество заряда фонарика
private var currentPower : float;    //Текущее значение заряда
var speed : float = 5.0;                //Скорость убывания заряда
private var alpha : float;               
private var duration : float = 0.2;     
private var baseIntensity : float; 

function Start(){                     //При старте игры задаем начальные значения
currentPower=maxPower;
baseIntensity=linkedLight.light.intensity;
}

function Update () {                   //В каждом кадре выполняем...

    if(Input.GetButtonUp("Flash") && abble==true){ //Если нажали кнопку отвечающую за фонарик, а также если фонарик у нас есть
		if (is_on)                    //Если фонарик включен, выключаем
		{
			flash_off();           //Вызываем функцию выключения фонарика
			is_on=false;             //Фонарик отключен
		}
		else                          //Если фонарик невключен, включаем
		{
			flash_on();               //Вызываем функцию включения фонарика
			is_on=true;               //Фонарик включен
linkedLight.light.intensity = baseIntensity; //Это должно быть тут =)
		}	
   }
   
   if(Input.GetButtonUp("Drop_Flash") && abble==true){  //Если нажали клавишу выброса фонарика
   DropFlash();                                    //Вызываем функцию выброса фонарика
   }
   
//В случае низкого количества заряда, фонарик будет мигать  
if(currentPower < maxPower/4 && linkedLight.enabled){ 
                var phi : float = Time.time / duration * 2 * Mathf.PI;
                var amplitude : float = Mathf.Cos( phi ) * .5 + baseIntensity;
                linkedLight.light.intensity = amplitude + Random.Range(0.1, 1.0) ;
        }
        linkedLight.light.color = Color(alpha/maxPower, alpha/maxPower, alpha/maxPower, alpha/maxPower);
        alpha = currentPower;  
		
		if (is_on==true) {  //Если фонарик включен
			if(currentPower > 0.0) currentPower -= Time.deltaTime * speed;  //Если энергия > 0 ,тогда энергия убывает
	        if(currentPower <= 0.0) {flash_off(); is_on=false;}   //Если энергия <=0 ,тогда выключаем фонарик
		}
		if (is_on==false) {       //Если фонарик выключен
		if(currentPower < maxPower) currentPower += Time.deltaTime * speed/2;  //Прибавляем заряд (заряжаем фонарик)
		}

}

function flash_on (){   //Функция включения фонарика
audio.PlayOneShot(switchon);     //Играем звук включения
linkedLight.enabled =true;        //Включаем свет фонарика
}

function flash_off(){     //Функция выключения фонарика
audio.PlayOneShot(switchoff);        //Играем звук выключения
linkedLight.enabled =false;          //Выключаем свет фонарика
}

function Igotflash(){   //Когда мы подбираем фонарик, он нам сообщает
abble=true;                      //Фонарик доступен
}

function DropFlash(){             //Когда бросаем фонарик
audio.PlayOneShot(dropSound);      //Играем Звук выброса
flash_off();                       //Вызываем функцию отключения фонарика
is_on=false;                       //ВЫключаем фонарик
abble=false;                       //Фонарик недоступен
var object : Rigidbody = Instantiate(flashModel, dropPosition.transform.position, dropPosition.transform.rotation);
	object.velocity = transform.TransformDirection(new Vector3(0, 0, 8));   //Выбрасываем модельку фонарика (префаб)
}

//If you want to display percentage of charge just uncomment all lines which under
//function OnGUI () {
//        GUI.Label (Rect(70, Screen.height - 75,150,60), "Battery:   " + currentPower.ToString("F0") + "%");
//}