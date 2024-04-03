using System.Collections;
using CnControls;
using System.Collections.Generic;
using UnityEngine;

// From the Unity AngryBots Demo
// We have added the ability to handle underwater effects, jumping and swimming in this script
public class LarsControl : MonoBehaviour
{


    public float height;
    public RaycastHit[] hits;
    public Collider ground;


    //These slots are where you will plug in the appropriate robot parts into the inspector.
    public Transform RobotBase;
    public Transform RobotUpperArm;


    //These allow us to have numbers to adjust in the inspector for the speed of each part's rotation.
    public float baseTurnRate = 5;
    public float upperArmTurnRate = 5;
    private float robotBaseYRot;
    public float robotBaseYRotMin = -45f;
    public float robotBaseYRotMax = 45f;


    void Update()
    {

        //rotating our base of the robot here around the Y axis and multiplying
        //the rotation by the slider's value and the turn rate for the base.
        //RobotBase.Rotate (0, robotBaseSliderValue * baseTurnRate, 0);

        
        bool held = CnInputManager.GetButton("Left2");
        bool held2 = CnInputManager.GetButton("Right2");
        bool held3 = CnInputManager.GetButton("Down2");
        bool held4 = CnInputManager.GetButton("Up2");


        if (held)
        {
            if (height > 35f)
            {
                robotBaseYRot += 1 * baseTurnRate;
                robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
                RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);
            }
        }

        if (held2)
        {
            if (height > 35f)
            {
                robotBaseYRot -= 1 * baseTurnRate;
                robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
                RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);
            }
        }

        if (held3)
        {
            if (robotBaseYRot > 50f)
            {
                RobotUpperArm.Translate(new Vector3(0, 1, 0) * -upperArmTurnRate * Time.deltaTime);
            }
        }

        if (held4)
        {
            if (robotBaseYRot > 50f)
            {
                RobotUpperArm.Translate(new Vector3(0, 1, 0) * upperArmTurnRate * Time.deltaTime);
            }
        }


        if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (height > 35f)
                {
                    robotBaseYRot += 1 * baseTurnRate;
                    robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
                    RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (height > 35f)
                {
                    robotBaseYRot -= 1 * baseTurnRate;
                    robotBaseYRot = Mathf.Clamp(robotBaseYRot, robotBaseYRotMin, robotBaseYRotMax);
                    RobotBase.eulerAngles = new Vector3(RobotBase.eulerAngles.x, robotBaseYRot, RobotBase.eulerAngles.z);
                }
            }


            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (robotBaseYRot > 50f)
                {
                    RobotUpperArm.Translate(new Vector3(0, 1, 0) * upperArmTurnRate * Time.deltaTime);
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (robotBaseYRot > 50f)
                {
                    RobotUpperArm.Translate(new Vector3(0, 1, 0) * -upperArmTurnRate * Time.deltaTime);
                }
            }




            //rotating our upper arm of the robot here around the X axis and multiplying
            //the rotation by the slider's value and the turn rate for the upper arm.
            //RobotUpperArm.Rotate (robotUpperArmSliderValue * upperArmTurnRate, 0 , 0);

        }

    }


    

