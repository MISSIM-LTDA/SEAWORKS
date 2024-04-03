using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterMovement: MonoBehaviour {


    public Transform blade1;
    public Transform blade2;
    public Transform blade3;
    public Transform blade4;
    public Transform blade5;
    public Transform blade6;
    public Transform blade7;
    public float rotRate = 1000.0f;
    public float rotRate2 = 1000.0f;
    //private bool isOnGround;
    //private float distFromGround;
    //private float heightOfGround;
    //private float yScale;
    //private float rayStart = 0.4f;
    //private float rayLength = 0.3f;
    //private bool soundPlaying = true;

    void Start()
    {
        //yScale = transform.localScale.z;
    }    

    void Update()
    {        
            blade1.Rotate(Vector3.down * Time.deltaTime * rotRate);
            blade2.Rotate(Vector3.down * Time.deltaTime * rotRate);
            blade3.Rotate(Vector3.up * Time.deltaTime * rotRate);
            blade4.Rotate(Vector3.up * Time.deltaTime * rotRate);
            blade5.Rotate(Vector3.up * Time.deltaTime * rotRate2);
            blade6.Rotate(Vector3.up * Time.deltaTime * rotRate2);
            blade7.Rotate(Vector3.up * Time.deltaTime * rotRate2); 
    }

}