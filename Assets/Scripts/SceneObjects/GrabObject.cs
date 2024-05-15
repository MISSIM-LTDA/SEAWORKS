using UnityEngine;
public class GrabObject : MonoBehaviour
{
    private Joint grabObjectJoint;

    private GameObject manipulator;
    private Rigidbody manipulatorRigidbody;

    private GameObject basket;
    private Rigidbody basketRigidbody;

    [SerializeField] private KeyCode lockKey = KeyCode.JoystickButton5;
    [SerializeField] private KeyCode leaveKey = KeyCode.JoystickButton4;
    [SerializeField] private KeyCode leaveOnWorldKey;

    private bool checkIn;
    private bool onBasket;
    private bool locked;
    void Start()
    {
        grabObjectJoint = gameObject.GetComponent<Joint>();

        manipulator = GameObject.FindGameObjectWithTag("Jaw7");
        manipulatorRigidbody = manipulator.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (Input.GetKey(lockKey) && checkIn && !locked){
            grabObjectJoint.connectedBody = manipulatorRigidbody;

            locked = true;
        }

        else if (Input.GetKey(leaveKey) && locked){
            if (onBasket) { grabObjectJoint.connectedBody = basketRigidbody; }

            else { grabObjectJoint.connectedBody = null; }

            locked = false;
        }

        else if (Input.GetKey(leaveOnWorldKey) && locked){
            Destroy(grabObjectJoint);

            locked = false;
        }
    }
    private void OnTriggerStay(Collider collision)
    { 
        GameObject obj = collision.gameObject;

        if(obj.tag == "Jaw7finger") {checkIn = true;}

        else if(obj.tag == "ROVComponents") {
            basket = obj.transform.parent.gameObject;
            basketRigidbody = obj.GetComponent<Rigidbody>();
            onBasket = true; 
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.tag == "Jaw7finger") { checkIn = false; }

        else if (obj.tag == "ROVComponents") {
            basket = null;
            basketRigidbody = null;
            onBasket = false; 
        }
    }
}