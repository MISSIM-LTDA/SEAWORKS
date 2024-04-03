using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T4FORCE1Other : MonoBehaviour
{

    public GameObject T4;
    private Rigidbody T4_rig;

    public GameObject ShoulderTarget;
    private Rigidbody ShoulderTarget_rig;

    public GameObject Base;
    private Rigidbody Base_rig;

    public GameObject Forearm;

    public GameObject Shoulder;

    public GameObject Yaw;

    public float m_Thrust = 2000f;
    public float m_Torque;
    
    public Rigidbody ROV;

    public ForceMode T4_ForceMode;
    public ForceMode Rotate_ForceMode;

    //public KeyCode SetaUp;
    //public KeyCode SetaDown;
    //public KeyCode SetaRight;
    //public KeyCode SetaLeft;

    public KeyCode UpDown;
    public KeyCode Rotate;

    //public KeyCode SpeedChanger;

    public bool Shoulder_movement;
    public bool Draw_Target;

    private void Start()
    {
        T4_rig = T4.GetComponent<Rigidbody>();
        ShoulderTarget_rig = ShoulderTarget.GetComponent<Rigidbody>();
        Base_rig = Base.GetComponent<Rigidbody>();
    }
    void Jointify(GameObject obj, Rigidbody connectedBody)
    {
        if (obj.GetComponent<FixedJoint>() != null)
            return;

        FixedJoint joint = obj.AddComponent<FixedJoint>();
        joint.connectedBody = connectedBody;
        joint.massScale = 1;
        joint.connectedMassScale = 1;
    }
    void ApplyMovements()
    {
        Vector3 TargetMovement;
        Vector3 ShoulderTargetMovement = Vector3.zero;

        if (Input.GetAxis("T4UD") >= 0.8 && !Input.GetKey(UpDown))
        {
            TargetMovement = new Vector3(0, 0, m_Thrust);
        }
        else if (Input.GetAxis("T4UD") <= -0.8 && !Input.GetKey(UpDown))
        {
            TargetMovement = new Vector3(0, 0, -m_Thrust);
            ShoulderTargetMovement = new Vector3(0, m_Thrust, 0) / 2;
        }
        else if (Input.GetAxis("T4S") >= 0.8 && Input.GetKey(Rotate))
        {
            T4.GetComponent<FixedJoint>().connectedBody = Base_rig;
            T4.GetComponent<ConfigurableJoint>().connectedBody = Base_rig;
            Vector3 Torque = new Vector3(0, 0, -m_Torque) * Time.fixedDeltaTime;
            // Destroy(Base.GetComponent<FixedJoint>());
            Base_rig.AddRelativeTorque(Torque, Rotate_ForceMode);
            return;
        }
        else if (Input.GetAxis("T4S") <= -0.8 && Input.GetKey(Rotate))
        {
            T4.GetComponent<FixedJoint>().connectedBody = Base_rig;
            T4.GetComponent<ConfigurableJoint>().connectedBody = Base_rig;
            Vector3 Torque = new Vector3(0, 0, m_Torque) * Time.fixedDeltaTime;
            //Destroy(Base.GetComponent<FixedJoint>());
            Base_rig.AddRelativeTorque(Torque, Rotate_ForceMode);
            return;
        }
        else if (Input.GetAxis("T4S") >= 0.8)
        {
            TargetMovement = new Vector3(-m_Thrust, 0, 0);
        }
        else if (Input.GetAxis("T4S") <= -0.8)
        {
            TargetMovement = new Vector3(m_Thrust, 0, 0);
        }
        else if (Input.GetAxis("T4UD") >= 0.8 && Input.GetKey(UpDown))
        {
            TargetMovement = new Vector3(0, m_Thrust, 0);
        }
        else if (Input.GetAxis("T4UD") <= -0.8 && Input.GetKey(UpDown))
        {
            TargetMovement = new Vector3(0, -m_Thrust, 0);
        }
        else
        {
            if (T4.GetComponent<FixedJoint>() == null)
            {
                Jointify(T4, ROV);
            }
            else
            {
                T4.GetComponent<FixedJoint>().connectedBody = ROV;
            }
            T4.GetComponent<ConfigurableJoint>().connectedBody = ROV;
            Jointify(Shoulder, Base_rig);
            return;
        }

        Destroy(T4.GetComponent<FixedJoint>());
        Destroy(Yaw.GetComponent<FixedJoint>());
        Destroy(Forearm.GetComponent<FixedJoint>());
        if (TargetMovement.y == 0)
            Destroy(Shoulder.GetComponent<FixedJoint>());

        Vector3 Force = TargetMovement * Time.fixedDeltaTime;
        T4_rig.AddRelativeForce(Force, T4_ForceMode);

        if (Shoulder_movement)
        {
            Force = ShoulderTargetMovement * Time.fixedDeltaTime;
            ShoulderTarget_rig.AddForce(Force, T4_ForceMode);
        }
    }
    void FixedUpdate()
    {
        ApplyMovements();
    }
    private void OnDrawGizmos()
    {
        if (!Draw_Target)
            return;
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawCube(T4.transform.position, Vector3.one);
    }
}