using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraGiro : MonoBehaviour
{

    float RotAngle;
    float RotAngle2;
    float RotAngle3 = 0;


    // Update is called once per frame
    void Update()
    {

        RotAngle = transform.eulerAngles.y;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetKey(KeyCode.V))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngle = RotAngle + 1;
            RotAngle3 = RotAngle3 + 1;



            if (RotAngle3 < 90)
            {
                transform.Rotate(0, 0, 1);

            }

            if (RotAngle3 > 90)
            {
                RotAngle3 = 89;
            }


        }

        if (Input.GetKey(KeyCode.B))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            RotAngle = RotAngle - 1;
            RotAngle3 = RotAngle3 - 1;


            if (RotAngle3 > -90)
            {
                transform.Rotate(0, 0, -1);
            }

            if (RotAngle3 < -90)
            {

                RotAngle3 = -89;
            }





        }
        Debug.Log(RotAngle);




    }
}
