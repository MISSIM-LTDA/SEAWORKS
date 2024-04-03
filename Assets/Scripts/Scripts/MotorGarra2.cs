using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorGarra2 : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.U) || Input.GetKey(KeyCode.I))
        {

            if (Input.GetKey(KeyCode.U))
            {
                HingeJoint hinge = GetComponent<HingeJoint>();
                JointMotor motor = hinge.motor;
                motor.targetVelocity = -10;
                motor.freeSpin = false;
                hinge.motor = motor;
            }

            if (Input.GetKey(KeyCode.I))
            {

                HingeJoint hinge = GetComponent<HingeJoint>();
                JointMotor motor = hinge.motor;
                //motor.force = 100;
                motor.targetVelocity = 10;
                motor.freeSpin = false;
                hinge.motor = motor;

            }

        }
        else
        {
            HingeJoint hinge = GetComponent<HingeJoint>();
            JointMotor motor = hinge.motor;
            //motor.force = 100;
            motor.targetVelocity = 0;
            motor.freeSpin = false;
            hinge.motor = motor;
        }

    }
}