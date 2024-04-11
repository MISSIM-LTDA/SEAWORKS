using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PTControl2 : MonoBehaviour {

    float RotAngle;
    float RotAngle2;
    float RotAngle3 = 0;

    

    // Update is called once per frame
    void Update()
    {

        RotAngle = transform.eulerAngles.x;
        //RotAngle = transform.localEulerAngles.x;


        if (Input.GetKey(KeyCode.L))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);
            RotAngle = RotAngle + 1;
            RotAngle3 = RotAngle3 + 1;

            

            if (RotAngle3 < 45)
            {
                transform.Rotate(0, 0, 1);

            }

            if (RotAngle3 > 45)
            {
                RotAngle3 = 44;
            }
            
        }

        if (Input.GetKey(KeyCode.K))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);
           
            RotAngle = RotAngle - 1;
            RotAngle3 = RotAngle3 - 1;


            if (RotAngle3 > -50)
            {
                transform.Rotate(0, 0, -1);
            }

            if (RotAngle3 < -50)
            {

                RotAngle3 = -49;
            }

           



        }
        //Debug.Log(RotAngle);




    }
}
