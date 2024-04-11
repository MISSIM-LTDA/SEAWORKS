using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigMasterControl : MonoBehaviour {

    public GameObject ObjectRotate;
    float RotAngle;
    float RotAngle2;
    float RotAngle3 = 0;
    public float Speed = 1;
    public KeyCode ShouderLeft;
    public KeyCode ShouderRight;
    public float LimitMax;
    public float LimitMin;


    // Update is called once per frame
    void Update()
    {

        RotAngle = ObjectRotate.transform.eulerAngles.y;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetKey(ShouderLeft))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngle = RotAngle + 1;
            RotAngle3 = RotAngle3 + 1;



            if (RotAngle3 < LimitMax)
            {
                ObjectRotate.transform.Rotate(0, 0, Speed);

            }

            if (RotAngle3 > LimitMax)
            {
                RotAngle3 = (LimitMax-1);
            }


        }

        if (Input.GetKey(ShouderRight))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            RotAngle = RotAngle - 1;
            RotAngle3 = RotAngle3 - 1;


            if (RotAngle3 > LimitMin)
            {
                ObjectRotate.transform.Rotate(0, 0, -Speed);
            }

            if (RotAngle3 < LimitMin)
            {

                RotAngle3 = (LimitMin+1);
            }





        }
        //Debug.Log(RotAngle);




    }
}
