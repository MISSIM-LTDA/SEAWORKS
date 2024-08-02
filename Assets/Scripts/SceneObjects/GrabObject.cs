using UnityEngine;
public class GrabObject : MonoBehaviour
{
    private Rigidbody basketRigidbody;

    private Rigidbody grabObjectRigidbody;
    private Joint grabObjectJoint;

    private GameObject manipulator;
    private Rigidbody manipulatorRigidbody;

    [SerializeField] private KeyCode lockUnlockKey = KeyCode.JoystickButton8;

    private bool checkIn;
    private bool onBasket;
    private bool locked;
    void Start()
    {
        basketRigidbody = GameObject.Find("Basket").GetComponent<Rigidbody>();

        grabObjectRigidbody = GetComponent<Rigidbody>();

        grabObjectJoint = GetComponent<Joint>();

        manipulator = GameObject.FindGameObjectWithTag("Jaw7");
        manipulatorRigidbody = manipulator.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetKeyDown(lockUnlockKey) && checkIn && !locked){
            grabObjectJoint = GetComponent<Joint>();
            if (!grabObjectJoint) {
                grabObjectJoint = gameObject.AddComponent<FixedJoint>();
            }

            grabObjectRigidbody.isKinematic = false;

            grabObjectJoint.connectedBody = manipulatorRigidbody;

            locked = true;
        }

        else if (Input.GetKeyDown(lockUnlockKey) && locked){
            if (onBasket) {
                grabObjectJoint.connectedBody = basketRigidbody;
            }

            else {
                grabObjectJoint.connectedBody = null;
                grabObjectRigidbody.isKinematic = true;
            }

            grabObjectRigidbody.useGravity = false;
            locked = false;
        }
    }
    private void OnTriggerEnter(Collider collision)
    { 
        Transform obj = collision.transform;

        if(obj.tag == "Jaw7finger") {checkIn = true;}  

        else if(obj.tag == "Basket") { onBasket = true;}
    }
    private void OnTriggerExit(Collider collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.tag == "Jaw7finger") { checkIn = false; }

        else if (obj.tag == "Basket") { onBasket = false; }
    }
}