using UnityEngine;

public class Titan4 : MonoBehaviour
{
    #region Azimuth Variables
    [Header("Azimuth")]
    [SerializeField] private GameObject azimuthRef;
    private Rigidbody azimuthRigidbody;
    private FixedJoint azimuthJoint;

    [SerializeField] private float azimuthSpeed = 20.0f;

    public KeyCode upDown;
    [Space(10)]
    #endregion

    #region Yaw Variables
    [Header("Yaw")]
    [SerializeField] private GameObject yaw;
    private Rigidbody yawRigidbody;
    private FixedJoint yawJoint;

    [SerializeField] private float yawSpeed = 30.0f;

    public KeyCode rightYaw;
    public KeyCode leftYaw;
    [Space(10)]
    #endregion

    #region ForeArm Variables
    [Header("ForeArm")]
    [SerializeField] private GameObject foreArm;
    private FixedJoint foreArmJoint;

    [Space(10)]
    #endregion

    #region Pitch Variables
    [Header("Pitch")]
    [SerializeField] private GameObject pitch;
    private Rigidbody pitchRigidbody;
    private FixedJoint pitchJoint;

    [SerializeField] private float pitchSpeed = 30.0f;

    public KeyCode rightPitch;
    public KeyCode leftPitch;
    [Space(10)]
    #endregion

    #region Jaw Variables
    [Header("Jaw")]
    [SerializeField] private GameObject jaw;
    private Rigidbody jawRigidbody;
    private FixedJoint jawJoint;

    [SerializeField] private float jawSpeed = 30.0f;

    public KeyCode rightJaw;
    public KeyCode leftJaw;
    [Space(10)]
    #endregion

    private Rigidbody rovRigidbody;
    private void Start()
    {
        azimuthRigidbody = azimuthRef.GetComponent<Rigidbody>();
        azimuthJoint = azimuthRef.GetComponent<FixedJoint>();

        yawRigidbody = yaw.GetComponent<Rigidbody>();
        yawJoint = yaw.GetComponent<FixedJoint>();

        foreArmJoint = foreArm.GetComponent<FixedJoint>();

        pitchRigidbody = pitch.GetComponent<Rigidbody>();
        pitchJoint = pitch.GetComponent<FixedJoint>();

        jawRigidbody = jaw.GetComponent<Rigidbody>();
        jawJoint = jaw.GetComponent<FixedJoint>();

        rovRigidbody = GameObject.FindGameObjectWithTag("XLX").GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        AzimuthRotation();

        if (Input.GetKey(rightYaw)) {
            GenericActuatorMovement(pitchJoint,
            foreArm,ref foreArmJoint,azimuthRigidbody,
            yawRigidbody,transform.right,yawSpeed); 
        }

        else if (Input.GetKey(leftYaw)){
            GenericActuatorMovement(pitchJoint,
            foreArm, ref foreArmJoint, azimuthRigidbody,
            yawRigidbody, -transform.right, yawSpeed);
        }

        else if (Input.GetKey(rightPitch)){
            GenericActuatorMovement(pitchJoint,
            yaw, ref yawJoint, azimuthRigidbody,
            pitchRigidbody, transform.up, pitchSpeed);
        }

        else if (Input.GetKey(leftPitch)){
           GenericActuatorMovement(pitchJoint,
           yaw, ref yawJoint, azimuthRigidbody,
           pitchRigidbody, -transform.up, pitchSpeed);
        }

        else if (Input.GetKey(rightJaw)){
            GenericActuatorMovement(jawJoint,
            jawRigidbody, transform.forward, jawSpeed);
        }

        else if (Input.GetKey(leftJaw)){
            GenericActuatorMovement(jawJoint,
            jawRigidbody, -transform.forward, jawSpeed);
        }

        else 
        {
            Destroy(yawJoint);
            yawRigidbody.AddTorque(Vector3.zero);

            Destroy(foreArmJoint);

            pitchJoint = Jointify(pitch, azimuthRigidbody);
            pitchRigidbody.AddTorque(Vector3.zero);

            jawJoint = Jointify(jaw, pitchRigidbody);
            jawRigidbody.AddTorque(Vector3.zero);
            jawRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void AzimuthRotation() 
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetAxis("T4UD") >= 0.8 && !Input.GetKey(upDown)){
            direction = transform.forward;
        }

        else if (Input.GetAxis("T4UD") >= 0.8 && Input.GetKey(upDown)){
            direction = transform.up;
        }

        else if (Input.GetAxis("T4UD") <= -0.8 && !Input.GetKey(upDown)){
            direction = transform.forward * (-1);
        }

        else if (Input.GetAxis("T4UD") <= -0.8 && Input.GetKey(upDown)){
            direction = transform.up * (-1);
        }

        else if (Input.GetAxis("T4S") <= -0.8){
            direction = transform.right;
        }

        else if (Input.GetAxis("T4S") >= 0.8){
            direction = transform.right * (-1);
        }

        else {
            if (!azimuthJoint) {
                azimuthJoint = Jointify(azimuthRef, rovRigidbody);
                return;
            }
        }

        Destroy(azimuthJoint);
        azimuthRigidbody.AddForce(direction * azimuthSpeed);
    }
    private void GenericActuatorMovement(FixedJoint destroyedJoint,
      Rigidbody torqueRigidbody, Vector3 direction, float speed)
    {
        Destroy(destroyedJoint);

        torqueRigidbody.AddTorque(direction * speed);
    }
    private void GenericActuatorMovement(FixedJoint destroyedJoint, 
        GameObject objToAddJoint, ref FixedJoint addJointIn,Rigidbody connectedBody,
        Rigidbody torqueRigidbody,Vector3 direction,float speed) 
    {
        Destroy(destroyedJoint);

        addJointIn = Jointify(objToAddJoint, connectedBody);

        torqueRigidbody.AddTorque(direction * speed);
    }
    private FixedJoint Jointify(GameObject obj, Rigidbody connectedBody)
    {
        FixedJoint joint = obj.GetComponent<FixedJoint>();

        if (!obj.GetComponent<FixedJoint>())
        {
            joint = obj.AddComponent<FixedJoint>();
            joint.connectedBody = connectedBody;
        }

        return joint;
    }
}
