using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RigControlNewOther : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------
    #region Yaw Variables
    public GameObject Yaw;
    Rigidbody Yaw_RigidBody;

    public Vector3 Yaw_TorqueForce;
    public ForceMode Yaw_ForceMode;
    bool yawing;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Pitch Variables;
    public GameObject Pitch;
    Rigidbody Pitch_RigidBody;

    public Vector3 Pitch_TorqueForce;
    public ForceMode Pitch_ForceMode;
    bool pitching;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Jaw Variables
    public GameObject Jaw;
    Rigidbody Jaw_RigidBody;
    public Vector3 Jaw_TorqueForce;
    public ForceMode Jaw_ForceMode;
    bool jawing;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Forearm Variables
    public GameObject ForeArm;
    Rigidbody Forearm_RigidBody;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Target
    public GameObject Target;
    Rigidbody Target_RigidBody;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Claw Variables
    public GameObject RightClaw_Tip;
    public GameObject RightClaw_Tip_Target_Reference;
    public GameObject LeftClaw_Tip;
    public GameObject LeftClaw_Tip_Target_Reference;
    public GameObject Bar;
    public GameObject LeftClawLock;
    public GameObject LeftClawBottomCollider;
    public GameObject RightClawLock;
    public GameObject RightClawBottomCollider;
    public ClawCollisionDetection ClawCollisionDetection;
    Rigidbody Bar_Rigidbody;
    
    public Vector3 Claw_Tip_movSpeed;
    public float ClawTip_openMax = -0.469f;
    public float ClawTip_closeMax = 0.43f;
    bool opened;

    public Vector3 Bar_Force;
    public ForceMode Bar_ForceMode;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region KeyCodes
    public KeyCode Yaw_Direita;
    public KeyCode Yaw_Esquerda;
    public KeyCode Pitch_Direita;
    public KeyCode Pitch_Esquerda;
    public KeyCode Jaw_Direita;
    public KeyCode Jaw_Esquerda;
    public KeyCode Open_Claw;
    public KeyCode Close_Claw;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Extras
    public float massScale1;
    public float connectedAnchor1;

    public bool movingModules;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        FetchRigidBodies();
    }
    void FetchRigidBodies()
    {
        Yaw_RigidBody = Yaw.GetComponent<Rigidbody>();
        Pitch_RigidBody = Pitch.GetComponent<Rigidbody>();
        Jaw_RigidBody = Jaw.GetComponent<Rigidbody>();
        Target_RigidBody = Target.GetComponent<Rigidbody>();
        Bar_Rigidbody = Bar.GetComponent<Rigidbody>();
        Forearm_RigidBody = ForeArm.GetComponent<Rigidbody>();
        //RightClaw_Tip_RigidBody = RightClaw_Tip.GetComponent<Rigidbody>();
        //LeftClaw_Tip_RigidBody = LeftClaw_Tip.GetComponent<Rigidbody>();
    }
    void PrintAllJoints()
    {
        string yaw = "Yaw: " + (Yaw.GetComponent<FixedJoint>() != null) + "\n";
        string pitch = "Pitch: " + (Pitch.GetComponent<FixedJoint>() != null) + "\n";
        string jaw = "Jaw: " + (Jaw.GetComponent<FixedJoint>() != null) + "\n";
        string forearm = "Forearm: " + (ForeArm.GetComponent<FixedJoint>() != null) + "\n";

        Debug.Log(yaw + pitch + jaw + forearm);
    }
    void Jointify(GameObject obj, Rigidbody connectedBody)
    {
        if (obj.GetComponent<FixedJoint>() != null)
            return;

        FixedJoint joint = obj.AddComponent<FixedJoint>();
        joint.connectedBody = connectedBody;
        joint.massScale = massScale1;
        joint.connectedMassScale = connectedAnchor1;
    }
    void YawMovement()
    {
        if (Target.GetComponent<FixedJoint>() == null)
        {
            yawing = false;
            Jointify(Yaw, Target_RigidBody);
            Jointify(ForeArm, Target_RigidBody);
            return;
        }

        int dir;
        if (Input.GetKey(Yaw_Direita) && !pitching)
            dir = 1;
        else if (Input.GetKey(Yaw_Esquerda) && !pitching)
            dir = -1;
        else
        {
            yawing = false;
            Jointify(Yaw, Target_RigidBody);
            Jointify(ForeArm, Target_RigidBody);
            return;
        }


        movingModules = true;
        yawing = true;

        //Destroy(ForeArm.GetComponent<FixedJoint>());
        Destroy(Yaw.GetComponent<FixedJoint>());
        Pitch.GetComponent<FixedJoint>().connectedBody = Yaw_RigidBody;
        ForeArm.GetComponent<FixedJoint>().connectedBody = Target_RigidBody;

        Vector3 Torque = Yaw_TorqueForce * Time.fixedDeltaTime * dir;
        Yaw_RigidBody.AddRelativeTorque(Torque, Yaw_ForceMode);
    }
    void PitchMovement()
    {
        if (Target.GetComponent<FixedJoint>() == null)
        {
            pitching = false;
            if (Pitch.GetComponent<FixedJoint>() == null)
                Jointify(Pitch, Target_RigidBody);
            else if (!yawing)
                Pitch.GetComponent<FixedJoint>().connectedBody = Target_RigidBody;

            return;
        }

        int dir;
        if (Input.GetKey(Pitch_Direita) && !yawing)
        {
            dir = 1;            
        }
        else if (Input.GetKey(Pitch_Esquerda) && !yawing)
        {
            dir = -1;
        }
        else
        {
            pitching = false;
            if (Pitch.GetComponent<FixedJoint>() == null)
                Jointify(Pitch, Target_RigidBody);
            else if (!yawing)
                Pitch.GetComponent<FixedJoint>().connectedBody = Target_RigidBody;
            return;
        }

        Destroy(Pitch.GetComponent<FixedJoint>());

        movingModules = true;
        pitching = true;

        Vector3 Torque = Time.fixedDeltaTime * Pitch_TorqueForce * dir;
        Pitch_RigidBody.AddRelativeTorque(Torque, Pitch_ForceMode);

    }
    void JawMovement()
    {
        int dir;
        if (Input.GetKey(Jaw_Direita))
            dir = 1;
        else if (Input.GetKey(Jaw_Esquerda))
            dir = -1;
        else
        {
            jawing = false;
            Jointify(Jaw, Pitch_RigidBody);
            return;
        }

        Destroy(Jaw.GetComponent<FixedJoint>());
        movingModules = true;
        jawing = true;

        Vector3 Torque = Jaw_TorqueForce * Time.fixedDeltaTime * dir;
        Jaw_RigidBody.AddRelativeTorque(Torque, Jaw_ForceMode);
    }
    void ClawMovement()
    {
        FixedJoint LeftJoint = LeftClawLock.GetComponent<FixedJoint>();
        FixedJoint RightJoint = RightClawLock.GetComponent<FixedJoint>();

        if (pitching || yawing || jawing || Target.GetComponent<FixedJoint>() == null)
        {
            Jointify(Bar, Jaw_RigidBody);
            LeftJoint.connectedBody = LeftClawBottomCollider.GetComponent<Rigidbody>();
            RightJoint.connectedBody = RightClawBottomCollider.GetComponent<Rigidbody>();
            return;
        }

        int dir;
        if (Input.GetKey(Open_Claw))
        {
            dir = 1;
        }
        else if (Input.GetKey(Close_Claw))
        {
            dir = -1;
        }
        else
        {
            Jointify(Bar, Jaw_RigidBody);
            LeftJoint.connectedBody = LeftClawBottomCollider.GetComponent<Rigidbody>();
            RightJoint.connectedBody = RightClawBottomCollider.GetComponent<Rigidbody>();
            return;
        }

        opened = ClawCollisionDetection.colided;
        if (!opened)
        {
            RightClaw_Tip.transform.LookAt(RightClaw_Tip_Target_Reference.transform, Jaw.transform.up);
            LeftClaw_Tip.transform.LookAt(LeftClaw_Tip_Target_Reference.transform, Jaw.transform.up);
        }

        movingModules = true;

        if (opened)
        {

            Jointify(Bar, Jaw_RigidBody);
            LeftJoint.connectedBody = LeftClawBottomCollider.GetComponent<Rigidbody>();
            RightJoint.connectedBody = RightClawBottomCollider.GetComponent<Rigidbody>();

            if (RightClaw_Tip.transform.localPosition.y > ClawTip_openMax && dir == -1)
            {
                RightClaw_Tip.transform.localPosition -= Claw_Tip_movSpeed;
                LeftClaw_Tip.transform.localPosition += Claw_Tip_movSpeed;
                return;
            }
            
            if(RightClaw_Tip.transform.localPosition.y < ClawTip_closeMax && dir == 1)
            {
                RightClaw_Tip.transform.localPosition += Claw_Tip_movSpeed;
                LeftClaw_Tip.transform.localPosition -= Claw_Tip_movSpeed;
                return;
            }
            if (dir == -1)
            {
                Debug.Log("TA tentando abrir mais doq da");
                return;
            }
                
        }

        Destroy(Bar.GetComponent<FixedJoint>());
        LeftJoint.connectedBody = Jaw_RigidBody;
        RightJoint.connectedBody = Jaw_RigidBody;

        Vector3 Force = Bar_Force * Time.fixedDeltaTime * dir;
        Bar_Rigidbody.AddRelativeForce(Force, Bar_ForceMode);
    }
    void FixedUpdate()
    {
        movingModules = false;

        YawMovement();
        PitchMovement();
        JawMovement();
        ClawMovement();
    }
}