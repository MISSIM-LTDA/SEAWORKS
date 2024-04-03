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

    int Lock1;

    public KeyCode Trava1;
    public KeyCode Solta;

    public bool CheckIn;


    // Use this for initialization
    void Start () {
        ROV = GameObject.FindGameObjectWithTag("XLX");
        ROVrigb = ROV.GetComponent<Rigidbody>();
        Lock1 = 2;
        CheckIn = false;
    }
	
	// Update is called once per frame
	void Update () {

        
        if (Input.GetKey(Trava1) && CheckIn == true)
        {
            //  ROV.transform.parent = Bailarina.transform;
            //TMS.transform.parent = Bailarina.transform;
            // Conjunto.transform.parent = Bailarina.transform;
            if (Lock1 > 1)
            {
                //Destroy(Aker_struc.GetComponent<FixedJoint>());

                //Aker_struc.AddComponent<FixedJoint>();

                //Aker_struc.GetComponent<FixedJoint>().connectedBody = ROVrigb;

                // LTPC_RigidBody.transform.parent = LTPC_Render.transform;

                LTPC_MeshCollider.SetActive(false);

                //LTPC_RigidBody.AddComponent<Rigidbody>();
                LTPC_RigidBody.GetComponent<Rigidbody>().mass = 2;
                LTPC_RigidBody.GetComponent<Rigidbody>().isKinematic = false;
                LTPC_RigidBody.AddComponent<FixedJoint>();
                LTPC_RigidBody.GetComponent<FixedJoint>().connectedBody = ROV.GetComponent<Rigidbody>();

                //Destroy(LTPC_MeshCollider.GetComponent<MeshCollider>());

                //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = false;

                Lock1 = 0;
               
            }

        }

        if (Input.GetKey(Solta) && Lock1 == 0)
        {

            // TMS.transform.parent = Conjunto.transform;           

            //Destroy(Aker_struc.GetComponent<FixedJoint>());

            //Aker_struc.AddComponent<FixedJoint>();

            //Aker_struc.GetComponent<FixedJoint>().connectedBody = TravaB2;

            Destroy(LTPC_RigidBody.GetComponent<FixedJoint>());
            //Destroy(LTPC_RigidBody.GetComponent<Rigidbody>());
            LTPC_RigidBody.GetComponent<Rigidbody>().isKinematic = true;

            //LTPC_MeshCollider.AddComponent<MeshCollider>();
            //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = false;
            //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = true;
            LTPC_MeshCollider.SetActive(true);
            //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = true;
            Lock1 = 2;

        }


    }


    private void OnTriggerStay(Collider collision)
    {

        CheckIn = true;
        GameObject obj1 = this.gameObject;
        GameObject obj2 = collision.gameObject;

        Debug.Log("Triggered Obj1: :" + obj1.name);
        Debug.Log("Triggered obj2: :" + obj2.name);
    }


    private void OnTriggerExit(Collider collision)
    {

        CheckIn = false;
        GameObject obj1 = this.gameObject;
        GameObject obj2 = collision.gameObject;

        Debug.Log("Saiu");
    }


}
