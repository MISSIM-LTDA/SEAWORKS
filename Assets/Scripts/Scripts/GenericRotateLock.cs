using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericRotateLock : MonoBehaviour {

    float RotAngle;
    float RotAngle2;
    public float RotAngle3 = 0;
    public KeyCode ShouderLeft;
    public KeyCode ShouderRight;
    public float LimitMin;
    public float limitMax;

    public float speed;

    public bool X;
    public bool Y;
    public bool Z;

    public Vector3 myVector3;


    // Update is called once per frame
    private void Start()
    {

      
    }
    void Update()
    {

        //RotAngle = transform.eulerAngles.y;
        //RotAngle = transform.localEulerAngles.x;

        myVector3 = transform.position;

        //myVector3 = gameObject.transform.eulerAngles;

        //var adjusted = 0;
        //myVector3.x = 0;
        myVector3.y = 0;
        //transform.LookAt(adjusted);


        if (X)
        {
            myVector3.x = 0;
        }

        if (Y)
        {
            myVector3.y = 0;
        }

        if (Z)
        {
            myVector3.z = 0;
        }

    }
}