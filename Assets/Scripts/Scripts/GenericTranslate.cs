using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTranslate : MonoBehaviour {

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


    // Update is called once per frame
    private void Start()
    {

        RotAngle3 = 0;
    }
    void Update()
    {

        //RotAngle = transform.eulerAngles.y;
        //RotAngle = transform.localEulerAngles.x;

        if (X)
        {
            if (Input.GetKey(ShouderLeft))
            {
                // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);



                RotAngle3 = RotAngle3 + speed;

                if (RotAngle3 < limitMax)
                {
                    transform.Translate(speed, 0, 0);
                }

                if (RotAngle3 >= limitMax)
                {
                    RotAngle3 = (limitMax + speed);
                }


            }

            if (Input.GetKey(ShouderRight))
            {
                // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

                RotAngle = RotAngle - speed;
                RotAngle3 = RotAngle3 - speed;


                if (RotAngle3 > LimitMin)
                {
                    transform.Translate(-speed, 0, 0);
                }

                if (RotAngle3 < LimitMin)
                {

                    RotAngle3 = (LimitMin - speed);
                }


            }
        }

        if (Y)
        {
            if (Input.GetKey(ShouderLeft))
            {
                // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


                RotAngle3 = RotAngle3 + speed;

                if (RotAngle3 < limitMax)
                {
                    transform.Translate(0, speed, 0);
                }

                if (RotAngle3 >= limitMax)
                {
                    RotAngle3 = (limitMax + speed);
                }


            }

            if (Input.GetKey(ShouderRight))
            {
                // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

                RotAngle = RotAngle - speed;
                RotAngle3 = RotAngle3 - speed;


                if (RotAngle3 > LimitMin)
                {
                    transform.Translate(0, -speed, 0);
                }

                if (RotAngle3 < LimitMin)
                {

                    RotAngle3 = (LimitMin - speed);
                }


            }
        }


        if (Z)
        {
            if (Input.GetKey(ShouderLeft))
            {
                // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);




                RotAngle3 = RotAngle3 + speed;

                if (RotAngle3 < limitMax)
                {
                    transform.Translate(0, 0, speed);
                }

                if (RotAngle3 >= limitMax)
                {
                    RotAngle3 = (limitMax + speed);
                }


            }

            if (Input.GetKey(ShouderRight))
            {
                // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

                //RotAngle = RotAngle - 0.01f;
                RotAngle3 = RotAngle3 - speed;


                if (RotAngle3 > LimitMin)
                {
                    transform.Translate(0, 0, -speed);
                }

                if (RotAngle3 < LimitMin)
                {

                    RotAngle3 = (LimitMin - speed);
                }


            }
        }


        //Debug.Log(RotAngle);


    }
}
