#pragma strict


//These slots are where you will plug in the appropriate robot parts into the inspector.
var RobotBase : Transform;


//These allow us to have numbers to adjust in the inspector for the speed of each part's rotation.
var baseTurnRate : float = 5;


private var robotBaseYRot : float;
public var robotBaseYRotMin : float = -45f;
public var robotBaseYRotMax : float = 45f;



function Update(){

//rotating our base of the robot here around the Y axis and multiplying
//the rotation by the slider's value and the turn rate for the base.
//RobotBase.Rotate (0, robotBaseSliderValue * baseTurnRate, 0);

 if(Input.GetKey(KeyCode.O))     
robotBaseYRot += 1 * baseTurnRate;
robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
RobotBase.eulerAngles = new Vector3(robotBaseYRot, RobotBase.eulerAngles.y, RobotBase.eulerAngles.z);

if(Input.GetKey(KeyCode.P))
robotBaseYRot -= 1 * baseTurnRate;
robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
RobotBase.eulerAngles = new Vector3(robotBaseYRot, RobotBase.eulerAngles.y,  RobotBase.eulerAngles.z);


 

//rotating our upper arm of the robot here around the X axis and multiplying
//the rotation by the slider's value and the turn rate for the upper arm.
//RobotUpperArm.Rotate (robotUpperArmSliderValue * upperArmTurnRate, 0 , 0);


  

}

