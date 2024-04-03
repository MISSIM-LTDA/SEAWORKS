using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraScript2 : MonoBehaviour
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



            if (RotAngle < 45)
            {
                transform.Rotate(0, 0, 1);


            }

            if (RotAngle > 44)
            {
                RotAngle = 45;
            }


        }

        if (Input.GetKey(KeyCode.M))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            if (RotAngle > 0)
            {
                transform.Rotate(0, 0, -1);
            }

            if (RotAngle < 1)
            {

                RotAngle = 180;
            }





        }
        Debug.Log(RotAngle);




    }
}
