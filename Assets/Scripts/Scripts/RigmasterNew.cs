using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigmasterNew : MonoBehaviour
{

    public GameObject Arm1;

    public Rigidbody m_Rigidbody;
    public float m_Speed;
    public float r_Speed;


    public Rigidbody m_Rigidbody2;
    Vector3 m_EulerAngleVelocity;
    Vector3 m_EulerAngleVelocity2;

    Vector3 m_Input;
    Vector3 m_Inpu2t;

    public Rigidbody ROVrigb;

    public KeyCode Direita;
    public KeyCode Esquerda;

    public bool Ind;

    public bool X;
    public bool Y;
    public bool Z;

    public bool Linear;
    public bool Rotation;

    public float massScale1;
    public float connectedAnchor1;

    public bool freeMovement;

    void Start()
    {
        //Fetch the Rigidbody from the GameObject with this script attached
        m_Rigidbody = Arm1.GetComponent<Rigidbody>();


        //Fetch the Rigidbody from the GameObject with this script attached
        //m_Rigidbody2 = GetComponent<Rigidbody>();

        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)
        //m_EulerAngleVelocity = new Vector3(0, 100, 0);

  
    }

    void FixedUpdate()
    {


        //Store user input as a movement vector


        //Set the angular velocity of the Rigidbody (rotating around the Y axis, 100 deg/sec)



        // Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
        // m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);

        if (X)
        {
            m_Input = new Vector3(0, m_Speed, 0);
            m_Inpu2t = new Vector3(0, -m_Speed, 0);

            m_EulerAngleVelocity = new Vector3(0, r_Speed, 0);
            m_EulerAngleVelocity2 = new Vector3(0, -r_Speed, 0);
        }

        if (Y)
        {
            m_Input = new Vector3(m_Speed, 0, 0);
            m_Inpu2t = new Vector3(-m_Speed, 0, 0);

            m_EulerAngleVelocity = new Vector3(r_Speed, 0, 0);
            m_EulerAngleVelocity2 = new Vector3(-r_Speed, 0, 0);
        }

        if (Z)
        {
            m_Input = new Vector3(0, 0, m_Speed);
            m_Inpu2t = new Vector3(0, 0, -m_Speed);

            m_EulerAngleVelocity = new Vector3(0, 0, r_Speed);
            m_EulerAngleVelocity2 = new Vector3(0, 0, -r_Speed);
        }

        if (Linear)
        {
            if (Input.GetKey(Direita) && freeMovement == false)
            {
                Destroy(Arm1.GetComponent<FixedJoint>());
                m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
            }

            else if (Input.GetKey(Esquerda) && freeMovement == false)
            {
                Destroy(Arm1.GetComponent<FixedJoint>());
                m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);
            }

            else if (Input.GetKey(Direita) && freeMovement == true)
            {
                m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);
            }

            else if (Input.GetKey(Esquerda) && freeMovement == true)
            {
                m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);
            }

            else
            {
                if (Arm1.GetComponent<FixedJoint>() == null && freeMovement == false)
                {
                    var joint = Arm1.AddComponent<FixedJoint>();
                    joint.connectedBody = ROVrigb;
                }
            }
        }

        if (Rotation)
        {


            if (Input.GetKey(Direita) && freeMovement == false)
            {
                //m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);

                Destroy(Arm1.GetComponent<FixedJoint>());

                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
            }

            else if (Input.GetKey(Esquerda) && freeMovement == false)
            {
                //m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                Destroy(Arm1.GetComponent<FixedJoint>());

                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity2 * Time.fixedDeltaTime);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
            }

            else if (Input.GetKey(Direita) && freeMovement == true)
            {

                //m_Rigidbody.MovePosition(transform.position + m_Input * Time.deltaTime * m_Speed);

                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);

            }

            else if (Input.GetKey(Esquerda) && freeMovement == true)
            {
                //m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity2 * Time.fixedDeltaTime);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
            }

            else
            {
                if (Arm1.GetComponent<FixedJoint>() == null && freeMovement == false)
                {
                    var joint = Arm1.AddComponent<FixedJoint>();
                    joint.connectedBody = ROVrigb;
                    joint.massScale = massScale1;
                    joint.connectedMassScale = connectedAnchor1;
                }
            }
        }
    }
}



