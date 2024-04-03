using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rigmaster : MonoBehaviour {


    public KeyCode Direita;
    public KeyCode Esquerda;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetKey(Direita))
        {
            var hinge = GetComponent<HingeJoint>();

            // Make the hinge motor rotate with 90 degrees per second and a strong force.
            var motor = hinge.motor;
            motor.force = 100;
            motor.targetVelocity = 10;
            motor.freeSpin = false;
            hinge.motor = motor;
            hinge.useMotor = true;
        }
        else
        {
            if (Input.GetKey(Esquerda))
            {
                var hinge = GetComponent<HingeJoint>();

                // Make the hinge motor rotate with 90 degrees per second and a strong force.
                var motor = hinge.motor;
                motor.force = 100;
                motor.targetVelocity = -10;
                motor.freeSpin = false;
                hinge.motor = motor;
                hinge.useMotor = true;
            }
            else
            {
                var hinge = GetComponent<HingeJoint>();

                // Make the hinge motor rotate with 90 degrees per second and a strong force.
                var motor = hinge.motor;
                motor.force = 0;
                motor.targetVelocity = 0;
                motor.freeSpin = false;
                hinge.motor = motor;
                hinge.useMotor = true;
            }
        }
       
       


        

    }

}
