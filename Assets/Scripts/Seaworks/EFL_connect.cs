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

    public KeyCode Trava1;
    public KeyCode Solta1;
    public KeyCode Solta2;

    bool locked;
    public bool CheckIn;
    void Start()
    {
        locked = false;
        CheckIn = false;
        Manip = GameObject.FindWithTag("Jaw7");
    }
    void Update()
    {
        if (Input.GetKey(Trava1) && CheckIn && !locked)
        {
            EFL.GetComponent<FixedJoint>().connectedBody = Manip.GetComponent<Rigidbody>();
            locked = true;
        }

        else if (Input.GetKey(Solta1) && locked)
        {
            EFL.GetComponent<FixedJoint>().connectedBody = null;
            locked = false;
        }

        else if (Input.GetKey(Solta2) && locked)
        {
            EFL.GetComponent<FixedJoint>().connectedBody = ExternalRig.GetComponent<Rigidbody>();
            locked = false;
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        CheckIn = true;
        GameObject obj1 = gameObject;
        GameObject obj2 = collision.gameObject;

        Debug.Log("Triggered Obj1: :" + obj1.name);
        Debug.Log("Triggered obj2: :" + obj2.name);
    }


    private void OnTriggerExit(Collider collision)
    {
        CheckIn = false;
        GameObject obj1 = gameObject;
        GameObject obj2 = collision.gameObject;

        Debug.Log("Saiu");
    }


}