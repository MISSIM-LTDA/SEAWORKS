using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTControl : MonoBehaviour
{

    public float RotAngle;
    public float RotAngle2;
    public float RotAngle3 = 0;

    public float limit;


    // Update is called once per frame
    void Update()
    {

        RotAngle = transform.eulerAngles.x;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetKey(KeyCode.O))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngle = RotAngle + 1;
            RotAngle3 = RotAngle3 + 1;



            if (RotAngle3 < limit)
            {
                transform.Rotate(1, 0, 0);

            }

            if (RotAngle3 > limit)
            {
                RotAngle3 = (limit -1);
            }


        }

        if (Input.GetKey(KeyCode.P))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            RotAngle = RotAngle - 1;
            RotAngle3 = RotAngle3 - 1;


            if (RotAngle3 > -limit)
            {
                transform.Rotate(-1, 0, 0);
            }

            if (RotAngle3 < -limit)
            {

                RotAngle3 = (limit - 1)*-1;
            }





        }
        //Debug.Log(RotAngle);




    }
}