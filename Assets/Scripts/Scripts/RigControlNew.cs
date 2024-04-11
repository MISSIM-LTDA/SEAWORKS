using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigControlNew : MonoBehaviour
{
    public GameObject Yaw;
    public GameObject Pitch;
    public GameObject Jaw;
    public GameObject ForeArm;
    public GameObject Target;

    Rigidbody Yaw_RigidBody;
    Rigidbody Pitch_RigidBody;
    Rigidbody Jaw_RigidBody;
    Rigidbody Target_RigidBody;

    public float torque_Speed;
    public float torque_Speed_Jaw;

    public KeyCode Yaw_Direita;
    public KeyCode Yaw_Esquerda;
    public KeyCode Pitch_Direita;
    public KeyCode Pitch_Esquerda;
    public KeyCode Jaw_Direita;
    public KeyCode Jaw_Esquerda;

    public float massScale1;
    public float connectedAnchor1;

    public bool aux;
    void Start()
    {
        Yaw_RigidBody = Yaw.GetComponent<Rigidbody>();
        Pitch_RigidBody = Pitch.GetComponent<Rigidbody>();
        Jaw_RigidBody = Jaw.GetComponent<Rigidbody>();
        Target_RigidBody = Target.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(Yaw_Direita))
        {
            Destroy(Pitch.GetComponent<FixedJoint>());

            if(aux == true) 
            {
                var joint = ForeArm.AddComponent<FixedJoint>();
                joint.connectedBody = Target_RigidBody;
                joint.massScale = massScale1;
                joint.connectedMassScale = connectedAnchor1;
                aux = false;
            }


            Yaw_RigidBody.AddTorque(transform.right * torque_Speed);
        }

        else if (Input.GetKey(Yaw_Esquerda))
        {
            Destroy(Pitch.GetComponent<FixedJoint>());

            if (aux == true)
            {
                var joint = ForeArm.AddComponent<FixedJoint>();
                joint.connectedBody = Target_RigidBody;
                joint.massScale = massScale1;
                joint.connectedMassScale = connectedAnchor1;
                aux = false;
            }

            Yaw_RigidBody.AddTorque(transform.right * -1 * torque_Speed);
        }

        else if (Input.GetKey(Pitch_Direita))
        {
            Destroy(Pitch.GetComponent<FixedJoint>());

            if (aux == true)
            {
                var joint = Yaw.AddComponent<FixedJoint>();
                joint.connectedBody = Target_RigidBody;
                joint.massScale = massScale1;
                joint.connectedMassScale = connectedAnchor1;
                aux = false;
            }

            Pitch_RigidBody.AddTorque(transform.up * torque_Speed);
        }

        else if (Input.GetKey(Pitch_Esquerda))
        {
            Destroy(Pitch.GetComponent<FixedJoint>());

            if (aux == true)
            {
                var joint = Yaw.AddComponent<FixedJoint>();
                joint.connectedBody = Target_RigidBody;
                joint.massScale = massScale1;
                joint.connectedMassScale = connectedAnchor1;
                aux = false;
            }

            Pitch_RigidBody.AddTorque(transform.up * -1 * torque_Speed);
        }

        else if (Input.GetKey(Jaw_Direita))
        {
            Destroy(Jaw.GetComponent<FixedJoint>());

            Jaw_RigidBody.AddTorque(transform.forward * torque_Speed_Jaw);
        }

        else if (Input.GetKey(Jaw_Esquerda))
        {
            Destroy(Jaw.GetComponent<FixedJoint>());

            Jaw_RigidBody.AddTorque( -transform.forward * torque_Speed_Jaw);
        }

        else
        {
            
            if (Pitch.GetComponent<FixedJoint>() == null)
            {
                Destroy(ForeArm.GetComponent<FixedJoint>());
                aux = true;

                if(Yaw.GetComponent<FixedJoint>() != null)
                    Destroy(Yaw.GetComponent<FixedJoint>());

                if (ForeArm.GetComponent<FixedJoint>() != null)
                    Destroy(ForeArm.GetComponent<FixedJoint>());

                var joint = Pitch.AddComponent<FixedJoint>();
                joint.connectedBody = Target_RigidBody;
                joint.massScale = massScale1;
                joint.connectedMassScale = connectedAnchor1;
                Yaw_RigidBody.AddTorque(transform.right * 0);
                Pitch_RigidBody.AddTorque(transform.right * 0);
            }

            if (Jaw.GetComponent<FixedJoint>() == null)
            {
                var joint = Jaw.AddComponent<FixedJoint>();
                joint.connectedBody = Pitch_RigidBody;
                joint.massScale = massScale1;
                joint.connectedMassScale = connectedAnchor1;
                Jaw_RigidBody.AddTorque(transform.forward * 0);
            }
        }

        if (Input.GetKeyUp(Jaw_Direita))
        {
            Jaw_RigidBody.AddTorque(transform.forward * 0);
            Jaw_RigidBody.angularVelocity = Vector3.zero;
        }
        if (Input.GetKeyUp(Jaw_Esquerda))
        {
            Jaw_RigidBody.AddTorque(transform.forward * 0);
            Jaw_RigidBody.angularVelocity = Vector3.zero;
        }
    }
}
