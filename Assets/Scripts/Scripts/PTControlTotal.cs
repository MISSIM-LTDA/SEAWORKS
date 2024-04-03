using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTControlTotal : MonoBehaviour
{

    public float RotAngleP;
    public float RotAngleP2;
    public float RotAngleP3 = 0;

    public float limitP;

    public float RotAngleT;
    public float RotAngleT2;
    public float RotAngleT3 = 0;

    public float limitT;

    public GameObject Pan;
    public GameObject Tilt;


    // Update is called once per frame
    void Update()
    {

        RotAngleP = Pan.transform.eulerAngles.x;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetAxis("T4S") <= -0.8)
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngleP = RotAngleP + 1;
            RotAngleP3 = RotAngleP3 + 1;



            if (RotAngleP3 < limitP)
            {
                Pan.transform.Rotate(0, 0, 1);

            }

            if (RotAngleP3 > limitP)
            {
                RotAngleP3 = (limitP -1);
            }


        }

        if (Input.GetAxis("T4S") >= 0.8)
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            RotAngleP = RotAngleP - 1;
            RotAngleP3 = RotAngleP3 - 1;


            if (RotAngleP3 > -limitP)
            {
                Pan.transform.Rotate(0, 0, -1);
            }

            if (RotAngleP3 < -limitP)
            {

                RotAngleP3 = (limitP - 1)*-1;
            }

        }
        //Debug.Log(RotAngle);

        // TILT----------
        
        
        RotAngleT = Tilt.transform.eulerAngles.x;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetAxis("T4UD") >= 0.8)
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngleT = RotAngleT + 1;
            RotAngleT3 = RotAngleT3 + 1;



            if (RotAngleT3 < limitT)
            {
                Tilt.transform.Rotate(1, 0, 0);

            }

            if (RotAngleT3 > limitT)
            {
                RotAngleT3 = (limitT -1);
            }


        }

        if (Input.GetAxis("T4UD") <= -0.8)
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            RotAngleT = RotAngleT - 1;
            RotAngleT3 = RotAngleT3 - 1;


            if (RotAngleT3 > -limitT)
            {
                Tilt.transform.Rotate(-1, 0, 0);
            }

            if (RotAngleT3 < -limitT)
            {

                RotAngleT3 = (limitT - 1)*-1;
            }

        }




    }
}