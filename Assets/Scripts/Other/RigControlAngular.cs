using System;
using System.Threading.Tasks;
using UnityEngine;

public class RigControlAngular : MonoBehaviour
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
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Jaw Variables
    [Header("Jaw")]
    public GameObject Jaw;
    Rigidbody Jaw_RigidBody;
    public Vector3 Jaw_TorqueForce;
    public ForceMode Jaw_ForceMode;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Extras
    [Header("Extras")]
    public Rigidbody ROV;
    public ReadArduino readArduino;
    public float massScale1 = 1;
    public float connectedAnchor1 = 1;
    float axis_to_angle_ratio = 240 / 1024f;
    public int ControllerValue;
    public int LastControllerValue;
    public int ControllerDisplacementValue;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Angular Displacements
    [Header("Angular Displacements")]
    public float Base_AngularDisplacement;
    public float Shoulder_AngularDisplacement;
    public float Forearm_AngularDisplacement;
    public float Yaw_AngularDisplacement;
    public float Pitch_AngularDisplacement;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Target Angles
    [Header("Target Angles")]
    public float Base_TargetAngle;
    public float Shoulder_TargetAngle;
    public float Forearm_TargetAngle;
    public float Yaw_TargetAngle;
    public float Pitch_TargetAngle;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    public void ResetAngularDisplacements()
    {
        Base_AngularDisplacement = 0f;
        Shoulder_AngularDisplacement = 0f;
        Forearm_AngularDisplacement = 0f;
        Yaw_AngularDisplacement = 0f;
        Pitch_AngularDisplacement = 0f;
    }
    public void FlipAngularDisplacements()
    {
        Base_AngularDisplacement *= -1;
        Shoulder_AngularDisplacement *= -1;
        Forearm_AngularDisplacement *= -1;
        Yaw_AngularDisplacement *= -1;
        Pitch_AngularDisplacement *= -1;
    }
    void FetchRigidBodies()
    {
        Base_RigidBody = Base.GetComponent<Rigidbody>();
        Shoulder_RigidBody = Shoulder.GetComponent<Rigidbody>();
        Forearm_RigidBody = ForeArm.GetComponent<Rigidbody>();
        Yaw_RigidBody = Yaw.GetComponent<Rigidbody>();
        Pitch_RigidBody = Pitch.GetComponent<Rigidbody>();
        Jaw_RigidBody = Jaw.GetComponent<Rigidbody>();
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
    void Start()
    {
        FetchRigidBodies();
    }
    void AddPitchLock()
    {
        if (Pitch.GetComponent<ConfigurableJoint>() != null)
            return;

        ConfigurableJoint joint = Pitch.AddComponent<ConfigurableJoint>();
        joint.connectedBody = ROV;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;

        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
    }
    float lastAngularDisplacement;
    void UniversalMovement(float AngularDisplacement, GameObject obj, Rigidbody target, Vector3 TorqueForce, ForceMode ForceMode)
    {
        if (AngularDisplacement >= -0.01 && AngularDisplacement <= 0.01)
        {
            Jointify(obj, target);
            return;
        }
        Destroy(obj.GetComponent<FixedJoint>());

        Vector3 Torque = TorqueForce * AngularDisplacement * Time.fixedDeltaTime;
        if (lastAngularDisplacement * AngularDisplacement < 0)
        {
            Torque *= 100 * obj.GetComponent<Rigidbody>().mass;
            Debug.Log("Empurradinho");
        }

        lastAngularDisplacement = AngularDisplacement;
        obj.GetComponent<Rigidbody>().AddRelativeTorque(Torque, ForceMode);
    }
    float last_angle_moved;
    bool canUpdateAngleVariable;
    public bool LockControler;
    public int zerosLimit;
    public int zeros_qtd;
    int GetControlerValue()
    {
        if (LockControler || ControllerValue >= -1 && ControllerValue <= 1)
            return 0;

        return ControllerValue;
    }
    void NewMovement(GameObject obj, Rigidbody target, float objAngle, float targetAngle, float largestAngleLimit, float smallestAngleLimit, Vector3 TorqueForce, ForceMode ForceMode)
    {
        //ControllerValueDisplacement = (LastControllerValue - ControllerValue);
        //ControllerDisplacementValue = GetControlerValue();
        //float angles_to_move = ControllerDisplacementValue * axis_to_angle_ratio;
        //float targetAngle = 0; 
        //if (obj.GetComponent<FixedJoint>() != null)
        //    targetAngle = obj.transform.localEulerAngles.z + angles_to_move;
        //else
        //    targetAngle += angles_to_move;

        GoToAngle(obj, target, objAngle, targetAngle, largestAngleLimit, smallestAngleLimit, TorqueForce, ForceMode);


        //if (ControllerValue == 0 && LastControllerValue == 0)
        //    zeros_qtd++;
        //else
        //    zeros_qtd = 0;


        //LastControllerValue = ControllerValue;
    }
    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
    void GoToAngle(GameObject obj, Rigidbody target, float objAngle, float targetAngle, float largestAngleLimit, float smallestAngleLimit, Vector3 TorqueForce, ForceMode ForceMode)
    {
        if (targetAngle > largestAngleLimit)
            targetAngle = largestAngleLimit;
        else if (targetAngle < smallestAngleLimit)
            targetAngle = smallestAngleLimit;

        objAngle = WrapAngle(objAngle);
        targetAngle = WrapAngle(targetAngle);

        int dir;
        if (objAngle > targetAngle)
            dir = -1;
        else
            dir = 1;
        Debug.Log("Obj angle: "+objAngle + Environment.NewLine + "Target angle: "+targetAngle);


        if (targetAngle - 0.5 <= objAngle && objAngle <= targetAngle + 0.5)
        {
            Jointify(obj, target);
            return;
        }
        Vector3 Torque = TorqueForce * Time.fixedDeltaTime * dir;

        Destroy(obj.GetComponent<FixedJoint>());
        obj.GetComponent<Rigidbody>().AddRelativeTorque(Torque, ForceMode);
    }
    void UpdatePitchAngularJoint()
    {
        if(Base.GetComponent<FixedJoint>()      != null &&
           Shoulder.GetComponent<FixedJoint>()  != null &&
           ForeArm.GetComponent<FixedJoint>()   != null &&
           Yaw.GetComponent<FixedJoint>()       != null &&
           Pitch.GetComponent<FixedJoint>()     != null)
        {
            AddPitchLock();
        }
        else
        {
            Destroy(Pitch.GetComponent<ConfigurableJoint>());
        }
    }
    void Update()
    {
        //if((Base_AngularDisplacement == Shoulder_AngularDisplacement) && (Shoulder_AngularDisplacement == Forearm_AngularDisplacement) && (Forearm_AngularDisplacement == Yaw_AngularDisplacement) && ( Yaw_AngularDisplacement == Pitch_AngularDisplacement) && (Pitch_AngularDisplacement >= -0.01 && Pitch_AngularDisplacement <= 0.01))
        //    AddPitchLock();
        //else
        //    Destroy(Pitch.GetComponent<ConfigurableJoint>());

        //ControllerValue = readArduino.recieved_data;
        UpdatePitchAngularJoint();
        NewMovement(Base,       ROV,                Base.transform.localEulerAngles.z,      Base_TargetAngle,           40, -90,  Base_TorqueForce,     Base_ForceMode);
        NewMovement(Shoulder,   Base_RigidBody,     Shoulder.transform.localEulerAngles.x,  Shoulder_TargetAngle,       83, -30,  Shoulder_TorqueForce, Shoulder_ForceMode);
        NewMovement(ForeArm,    Shoulder_RigidBody, ForeArm.transform.localEulerAngles.z - Shoulder.transform.localEulerAngles.x,   Forearm_TargetAngle,        -4, -150, ForeArm_TorqueForce,  ForeArm_ForceMode);
        

        //UniversalMovement(Shoulder_AngularDisplacement, Shoulder,   Base_RigidBody,     Shoulder_TorqueForce,   Shoulder_ForceMode);
        //UniversalMovement(Forearm_AngularDisplacement,  ForeArm,    Shoulder_RigidBody, ForeArm_TorqueForce,    ForeArm_ForceMode);
        //UniversalMovement(Yaw_AngularDisplacement,      Yaw,        Forearm_RigidBody,  Yaw_TorqueForce,        Yaw_ForceMode);
        //UniversalMovement(Pitch_AngularDisplacement,    Pitch,      Yaw_RigidBody,      Pitch_TorqueForce,      Pitch_ForceMode);
        LastControllerValue = ControllerValue;
    }
}