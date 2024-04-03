#pragma strict
import UnityEngine.UI;
import UnityEngine.EventSystems;

var hits : RaycastHit[];
 var ground : Collider;
 var height : float;
 //var height2 : string;
 var stringToEdit : String ;
 


//These slots are where you will plug in the appropriate robot parts into the inspector.
var RobotBase : Transform;
var RobotUpperArm : Transform;

//These allow us to have numbers to adjust in the inspector for the speed of each part's rotation.
var baseTurnRate : float = 5;
var upperArmTurnRate : float = 5;

private var robotBaseYRot : float;
public var robotBaseYRotMin : float = -45f;
public var robotBaseYRotMax : float = 45f;
  static var pointerDown = false;
 

//var btn : GameObject = GameObject.FindGameObjectWithTag("Left1");
//var btn : UI.Button;
 public var btn : Button;


  



	
function Update(){	

//btn.GetComponent(UI.Button).onClick.AddListener(ButtonClicked);
     btn.onClick.AddListener(ButtonClicked);
		



hits=Physics.RaycastAll (transform.position, Vector3.down , Mathf.Infinity);

 {
     for( var hit : RaycastHit in hits ) 
     {
         if( hit.collider.name == "Terrain" )
         {
             //height = transform.position.y - hit.point.y;//seems to work aswell
             height = hit.distance;
            
			 stringToEdit = height.ToString();
         }
     }
 }  

//rotating our base of the robot here around the Y axis and multiplying
//the rotation by the slider's value and the turn rate for the base.
//RobotBase.Rotate (0, robotBaseSliderValue * baseTurnRate, 0);

  
 if(Input.GetKey(KeyCode.LeftArrow))  
 if(height>35f)
robotBaseYRot += 1 * baseTurnRate;
robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);

 
if(Input.GetKey(KeyCode.RightArrow))
if(height>35f)
robotBaseYRot -= 1 * baseTurnRate;
robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);


if(Input.GetKey(KeyCode.UpArrow)) 
if(robotBaseYRot>50f)
RobotUpperArm.Translate(new Vector3(0, 1, 0) * upperArmTurnRate * Time.deltaTime);

if(Input.GetKey(KeyCode.DownArrow)) 
if(robotBaseYRot>50f)
RobotUpperArm.Translate(new Vector3(0, 1, 0) * -upperArmTurnRate * Time.deltaTime);
 


 

//rotating our upper arm of the robot here around the X axis and multiplying
//the rotation by the slider's value and the turn rate for the upper arm.
//RobotUpperArm.Rotate (robotUpperArmSliderValue * upperArmTurnRate, 0 , 0);


}


function Left()
  {

  if(height>35f)
robotBaseYRot += 1 * baseTurnRate;
robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);

    
  }

  function Right()
  {

if(height>35f)
robotBaseYRot -= 1 * baseTurnRate;
robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);

    
  }


function Up()
  {

 if(robotBaseYRot>50f)
RobotUpperArm.Translate(new Vector3(0, 1, 0) * upperArmTurnRate * Time.deltaTime);
    
  }


function Down()
  {

if(robotBaseYRot>50f)
RobotUpperArm.Translate(new Vector3(0, 1, 0) * -upperArmTurnRate * Time.deltaTime);
 
    
  }
  


   function ButtonClicked ()
{


if(height>35f)
robotBaseYRot += 1 * baseTurnRate;
robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);
print("asd");
   
    
}




