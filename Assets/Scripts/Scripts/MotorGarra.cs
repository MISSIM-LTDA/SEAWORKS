using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorGarra : MonoBehaviour {

    public Rigidbody rb;
    public float CenterOfMass;   // Rotatation force relative to forward / back.

    private void Start()
    {
        
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, CenterOfMass);

    }

    // Update is called once per frame
    void Update () {
        
        if (Input.GetKey(KeyCode.T) || Input.GetKey(KeyCode.Y))
        {

            if (Input.GetKey(KeyCode.T))
            {
                HingeJoint hinge = GetComponent<HingeJoint>();
                JointMotor motor = hinge.motor;
                motor.targetVelocity = 10;
                motor.freeSpin = false;
                hinge.motor = motor;

                //JointLimits limits = hinge.limits;
               // limits.min = 1;
                //limits.max = 45;
               // hinge.limits = limits;
                //hinge.useLimits = true;
            }

            if (Input.GetKey(KeyCode.Y))
            {

                HingeJoint hinge = GetComponent<HingeJoint>();
                JointMotor motor = hinge.motor;
                //motor.force = 100;
                motor.targetVelocity = -10;
                motor.freeSpin = false;
                hinge.motor = motor;

                //JointLimits limits = hinge.limits;
                //limits.min = 1;
                //limits.max = 45;
                //hinge.limits = limits;
                //hinge.useLimits = true;

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

            //JointLimits limits = hinge.limits;
            //limits.min = 0;
            //limits.max = 0;
            //hinge.limits = limits;
            //hinge.useLimits = true;
        }   
        
    }
}
