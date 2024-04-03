using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EFL_connect : MonoBehaviour {

    public GameObject EFL;
    public GameObject Manip;
    public GameObject ExternalRig;
    public GameObject Trava2;

    public Rigidbody EFLrig;
    public Rigidbody Maniprig;
    public Rigidbody TravaB2;

    int Lock1;

    public KeyCode Trava1;
    public KeyCode Solta1;
    public KeyCode Solta2;

    public float Breakeforce1;

    public bool CheckIn;

    public bool free;


    // Use this for initialization
    void Start()
    {
        Lock1 = 2;
        CheckIn = false;
        Manip = GameObject.FindWithTag("Jaw7");
    }

    // Update is called once per frame
    void Update()
    {


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

                // LTPC_MeshCollider.SetActive(false);
                //Destroy(EFL.GetComponent<FixedJoint>());
                //Trava2.SetActive(true);

                //LTPC_RigidBody.AddComponent<Rigidbody>();
                //LTPC_RigidBody.GetComponent<Rigidbody>().mass = 2;
                // LTPC_RigidBody.GetComponent<Rigidbody>().isKinematic = false;
                //EFL.AddComponent<FixedJoint>();

                EFL.GetComponent<FixedJoint>().connectedBody = Manip.GetComponent<Rigidbody>();

                //Destroy(LTPC_MeshCollider.GetComponent<MeshCollider>());

                //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = false;

                Lock1 = 0;

            }

        }

        if (Input.GetKey(Solta1) && Lock1 == 0)
        {

            // TMS.transform.parent = Conjunto.transform;           

            //Destroy(Aker_struc.GetComponent<FixedJoint>());

            //Aker_struc.AddComponent<FixedJoint>();

            //Aker_struc.GetComponent<FixedJoint>().connectedBody = TravaB2;

            //EFL.AddComponent<FixedJoint>();
            
            EFL.GetComponent<FixedJoint>().connectedBody = null;
            
            //Trava2.SetActive(false);

            //Destroy(LTPC_RigidBody.GetComponent<Rigidbody>());
            //LTPC_RigidBody.GetComponent<Rigidbody>().isKinematic = true;

            //LTPC_MeshCollider.AddComponent<MeshCollider>();
            //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = false;
            //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = true;
            //LTPC_MeshCollider.SetActive(true);
            //LTPC_MeshCollider.GetComponent<MeshCollider>().enabled = true;
            Lock1 = 2;

        }

        if (Input.GetKey(Solta2) && Lock1 == 0)
        {
            EFL.GetComponent<FixedJoint>().connectedBody = ExternalRig.GetComponent<Rigidbody>();
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