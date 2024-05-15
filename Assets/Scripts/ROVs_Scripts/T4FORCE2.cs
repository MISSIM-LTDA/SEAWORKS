using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T4FORCE2 : MonoBehaviour
{

    public GameObject T4;
    public Rigidbody m_Rigidbody;
    public float m_Thrust = 20f;
    public Rigidbody ROV;
    public float force = 100;
    Vector3 m_Inpu2t;
    public float m_Speed;
    public float r_Speed;

    

    public KeyCode UpDown;

    public KeyCode SpeedChanger;


    void FixedUpdate()
    {

        if (Input.GetAxis("T4UD") >= 0.8 && !Input.GetKey(UpDown))
        {
            Destroy(T4.GetComponent<FixedJoint>());
           // m_Inpu2t = new Vector3(0, 0, m_Speed);
           // m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * -m_Speed);

            m_Rigidbody.AddForce(transform.forward * m_Thrust);
        }
        else
        {

            if (Input.GetAxis("T4UD") <= -0.8 && !Input.GetKey(UpDown))
            {
                Destroy(T4.GetComponent<FixedJoint>());
                // m_Inpu2t = new Vector3(0, 0, m_Speed);
                // m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                m_Rigidbody.AddForce(transform.forward * -1 * m_Thrust);
            }
            else
            {

                if (Input.GetAxis("T4S") >= 0.8)
                {
                    Destroy(T4.GetComponent<FixedJoint>());
                    //m_Inpu2t = new Vector3(m_Speed, 0, 0);
                    // m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                    m_Rigidbody.AddForce(transform.right * -1 * m_Thrust);

                }
                else
                {

                    if (Input.GetAxis("T4S") <= -0.8)
                    {
                        Destroy(T4.GetComponent<FixedJoint>());
                        //m_Inpu2t = new Vector3(m_Speed, 0, 0);
                        //m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * -m_Speed);

                        m_Rigidbody.AddForce(transform.right * m_Thrust);


                    }
                    else
                    {

                        if (Input.GetAxis("T4UD") >= 0.8 && Input.GetKey(UpDown))
                        {
                            Destroy(T4.GetComponent<FixedJoint>());
                            //m_Inpu2t = new Vector3(0, m_Speed, 0);
                            //m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                            m_Rigidbody.AddForce(transform.up * m_Thrust);
                        }
                        else
                        {

                            if (Input.GetAxis("T4UD") <= -0.8 && Input.GetKey(UpDown))
                            {
                                Destroy(T4.GetComponent<FixedJoint>());
                                //m_Inpu2t = new Vector3(0, m_Speed, 0);
                                //m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * -m_Speed);

                                m_Rigidbody.AddForce(transform.up * -1 * m_Thrust);
                            }
                            else
                            {
                                if (T4.GetComponent<FixedJoint>() == null)
                                {
                                    T4.AddComponent<FixedJoint>();

                                    T4.GetComponent<FixedJoint>().connectedBody = ROV;
                                }

                            }





                        }
                    }
                }
            }
        }
    }
}
