using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T4FORCE1 : MonoBehaviour
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
    public Rigidbody ROV;

    public ForceMode T4_ForceMode;

    //public KeyCode SetaUp;
    //public KeyCode SetaDown;
    //public KeyCode SetaRight;
    //public KeyCode SetaLeft;

    public KeyCode UpDown;

    //public KeyCode SpeedChanger;

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
        Vector3 TargetMovement = Vector3.zero;
        Vector3 ShoulderTargetMovement = Vector3.zero;

        if (Input.GetAxis("T4UD") >= 0.8 && !Input.GetKey(UpDown))
        {
            TargetMovement = new Vector3(0,0,m_Thrust);
        }
        else if (Input.GetAxis("T4UD") <= -0.8 && !Input.GetKey(UpDown))
        {
            TargetMovement = new Vector3(0, 0, -m_Thrust);
            ShoulderTargetMovement = new Vector3(0, m_Thrust, 0);
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
            Jointify(T4, ROV);
            Jointify(Shoulder, Base_rig);
            return;
        }

        Destroy(T4.GetComponent<FixedJoint>());
        Destroy(Yaw.GetComponent<FixedJoint>());
        Destroy(Shoulder.GetComponent<FixedJoint>());
        Destroy(Forearm.GetComponent<FixedJoint>());

        Vector3 Force = TargetMovement * Time.fixedDeltaTime;
        T4_rig.AddRelativeForce(Force, T4_ForceMode);

        if (ShoulderTargetMovement != Vector3.zero)
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