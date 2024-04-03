using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T4FORCE : MonoBehaviour
{

    public GameObject T4;
    public Rigidbody m_Rigidbody;
    public float m_Thrust = 20f;
    public Rigidbody ROV;
    public float force = 100;
    Vector3 m_Inpu2t;
    public float m_Speed;
    public float r_Speed;

    public KeyCode SetaUp;
    public KeyCode SetaDown;
    public KeyCode SetaRight;
    public KeyCode SetaLeft;

    public KeyCode SpeedChanger;


    void FixedUpdate()
    {

        if (Input.GetKey(SetaUp) && !Input.GetKey(SpeedChanger))
        {
            Destroy(T4.GetComponent<FixedJoint>());
           // m_Inpu2t = new Vector3(0, 0, m_Speed);
           // m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * -m_Speed);

            m_Rigidbody.AddForce(transform.forward * m_Thrust);
        }
        else
        {

            if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftShift))
            {
                Destroy(T4.GetComponent<FixedJoint>());
                // m_Inpu2t = new Vector3(0, 0, m_Speed);
                // m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                m_Rigidbody.AddForce(transform.forward * -1 * m_Thrust);
            }
            else
            {

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    Destroy(T4.GetComponent<FixedJoint>());
                    //m_Inpu2t = new Vector3(m_Speed, 0, 0);
                    // m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                    m_Rigidbody.AddForce(transform.right * -1 * m_Thrust);

                }
                else
                {

                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        Destroy(T4.GetComponent<FixedJoint>());
                        //m_Inpu2t = new Vector3(m_Speed, 0, 0);
                        //m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * -m_Speed);

                        m_Rigidbody.AddForce(transform.right * m_Thrust);


                    }
                    else
                    {

                        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift))
                        {
                            Destroy(T4.GetComponent<FixedJoint>());
                            //m_Inpu2t = new Vector3(0, m_Speed, 0);
                            //m_Rigidbody.MovePosition(transform.position + m_Inpu2t * Time.deltaTime * m_Speed);

                            m_Rigidbody.AddForce(transform.up * m_Thrust);
                        }
                        else
                        {

                            if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift))
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
