using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CampTorque2 : MonoBehaviour
{


    public float amount = 50f;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {



        if (Input.GetKey(KeyCode.T))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);
            // float h = Input.GetAxis("Horizontal") * amount * Time.deltaTime;
            rb.AddTorque(transform.forward * 2);

        }

        if (Input.GetKey(KeyCode.Y))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            //float v = Input.GetAxis("Vertical") * amount * Time.deltaTime;
            rb.AddTorque(-transform.forward * 2);
        }
    }
}
