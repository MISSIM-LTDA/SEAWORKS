using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class RigmasterTotalOther : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------
    #region Shoulder Variables
    public GameObject Shoulder;

    private Rigidbody Shoulder_rig;
    public float Shoulder_Speed_rot = 100;

    [SerializeField] ForceMode Shoulder_ForceMode = ForceMode.Impulse;
    Vector3 Shoulder_TorqueForce;

    int Shoulder_CurDir;
    Vector3 m_EulerAngleVelocity_Shoulder;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Arm Variables
    public GameObject Arm;

    private Rigidbody Arm_rig;
    public float Arm_Speed_rot = 1000;

    [SerializeField] ForceMode Arm_ForceMode = ForceMode.Impulse;
    Vector3 Arm_TorqueForce;

    bool Arm_wasMoving;
    bool Arm_isMoving;

    int Arm_CurDir;
    Vector3 m_EulerAngleVelocity_Arm;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Boom Variables
    public GameObject Boom;

    private Rigidbody Boom_rig;
    public float Boom_Speed_mov = 100;

    [SerializeField] ForceMode Boom_ForceMode = ForceMode.Impulse;
    Vector3 Boom_Force;

    bool Boom_isExtending;
    int Boom_CurDir;
    Vector3 m_Input_Boom;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Wrist Variables
    public GameObject Wrist;

    private Rigidbody Wrist_rig;

    [SerializeField] ForceMode Wrist_ForceMode = ForceMode.Force;
    public float Wrist_Speed_rot = 1;

    Vector3 m_EulerAngleVelocity_Wrist;
    Vector3 Wrist_TorqueForce;

    public KeyCode Direita_rot;
    public KeyCode Esquerda_rot;

    float Timer_lerp = 0;
    float Timer_rot;
    int MaxSeconds = 2;

    bool Wrist_isRotating;
    int LastDir;
    int CurDir;

    float TorqueLimit;
    int InitTorqueLimit = 10;
    int MaxTorqueLimit = 100;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Claw Variables
    public GameObject Bar;
    public GameObject LeftClawLock;
    public GameObject LeftClawBottomCollider;
    public GameObject RightClawLock;
    public GameObject RightClawBottomCollider;

    private Rigidbody Bar_rig;

    public KeyCode OpenClaw;
    public KeyCode CloseClaw;

    public float Bar_Speed_Sideways = 100;
    public float Bar_Speed_UpDown = 350;
    public ForceMode Bar_ForceMode = ForceMode.Force;
    Vector3 Bar_Force;

    bool Claw_canOpen = true;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Extras
    public Rigidbody ROVrigb;

    public float Joint_massScale1 = 1;
    public float JointconnectedAnchor1 = 1;

    public bool freeMovement;

    public KeyCode UpDown;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    void FetchRigidBodies()
    {
        Shoulder_rig = Shoulder.GetComponent<Rigidbody>();
        Arm_rig = Arm.GetComponent<Rigidbody>();
        Boom_rig = Boom.GetComponent<Rigidbody>();
        Wrist_rig = Wrist.GetComponent<Rigidbody>();
        Bar_rig = Bar.GetComponent<Rigidbody>();
    }
    void Set_Values()
    {
        m_EulerAngleVelocity_Shoulder = new Vector3(0, 0, Shoulder_Speed_rot);
        m_EulerAngleVelocity_Arm = new Vector3(0, 0, Arm_Speed_rot);
        m_Input_Boom = new Vector3(0, Boom_Speed_mov, 0);
        m_EulerAngleVelocity_Wrist = new Vector3(0, 0, Wrist_Speed_rot);
        Claw_canOpen = true;
    }
    void Start()
    {
        FetchRigidBodies();
        Set_Values();
    }
    void ShoulderMovement()
    {
        if (Input.GetAxis("T4S") <= -0.8 && freeMovement == false)
            Shoulder_CurDir = 1;
        else if (Input.GetAxis("T4S") >= 0.8 && freeMovement == false)
            Shoulder_CurDir = -1;
        else
        {
            Jointify(Shoulder, ROVrigb);
            return;
        }
        Destroy(Shoulder.GetComponent<FixedJoint>());
        Shoulder_TorqueForce = m_EulerAngleVelocity_Shoulder * Time.fixedDeltaTime * Shoulder_CurDir;
        Shoulder_rig.AddRelativeTorque(Shoulder_TorqueForce, Shoulder_ForceMode);
    }
    IEnumerator UnBlockClaw()
    {
        yield return new WaitForSeconds(1.5f);

        Claw_canOpen = true;
        StopCoroutine("UnBlockClaw");
    }
    void ArmMovement()
    {
        if(Arm_wasMoving && !Arm_isMoving)
        {
            Claw_canOpen = false;
            StartCoroutine("UnBlockClaw");
        }

        Arm_wasMoving = Arm_isMoving;


        if (Input.GetAxis("T4UD") <= -0.8 && freeMovement == false && !Input.GetKey(UpDown))
            Arm_CurDir = 1;
        else if (Input.GetAxis("T4UD") >= 0.8 && freeMovement == false && !Input.GetKey(UpDown))
            Arm_CurDir = -1;
        else
        {
            Arm_isMoving = false;
            
            Jointify(Arm, Shoulder_rig);
            return;
        }

        Arm_isMoving = true;
        Destroy(Arm.GetComponent<FixedJoint>());
        Arm_TorqueForce = m_EulerAngleVelocity_Arm * Time.fixedDeltaTime * Arm_CurDir;
        Arm_rig.AddRelativeTorque(Arm_TorqueForce, Arm_ForceMode);
    }
    void BoomMovement()
    {
        if (Input.GetAxis("T4UD") <= -0.8 && freeMovement == false && Input.GetKey(UpDown))
            Boom_CurDir = 1;
        else if (Input.GetAxis("T4UD") >= 0.8 && freeMovement == false && Input.GetKey(UpDown))
            Boom_CurDir = -1;
        else
        {
            Boom_isExtending = false;
            Jointify(Boom, Arm_rig);
            return;
        }
        Boom_isExtending = true;
        Destroy(Boom.GetComponent<FixedJoint>());
        Boom_Force = m_Input_Boom * Time.fixedDeltaTime * Boom_CurDir;
        Boom_rig.AddRelativeForce(Boom_Force, Boom_ForceMode);
    }
    void WristRotation()
    {
        Wrist_isRotating = false;
        if (freeMovement == true)
            return;

        if (Input.GetKey(Direita_rot))
            CurDir = 1;
        else if (Input.GetKey(Esquerda_rot))
            CurDir = -1;
        else
        {
            Timer_lerp = 0;
            Timer_rot = 0;
            Jointify(Wrist, Boom_rig);
            return;
        }

        if (LastDir != CurDir)
        {
            Timer_rot = 0;
            TorqueLimit = InitTorqueLimit;
        }
        Timer_rot += Time.fixedDeltaTime;

        if (Timer_rot >= MaxSeconds && TorqueLimit < MaxTorqueLimit)
            TorqueLimit += 20 * Time.fixedDeltaTime;

        Destroy(Wrist.GetComponent<FixedJoint>());
        RotateWrist(TorqueLimit);


    }
    
    void RotateWrist(float MaxTorque)
    {
        
        Wrist_isRotating = true;
        if (Timer_lerp < 1)
            Timer_lerp += Time.fixedDeltaTime;

        if (CurDir != LastDir)
            Timer_lerp = 0;

        LastDir = CurDir;

        Wrist_TorqueForce = Mathf.Lerp(0, MaxTorque, Timer_lerp) * m_EulerAngleVelocity_Wrist * CurDir;
        Wrist_rig.AddTorque(Wrist_TorqueForce, Wrist_ForceMode);
    }
    Vector3 GetBarForce(float angle)
    {
        float force = Bar_Speed_Sideways;
        if ((angle <= 110 && angle >= 65) || ((angle <= 295 && angle >= 235)))
            force = Bar_Speed_UpDown;

        return new Vector3(0, force,0);
    }
    void ClawMovement()
    {
        FixedJoint LeftJoint = LeftClawLock.GetComponent<FixedJoint>();
        FixedJoint RightJoint = RightClawLock.GetComponent<FixedJoint>();
        int dir;

        if (Wrist_isRotating || Boom_isExtending || Arm_isMoving || !Claw_canOpen)
        {
            Jointify(Bar, Wrist_rig);
            LeftJoint.connectedBody = LeftClawBottomCollider.GetComponent<Rigidbody>();
            RightJoint.connectedBody = RightClawBottomCollider.GetComponent<Rigidbody>();
            return;
        }

        if (Input.GetKey(OpenClaw))
        {
            dir = -1;
        }
        else if (Input.GetKey(CloseClaw))
        {
            dir = 1;
        }
        else
        {
            Jointify(Bar, Wrist_rig);
            LeftJoint.connectedBody = LeftClawBottomCollider.GetComponent<Rigidbody>();
            RightJoint.connectedBody = RightClawBottomCollider.GetComponent<Rigidbody>();
            return;
        }

        Destroy(Bar.GetComponent<FixedJoint>());

        LeftJoint.connectedBody = Wrist_rig;
        RightJoint.connectedBody = Wrist_rig;
        
        Bar_Force = GetBarForce(Wrist.transform.localEulerAngles.z) * Time.deltaTime * dir;
        Bar_rig.AddRelativeForce(Bar_Force, Bar_ForceMode);
    }
    void Jointify(GameObject obj, Rigidbody connectedBody)
    {
        if (obj.GetComponent<FixedJoint>() != null || freeMovement == true)
            return;

        FixedJoint joint = obj.AddComponent<FixedJoint>();
        joint.connectedBody = connectedBody;
        joint.massScale = Joint_massScale1;
        joint.connectedMassScale = JointconnectedAnchor1;
    }
    void FixedUpdate()
    {
        Set_Values();
        ShoulderMovement();
        ArmMovement();
        BoomMovement();
        WristRotation();
        ClawMovement();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
    }
}