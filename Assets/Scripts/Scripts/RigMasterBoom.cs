using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigMasterBoom : MonoBehaviour {

    float RotAngle;
    float RotAngle2;
    float RotAngle3 = 0;
    public KeyCode ShouderLeft;
    public KeyCode ShouderRight;
    public float LimitMin;
    public float limitMax;


    // Update is called once per frame
    private void Start()
    {

        RotAngle3 = 0;
    }
    void Update()
    {

        //RotAngle = transform.eulerAngles.y;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetKey(ShouderLeft))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngle3 = RotAngle3 + 1;



            if (RotAngle3 < limitMax)
            {
                transform.Translate(0, 0.05f, 0);

            }

            if (RotAngle3 > limitMax)
            {
                RotAngle3 = limitMax-1;
            }


        }

        if (Input.GetKey(ShouderRight))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            RotAngle = RotAngle - 1;
            RotAngle3 = RotAngle3 - 1;


            if (RotAngle3 > LimitMin)
            {
                transform.Translate(0, -0.05f, 0);
            }

            if (RotAngle3 < LimitMin)
            {

                RotAngle3 = (LimitMin-1);
            }





        }
        Debug.Log(RotAngle);




    }
}
