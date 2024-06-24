using UnityEngine;
public class GenericMovement : MonoBehaviour
{
    private enum MovementType { Linear, Rotation };
    [SerializeField] private MovementType movementType;
    private enum MoveAxis { X, Y, Z };
    [SerializeField] private MoveAxis moveAxis;
    private class Direction
    {
        public MoveAxis axis;

        static readonly Vector3[] axisVector = new Vector3[] {
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(0,0,1)
        };
        public Vector3 GetAxis(){
            return axisVector[(int)axis];
        }
    }

    private Direction direction = new Direction();
    private Vector3 moveDirection;

    [SerializeField] private float speed;

    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode left;

    private Rigidbody rigidBody;

    private Joint joint;
    private Rigidbody connectedBody;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        joint = GetComponent<FixedJoint>();

        connectedBody = joint.connectedBody;
    }
    void FixedUpdate()
    {
        direction.axis = moveAxis;
        moveDirection = direction.GetAxis() * speed;

        if (movementType == MovementType.Linear) { 
            LinearMovement(); 
        }

        else if (movementType == MovementType.Rotation) { 
            RotationMovement(); 
        }
    }
    public void LinearMovement() 
    {
        if(Input.GetKey(right) || Input.GetKey(left)) 
        {
            moveDirection = Input.GetKey(left) ? 
                moveDirection = -moveDirection : moveDirection;

            Destroy(joint);

            rigidBody.MovePosition(transform.position + 
                moveDirection * Time.fixedDeltaTime * speed);
        }

        else if(!joint) { ReAddJoint(); }
    }
    public void RotationMovement() 
    {
        if (Input.GetKey(right) || Input.GetKey(left)) 
        {
            moveDirection = Input.GetKey(left) ?
                moveDirection = -moveDirection : moveDirection;

            Destroy(joint);

            Quaternion deltaRotation = Quaternion.Euler
                (moveDirection * Time.fixedDeltaTime);

            rigidBody.MoveRotation(rigidBody.rotation * deltaRotation);
        }

        else if(!joint){ ReAddJoint(); }
    }
    public void ReAddJoint() 
    {
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = connectedBody;
    }
}



