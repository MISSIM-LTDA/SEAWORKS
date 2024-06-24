using System.Collections;
using UnityEngine;
public class Rigmaster : MonoBehaviour
{
    #region Shoulder Variables
    [Header("Shoulder")]
    [SerializeField] private GameObject shoulder;

    private Rigidbody shoulderRigidbody;
    private FixedJoint shoulderJoint;
    [SerializeField] private float shoulderRotationSpeed = 50.0f;

    [SerializeField] private ForceMode shoulderForceMode = ForceMode.Impulse;
    private Vector3 shoulderTorqueForce;

    private Vector3 shoulderEulerAngleVelocity;
    [Space(10)]
    #endregion

    #region Arm Variables
    [Header("Arm")]
    [SerializeField] private GameObject arm;

    private Rigidbody armRigidbody;
    private FixedJoint armJoint;
    [SerializeField] private float armRotationSpeed = 50.0f;

    [SerializeField] private ForceMode armForceMode = ForceMode.Impulse;
    private Vector3 armTorqueForce;

    private Vector3 armEulerAngleVelocity;
    [Space(10)]
    #endregion

    #region Boom Variables
    [Header("Boom")]
    [SerializeField] private GameObject boom;

    private Rigidbody boomRigidbody;
    private FixedJoint boomJoint;
    [SerializeField] private float boomMoveSpeed = 100.0f;

    [SerializeField] private ForceMode boomForceMode = ForceMode.Impulse;
    private Vector3 boomForce;

    private Vector3 boomVelocity;
    [Space(10)]
    #endregion

    #region Wrist Variables
    [Header("Wrist")]
    [SerializeField] private GameObject wrist;

    private Rigidbody wristRigidbody;
    private FixedJoint wristJoint;
    [SerializeField] private float wristRotationSpeed = 1.0f;

    [SerializeField] private ForceMode wristForceMode = ForceMode.Force;
    private Vector3 wristTorqueForce;

    private Vector3 wristEulerAngleVelocity;

    float timerLerp;
    float timerRot;

    int maxSeconds = 2;

    int lastDir;

    float torqueLimit;
    int initTorqueLimit = 10;
    int maxTorqueLimit = 100;
    [Space(10)]
    #endregion

    [SerializeField] private KeyCode rightRotation;
    [SerializeField] private KeyCode leftRotation;

    [SerializeField] private KeyCode upDown;

    private Rigidbody rovRigidbody;
    void Start()
    {
        shoulderRigidbody = shoulder.GetComponent<Rigidbody>();
        shoulderJoint = shoulder.GetComponent<FixedJoint>();
        shoulderEulerAngleVelocity = new Vector3(0, 0, shoulderRotationSpeed);

        armRigidbody = arm.GetComponent<Rigidbody>();
        armJoint = shoulder.GetComponent<FixedJoint>();
        armEulerAngleVelocity = new Vector3(0, 0, armRotationSpeed);

        boomRigidbody = boom.GetComponent<Rigidbody>();
        boomJoint = boom.GetComponent<FixedJoint>();
        boomVelocity = new Vector3(0, boomMoveSpeed, 0);

        wristRigidbody = wrist.GetComponent<Rigidbody>();
        wristJoint = wrist.GetComponent<FixedJoint>();
        wristEulerAngleVelocity = new Vector3(0, 0, wristRotationSpeed);

        rovRigidbody = transform.parent.parent.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        ShoulderRotation();
        ArmRotation();
        BoomMovement();
        WristRotation();
    }
    private void ShoulderRotation()
    {
        int shoulderCurrentDirection;

        if (Input.GetAxis("T4S") <= -0.8) {
            shoulderCurrentDirection = 1;
        }
        else if (Input.GetAxis("T4S") >= 0.8) {
            shoulderCurrentDirection = -1;
        }
        else{
            shoulderJoint = Jointify(shoulder, rovRigidbody);
            return;
        }

        Destroy(shoulderJoint);

        shoulderTorqueForce = shoulderEulerAngleVelocity * Time.fixedDeltaTime * shoulderCurrentDirection;
        shoulderRigidbody.AddRelativeTorque(shoulderTorqueForce, shoulderForceMode);
    }
    private void ArmRotation()
    {
        if (!Input.GetKey(upDown)) 
        {
            int armCurrentDirection;

            if (Input.GetAxis("T4UD") <= -0.8)
                armCurrentDirection = 1;
            else if (Input.GetAxis("T4UD") >= 0.8)
                armCurrentDirection = -1;
            else
            {
                armJoint = Jointify(arm, shoulderRigidbody);
                return;
            }

            Destroy(armJoint);

            armTorqueForce = armEulerAngleVelocity * Time.fixedDeltaTime * armCurrentDirection;
            armRigidbody.AddRelativeTorque(armTorqueForce, armForceMode);
        }
    }
    private void BoomMovement()
    {
        if (Input.GetKey(upDown)) 
        {
            int boomCurrentDirection;

            if (Input.GetAxis("T4UD") <= -0.8)
                boomCurrentDirection = 1;
            else if (Input.GetAxis("T4UD") >= 0.8)
                boomCurrentDirection = -1;
            else
            {
                boomJoint = Jointify(boom, armRigidbody);
                return;
            }

            Destroy(boomJoint);

            boomForce = boomVelocity * Time.fixedDeltaTime * boomCurrentDirection;
            boomRigidbody.AddRelativeForce(boomForce, boomForceMode);
        }
    }
    private void WristRotation()
    {
        int wristCurrentDirection;

        if (Input.GetKey(rightRotation))
            wristCurrentDirection = 1;
        else if (Input.GetKey(leftRotation))
            wristCurrentDirection = -1;

        else{
            timerLerp = 0;
            timerRot = 0;

            wristJoint = Jointify(wrist, boomRigidbody);
            return;
        }

        if (wristCurrentDirection != lastDir){
            timerLerp = 0;
            timerRot = 0;

            torqueLimit = initTorqueLimit;
        }
        timerRot += Time.fixedDeltaTime;

        if (timerRot >= maxSeconds && torqueLimit < maxTorqueLimit) {
            torqueLimit += 20 * Time.fixedDeltaTime;
        }

        Destroy(wristJoint);

        if (timerLerp < 1) {
            timerLerp += Time.fixedDeltaTime;
        }

        lastDir = wristCurrentDirection;

        wristTorqueForce = Mathf.Lerp(0, torqueLimit, timerLerp) * wristEulerAngleVelocity * wristCurrentDirection;
        wristRigidbody.AddTorque(wristTorqueForce, wristForceMode);
    }
    private FixedJoint Jointify(GameObject obj, Rigidbody connectedBody)
    {
        FixedJoint joint = obj.GetComponent<FixedJoint>();

        if (!obj.GetComponent<FixedJoint>()) {
            joint = obj.AddComponent<FixedJoint>();
            joint.connectedBody = connectedBody;
        } 

        return joint;
    }
}