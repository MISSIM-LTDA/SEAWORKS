#pragma strict


//These slots are where you will plug in the appropriate robot parts into the inspector.
var RobotBase : Transform;

//These allow us to have numbers to adjust in the inspector for the speed of each part's rotation.
var baseTurnRate : float = 5;

private var robotBaseYRot : float;
public var robotBaseYRotMin : float = 0f;
public var robotBaseYRotMax : float = 45f;



function Update(){

//rotating our base of the robot here around the Y axis and multiplying
//the rotation by the slider's value and the turn rate for the base.
//RobotBase.Rotate (0, robotBaseSliderValue * baseTurnRate, 0);

  if(Input.GetKey(KeyCode.UpArrow)) 
//robotBaseYRot += 1 * baseTurnRate;
//robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
//RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);
//RobotBase.Translate = new Vector3(RobotBase.Translate, robotBaseYRot, RobotBase.Translate.z);
RobotBase.Translate(new Vector3(0, 1, 0) * baseTurnRate * Time.deltaTime);

if(Input.GetKey(KeyCode.DownArrow)) 
RobotBase.Translate(new Vector3(0, 1, 0) * -baseTurnRate * Time.deltaTime);
 

//rotating our upper arm of the robot here around the X axis and multiplying
//the rotation by the slider's value and the turn rate for the upper arm.
//RobotUpperArm.Rotate (robotUpperArmSliderValue * upperArmTurnRate, 0 , 0);


  

}

