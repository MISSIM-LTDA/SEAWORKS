using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class GrabObjct : MonoBehaviour
{
    public GameObject EFL;
    public GameObject Manip;
    public GameObject ExternalRig;
    public GameObject Trava2;

    public Rigidbody EFLrig;
    public Rigidbody Maniprig;
    public Rigidbody TravaB2;

    public int Lock1;

    public KeyCode Trava1;
    public KeyCode Solta1;
    public KeyCode Solta2;
    public KeyCode Solta3;

    public float Breakeforce1;

    public bool CheckIn;

    public bool free;

    public bool Fixed;
    public bool Hinge;
    public bool Spring;

    private ObiSolver obiSolver;
    void Start()
    {
        Manip = GameObject.FindGameObjectWithTag("Jaw7");
        Maniprig = Manip.GetComponent<Rigidbody>();
        Lock1 = 2;
        CheckIn = false;
        obiSolver = GameObject.FindObjectOfType<ObiSolver>();
    }
    void Update()
    {
        if (Input.GetKey(Trava1) && CheckIn == true)
        {
            if (Lock1 > 1)
            {
                if(Fixed)
                {
                    EFL.GetComponent<FixedJoint>().connectedBody = Manip.GetComponent<Rigidbody>();
                }

                if (Hinge)
                {
                    EFL.GetComponent<HingeJoint>().connectedBody = Manip.GetComponent<Rigidbody>();
                }

                if (Spring)
                {
                    EFL.GetComponent<SpringJoint>().connectedBody = Manip.GetComponent<Rigidbody>();
                }

                Lock1 = 0;
            }
        }

        if (Input.GetKey(Solta1) && Lock1 == 0)
        {
            if (Fixed)
            {
                EFL.GetComponent<FixedJoint>().connectedBody = null;
            }

            if (Hinge)
            {
                EFL.GetComponent<HingeJoint>().connectedBody = null;
            }

            if (Spring)
            {
                EFL.GetComponent<SpringJoint>().connectedBody = null;
            }

            Lock1 = 2;

        }

        if (Input.GetKey(Solta2) && Lock1 == 0)
        {
            if (Fixed)
            {
                EFL.GetComponent<FixedJoint>().connectedBody = ExternalRig.GetComponent<Rigidbody>();
            }

            if (Hinge)
            {
                EFL.GetComponent<HingeJoint>().connectedBody = ExternalRig.GetComponent<Rigidbody>();
            }

            if (Spring)
            {
                EFL.GetComponent<SpringJoint>().connectedBody = ExternalRig.GetComponent<Rigidbody>();
            }
            Lock1 = 2;
        }

        if (Input.GetKey(Solta3) && Lock1 == 0)
        {
            Destroy(EFL.GetComponent<FixedJoint>());
            Lock1 = 2;
        }
    }


    private void OnTriggerStay(Collider collision)
    { 
        GameObject obj = collision.gameObject;

        if(obj.tag == "Jaw7finger") {CheckIn = true;}
    }
    private void OnTriggerExit(Collider collision)
    {
        CheckIn = false;
    }
}