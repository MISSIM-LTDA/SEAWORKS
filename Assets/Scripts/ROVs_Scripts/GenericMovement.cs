using UnityEngine;
public class GenericMovement : MonoBehaviour
{
    private Rigidbody m_Rigidbody;
    private Joint m_Joint;

    private Rigidbody rovRigb;

    [SerializeField] private bool x;
    [SerializeField] private bool y;
    [SerializeField] private bool z;

    [SerializeField] private bool linear;
    [SerializeField] private bool rotation;

    [SerializeField] private float speed;

    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode left;
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Joint = GetComponent<FixedJoint>();

        rovRigb = transform.parent.parent.GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        Vector3 input = Vector3.zero;
        Vector3 eulerAngleVelocity = Vector3.zero;

        if (x){
            input = new Vector3(0, speed, 0);
            eulerAngleVelocity = new Vector3(0, speed, 0);
        }

        else if (y){
            input = new Vector3(speed, 0, 0);
            eulerAngleVelocity = new Vector3(speed, 0, 0);
        }

        else if (z){
            input = new Vector3(0, 0, speed);
            eulerAngleVelocity = new Vector3(0, 0, speed);
        }

        if (linear)
        {
            if (Input.GetKey(right)){
                Destroy(m_Joint);
                m_Rigidbody.MovePosition(transform.position + input * Time.deltaTime * speed);
            }

            else if (Input.GetKey(left)){  
                Destroy(m_Joint);
                m_Rigidbody.MovePosition(transform.position + (-input) * Time.deltaTime * speed);
            }

            else{
                if (!m_Joint){
                    m_Joint = gameObject.AddComponent<FixedJoint>();
                    m_Joint.connectedBody = rovRigb;
                }
            }
        }

        else if (rotation)
        {
            if (Input.GetKey(right)){
                Destroy(m_Joint);
                Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
            }

            else if (Input.GetKey(left)){
                Destroy(m_Joint);
                Quaternion deltaRotation = Quaternion.Euler((-eulerAngleVelocity) * Time.fixedDeltaTime);
                m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);
            }

            else{
                if (!m_Joint){
                    m_Joint = gameObject.AddComponent<FixedJoint>();
                    m_Joint.connectedBody = rovRigb;
                }
            }
        }
    }
}



