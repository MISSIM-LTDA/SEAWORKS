using UnityEngine;

public class Lock_LTPC : MonoBehaviour {

    private Rigidbody rovRigidBody;

    private Rigidbody rigidBody;
    [SerializeField] private MeshCollider meshCollider;

    private FixedJoint joint;

    [SerializeField] private KeyCode lockKey = KeyCode.JoystickButton4;
    [SerializeField] private KeyCode releaseKey = KeyCode.JoystickButton5;

    private bool checkIn;
    private bool locked;
    void Start () {
        rovRigidBody = GameObject.FindGameObjectWithTag
            ("FLOT").GetComponent<Rigidbody>();

        rigidBody = gameObject.GetComponent<Rigidbody>();
    }
	
	void FixedUpdate () {
        if (checkIn && Input.GetKey(lockKey))
        {
            if (!locked) {
                meshCollider.enabled = false;

                rigidBody.isKinematic = false;

                joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = rovRigidBody;

                locked = true;
            }
        }

        else if (locked && Input.GetKey(releaseKey)) {
            DestroyImmediate(joint);

            rigidBody.isKinematic = true;

            meshCollider.enabled = true;

            locked = false;
        }
    }
    private void OnTriggerEnter(Collider collision){ checkIn = true; }
    private void OnTriggerExit(Collider collision) { checkIn = false; }
}
