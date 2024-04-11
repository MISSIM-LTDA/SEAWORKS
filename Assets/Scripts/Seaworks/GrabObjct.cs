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

    public GameObject basket;

    public Rigidbody EFLrig;
    public Rigidbody Maniprig;
    public Rigidbody TravaB2;

    public int Lock1;

    public KeyCode Trava1;
    public KeyCode Solta1;
    public KeyCode Solta2;
    public KeyCode Solta3;

    public float Breakeforce1;

    public bool checkIn;
    public bool onBasket;

    public bool locked;

    private ObiSolver obiSolver;
    void Start()
    {
        Manip = GameObject.FindGameObjectWithTag("Jaw7");
        Maniprig = Manip.GetComponent<Rigidbody>();
        checkIn = false;
        onBasket = false;
        locked = false;
        obiSolver = GameObject.FindObjectOfType<ObiSolver>();
    }
    void Update()
    {
        if (Input.GetKey(Trava1) && checkIn && !locked)
        {
            EFL.GetComponent<Joint>().connectedBody = Manip.GetComponent<Rigidbody>();

            locked = true;
        }

        else if (Input.GetKey(Solta1) && locked)
        {
            if (onBasket) { EFL.GetComponent<Joint>().connectedBody = basket.GetComponent<Rigidbody>(); }

            else { EFL.GetComponent<Joint>().connectedBody = null; }

            locked = false;
        }

        else if (Input.GetKey(Solta2) && locked)
        {
            if (onBasket) { EFL.GetComponent<Joint>().connectedBody = basket.GetComponent<Rigidbody>(); }

            else { EFL.GetComponent<Joint>().connectedBody = ExternalRig.GetComponent<Rigidbody>(); }

            locked = false;
        }

        else if (Input.GetKey(Solta3) && locked)
        {
            Destroy(EFL.GetComponent<Joint>());

            locked = false;
        }
    }


    private void OnTriggerStay(Collider collision)
    { 
        GameObject obj = collision.gameObject;

        if(obj.tag == "Jaw7finger") {checkIn = true;}

        if(obj.tag == "ROVComponents") 
        {
            basket = obj.transform.parent.gameObject;
            onBasket = true; 
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.tag == "Jaw7finger") { checkIn = false; }

        if (obj.tag == "ROVComponents") 
        {
            basket = null;
            onBasket = false; 
        }
    }
}