using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigControl : MonoBehaviour
{
    public GameObject Yaw;
    public GameObject Pitch;
    public GameObject Jaw;
    public GameObject Target;

    public Rigidbody fixedConnectedBody;

    public float m_Speed;
    public float r_Speed;

    Vector3 m_EulerAngleVelocity;
    Vector3 m_EulerAngleVelocity2;

    Vector3 m_Input;
    Vector3 m_Inpu2t;

    public KeyCode Yaw_Direita;
    public KeyCode Yaw_Esquerda;
    public KeyCode Pitch_Direita;
    public KeyCode Pitch_Esquerda;
    public KeyCode Jaw_Direita;
    public KeyCode Jaw_Esquerda;

    public bool X;
    public bool Y;
    public bool Z;

    public float massScale1;
    public float connectedAnchor1;

    void Start()
    {

    }

    void FixedUpdate()
    {
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

        if (Input.GetKey(Yaw_Direita))
        {

        }

        else if (Input.GetKey(Yaw_Esquerda))
        {

        }
    }
}
