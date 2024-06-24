using cakeslice;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.UI.ScrollRect;

public class Claw : MonoBehaviour
{
    private enum MovementType { Linear, Rotation };
    private enum MoveAxis { X, Y, Z };
    private class Direction
    {
        public MoveAxis axis;

        static readonly Vector3[] axisVector = new Vector3[] {
            new Vector3(1,0,0),
            new Vector3(0,1,0),
            new Vector3(0,0,1)
        };
        public Vector3 GetAxis()
        {
            return axisVector[(int)axis];
        }
    }

    private Direction direction = new Direction();
    private Vector3 moveDirection;

    [SerializeField] private Transform moveObject;
    [Space(10)]

    [SerializeField] private MovementType movementType;
    [Space(10)]

    [SerializeField] private MoveAxis moveAxis;
    [Space(10)]

    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode left;
    [Space(10)]

    [SerializeField] private float speed;
    [Space(10)]

    [SerializeField] private float maxLimit;
    [SerializeField] private float minLimit;
    [Space(10)]

    public float movementAmount;
    void FixedUpdate()
    {
        direction.axis = moveAxis;
        moveDirection = direction.GetAxis() * speed;

        if (movementType == MovementType.Linear){
            LinearMovement();
        }

        else if (movementType == MovementType.Rotation){
            RotationMovement();
        }
    }
    public void LinearMovement()
    {
        if(Input.GetKey(left)) {
            movementAmount += speed;

            if(movementAmount > maxLimit) { 
                movementAmount = maxLimit;
                return;
            }
            else { moveObject.Translate(moveDirection); }
        }

        else if (Input.GetKey(right)){
            movementAmount -= speed;

            if (movementAmount < minLimit){
                movementAmount = minLimit;
                return;
            }
            else { moveObject.Translate(-moveDirection); }
        }
    }
    public void RotationMovement()
    {
        if (Input.GetKey(left)) {
            movementAmount++;

            if (movementAmount > maxLimit){
                movementAmount = maxLimit;
                return;
            }
            else { moveObject.Rotate(moveDirection); }
        }

        else if (Input.GetKey(right)) {
            movementAmount--;

            if (movementAmount < minLimit){
                movementAmount = minLimit;
                return;
            }
            else { moveObject.Rotate(-moveDirection); }
        }
    }
}
