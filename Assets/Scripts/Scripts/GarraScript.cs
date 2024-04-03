using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraScript : MonoBehaviour
{

    public float RotAngle;
    public float RotAngle2;
    public float RotAngle3 = 0;


    // Update is called once per frame
    void Update()
    {

        RotAngle = transform.eulerAngles.y;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetKey(KeyCode.N))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            RotAngle3 = RotAngle3 + 1;

            if (RotAngle3 < 45)
            {
                transform.Rotate(0, 0, 1);


            }

            if (RotAngle3 > 45)
            {
                RotAngle3 = 46;
            }




        }

        if (Input.GetKey(KeyCode.M))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);
            RotAngle3 = RotAngle3 - 1;

            if (RotAngle3 > 0)
            {
                transform.Rotate(0, 0, -1);
            }

            if (RotAngle3 < 1)
            {

                RotAngle3 = 0;
            }




        }
        Debug.Log(RotAngle);




    }
}
