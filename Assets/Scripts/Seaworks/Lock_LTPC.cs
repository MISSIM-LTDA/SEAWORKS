using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock_LTPC : MonoBehaviour {

    public GameObject ROV;
    public GameObject Aker_struc;
    public GameObject Trava2;

    public GameObject LTPC_Render;
    public GameObject LTPC_MeshCollider;
    public GameObject LTPC_RigidBody;

    public Rigidbody ROVrigb;
    public Rigidbody Akerrigb;
    public Rigidbody TravaB2;

    bool locked;

    public KeyCode Trava1;
    public KeyCode Solta;

    public bool CheckIn;

    // Use this for initialization
    void Start () {
        ROV = GameObject.FindGameObjectWithTag("XLX");
        ROVrigb = ROV.GetComponent<Rigidbody>();
        locked = false;
        CheckIn = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(Trava1) && CheckIn)
        {
            if (!locked)
            {
                LTPC_MeshCollider.SetActive(false);

                LTPC_RigidBody.GetComponent<Rigidbody>().mass = 2;
                LTPC_RigidBody.GetComponent<Rigidbody>().isKinematic = false;
                LTPC_RigidBody.AddComponent<FixedJoint>();
                LTPC_RigidBody.GetComponent<FixedJoint>().connectedBody = ROV.GetComponent<Rigidbody>();

                locked = true;
            }

        }

        if (Input.GetKey(Solta) && locked)
        {
            Destroy(LTPC_RigidBody.GetComponent<FixedJoint>());

            LTPC_RigidBody.GetComponent<Rigidbody>().isKinematic = true;
            LTPC_MeshCollider.SetActive(true);

            locked = false;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        CheckIn = true;
        GameObject obj1 = this.gameObject;
        GameObject obj2 = collision.gameObject;
    }
    private void OnTriggerExit(Collider collision)
    {
        CheckIn = false;
        GameObject obj1 = this.gameObject;
        GameObject obj2 = collision.gameObject;
    }
}
