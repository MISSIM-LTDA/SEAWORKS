using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RigControlSeperate : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------
    #region Base Variables
    [Header("Base")]
    public GameObject Base;
    Rigidbody Base_RigidBody;

    public Vector3 Base_TorqueForce;
    public ForceMode Base_ForceMode;

    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Shoulder Variables
    [Header("Shoulder")]
    public GameObject Shoulder;
    Rigidbody Shoulder_RigidBody;

    public Vector3 Shoulder_TorqueForce;
    public ForceMode Shoulder_ForceMode;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Forearm Variables
    [Header("Forearm")]
    public GameObject ForeArm;
    Rigidbody Forearm_RigidBody;

    public Vector3 ForeArm_TorqueForce;
    public ForceMode ForeArm_ForceMode;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Yaw Variables
    [Header("Yaw")]
    public GameObject Yaw;
    Rigidbody Yaw_RigidBody;

    public Vector3 Yaw_TorqueForce;
    public ForceMode Yaw_ForceMode;
    bool yawing;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Pitch Variables;
    [Header("Pitch")]
    public GameObject Pitch;
    Rigidbody Pitch_RigidBody;

    public Vector3 Pitch_TorqueForce;
    public ForceMode Pitch_ForceMode;
    bool pitching;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Jaw Variables
    [Header("Jaw")]
    public GameObject Jaw;
    Rigidbody Jaw_RigidBody;
    public Vector3 Jaw_TorqueForce;
    public ForceMode Jaw_ForceMode;
    bool jawing;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Claw Variables
    [Header("Claw")]
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
    [Header("Controls")]
    public KeyCode Change_Mode;
    public KeyCode Open_Claw;
    public KeyCode Close_Claw;
    public KeyCode Jaw_Direita;
    public KeyCode Jaw_Esquerda;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Extras
    [Header("Extras")]
    public Rigidbody ROV;
    float massScale1 = 1;
    float connectedAnchor1 = 1;

    bool movingModules;
    int mode_index;

    public enum MovementMode
    {
        Base_Shoulder,
        Forearm,
        Yaw_Pitch
    }
    Array modes = Enum.GetValues(typeof(MovementMode));
    public MovementMode curMode;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        FetchRigidBodies();
    }
    void FetchRigidBodies()
    { 
        Base_RigidBody = Base.GetComponent<Rigidbody>();
        Shoulder_RigidBody = Shoulder.GetComponent<Rigidbody>();
        Forearm_RigidBody = ForeArm.GetComponent<Rigidbody>();
        Yaw_RigidBody = Yaw.GetComponent<Rigidbody>();
        Pitch_RigidBody = Pitch.GetComponent<Rigidbody>();
        Jaw_RigidBody = Jaw.GetComponent<Rigidbody>();
        Bar_Rigidbody = Bar.GetComponent<Rigidbody>();
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
    void CheckMovementMode()
    {
        if (Input.GetKeyDown(Change_Mode))
        {
            mode_index++;
            if (mode_index > 2 || mode_index < 0)
                mode_index = 0;
            curMode = (MovementMode)modes.GetValue(mode_index);
        }
    }
    void UniveralMovement(bool stopCondition, bool mov1Condition, bool mov2Condition, GameObject obj, Rigidbody target, Vector3 torqueForce, ForceMode forceMode)
    {
        if (stopCondition)
        {
            Jointify(obj, target);
            return;
        }

        int dir;
        if (mov1Condition)
            dir = -1;
        else if (mov2Condition)
            dir = 1;
        else
        {
            Jointify(obj, target);
            return;
        }
        Destroy(obj.GetComponent<FixedJoint>());
        movingModules = true;

        Vector3 Torque = torqueForce * Time.fixedDeltaTime * dir;
        obj.GetComponent<Rigidbody>().AddRelativeTorque(Torque, forceMode);
    }
    void BaseMovement()
    {
        UniveralMovement(curMode != MovementMode.Base_Shoulder, Input.GetAxis("T4S") >= 0.8, Input.GetAxis("T4S") <= -0.8, 
                         Base, ROV, Base_TorqueForce, Base_ForceMode);
    }
    void ShoulderMovement()
    {
        UniveralMovement(curMode != MovementMode.Base_Shoulder, Input.GetAxis("T4UD") >= 0.8, Input.GetAxis("T4UD") <= -0.8,
                         Shoulder, Base_RigidBody, Shoulder_TorqueForce, Shoulder_ForceMode);
    }
    void ForearmMovement()
    {
        UniveralMovement(curMode != MovementMode.Forearm, Input.GetAxis("T4UD") >= 0.8, Input.GetAxis("T4UD") <= -0.8, 
                         ForeArm, Shoulder_RigidBody, ForeArm_TorqueForce, ForeArm_ForceMode);
        if (pitching)
        {
            ConfigurableJoint joint = ForeArm.AddComponent<ConfigurableJoint>();
            joint.connectedBody = ROV;
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
        }
        else
        {
            Destroy(ForeArm.GetComponent<ConfigurableJoint>());
        }
    }
    void YawMovement()
    {
        //if (movingModules)
        //{
        //    Destroy(Yaw.GetComponent<ConfigurableJoint>());
        //}
        //else if(Yaw.GetComponent<ConfigurableJoint>() == null)
        //{
        //    ConfigurableJoint joint = Yaw.AddComponent<ConfigurableJoint>();
        //    joint.xMotion = ConfigurableJointMotion.Locked;
        //    joint.yMotion = ConfigurableJointMotion.Locked;
        //    joint.zMotion = ConfigurableJointMotion.Locked;

        //    joint.angularXMotion = ConfigurableJointMotion.Locked;
        //    joint.angularYMotion = ConfigurableJointMotion.Locked;
        //    joint.angularZMotion = ConfigurableJointMotion.Locked;
        //}

        UniveralMovement(curMode != MovementMode.Yaw_Pitch || pitching, Input.GetAxis("T4UD") >= 0.8, Input.GetAxis("T4UD") <= -0.8,
                         Yaw, Forearm_RigidBody, Yaw_TorqueForce, Yaw_ForceMode);
        
        if(Yaw.GetComponent<FixedJoint>() == null)
        {
            //Destroy(Yaw.GetComponent<ConfigurableJoint>());
            yawing = true;
        }
        else
            yawing = false;
    }
    void PitchMovement()
    {
        if (movingModules)
        {
            Destroy(Pitch.GetComponent<ConfigurableJoint>());
        }
        else if (Pitch.GetComponent<ConfigurableJoint>() == null)
        {
            ConfigurableJoint joint = Pitch.AddComponent<ConfigurableJoint>();
            joint.connectedBody = ROV;
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
        }
        UniveralMovement(curMode != MovementMode.Yaw_Pitch || yawing, Input.GetAxis("T4S") <= -0.8, Input.GetAxis("T4S") >= 0.8,
                         Pitch, Yaw_RigidBody, Pitch_TorqueForce, Pitch_ForceMode);
        
        if (Pitch.GetComponent<FixedJoint>() == null)
        {
            Destroy(Pitch.GetComponent<ConfigurableJoint>());
            pitching = true;
        }
        else
            pitching = false;
    }
    void JawMovement()
    {
        UniveralMovement(false, Input.GetKey(Jaw_Esquerda), Input.GetKey(Jaw_Direita), 
                         Jaw, Pitch_RigidBody, Jaw_TorqueForce, Jaw_ForceMode);

        if (Jaw.GetComponent<FixedJoint>() == null)
            jawing = true;
        else
            jawing = false;
    }
    void ClawMovement()
    {
        FixedJoint LeftJoint = LeftClawLock.GetComponent<FixedJoint>();
        FixedJoint RightJoint = RightClawLock.GetComponent<FixedJoint>();

        if (pitching || yawing || jawing || movingModules)
        {
            Jointify(Bar, Jaw_RigidBody);
            LeftJoint.connectedBody = LeftClawBottomCollider.GetComponent<Rigidbody>();
            RightJoint.connectedBody = RightClawBottomCollider.GetComponent<Rigidbody>();
            return;
        }

        int dir;
        if (Input.GetKey(Open_Claw))
        {
            dir = -1;
        }
        else if (Input.GetKey(Close_Claw))
        {
            dir = 1;
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
            if (RightClaw_Tip.transform.localPosition.y < ClawTip_closeMax && dir == 1)
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
    void Update()
    {
        CheckMovementMode();
        movingModules = false;

        BaseMovement();
        ShoulderMovement();
        ForearmMovement();
        YawMovement();
        PitchMovement();
        JawMovement();
        ClawMovement();

        Debug.Log(ForeArm.transform.localEulerAngles.z - Shoulder.transform.localEulerAngles.x);
    }
}