using UnityEngine;
public class GrabObject : MonoBehaviour
{
    private Rigidbody basketRigidbody;

    private Rigidbody grabObjectRigidbody;
    private Joint grabObjectJoint;

    private GameObject manipulator;
    private Rigidbody manipulatorRigidbody;

    [SerializeField] private KeyCode lockKey = KeyCode.JoystickButton5;
    [SerializeField] private KeyCode leaveKey = KeyCode.JoystickButton4;

    private bool checkIn;
    private bool onBasket;
    private bool locked;
    void Start()
    {
        basketRigidbody = GameObject.Find("Basket").GetComponent<Rigidbody>();

        grabObjectRigidbody = GetComponent<Rigidbody>();

        grabObjectJoint = GetComponent<Joint>();
        grabObjectJoint.connectedBody = basketRigidbody;

        manipulator = GameObject.FindGameObjectWithTag("Jaw7");
        manipulatorRigidbody = manipulator.GetComponent<Rigidbody>();

    }
    void Update()
    {
        if (Input.GetKey(lockKey) && checkIn && !locked){
            if (!grabObjectJoint) {
                grabObjectJoint = gameObject.AddComponent<HingeJoint>();
            }

            grabObjectJoint.connectedBody = manipulatorRigidbody;

            locked = true;
        }

        else if (Input.GetKey(leaveKey) && locked){
            if (onBasket) {
                if (!grabObjectJoint){
                    grabObjectJoint = gameObject.AddComponent<HingeJoint>();
                }

                grabObjectJoint.connectedBody = basketRigidbody;

                grabObjectRigidbody.isKinematic = true;
                grabObjectRigidbody.useGravity = false;
            }

            else {
                Destroy(grabObjectJoint);

                grabObjectRigidbody.isKinematic = false;
                grabObjectRigidbody.useGravity = true;
            }

            locked = false;
        }
    }
    private void OnTriggerEnter(Collider collision)
    { 
        Transform obj = collision.transform;

        if(obj.tag == "Jaw7finger") {checkIn = true;}

        else if(obj.tag == "ROVComponents") { onBasket = true; }
    }
    private void OnTriggerExit(Collider collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.tag == "Jaw7finger") { checkIn = false; }

        else if (obj.tag == "ROVComponents") { onBasket = false; }
    }
}