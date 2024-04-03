using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lars1 : MonoBehaviour {


    float RotAngle;
    float RotAngle3;
    float RotAngle4;
    float RotAngle5;
    public Vector3 RotAngle2;
    public GameObject ROV;
    public GameObject Bailarina;
    public GameObject TMS;
    public GameObject Conjunto;
    public GameObject Winch;
    private Rigidbody ConjuntoB;
    private Rigidbody BailarinaB;
    int Lock1;

    // Update is called once per frame

    private void Start()
    {
        Lock1 = 2;
        RotAngle5 = 90;

       
    }

    void Update()
    {

        ConjuntoB = Conjunto.GetComponent<Rigidbody>();
        BailarinaB = Bailarina.GetComponent<Rigidbody>();

        if (Input.GetKey(KeyCode.P))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

            
            RotAngle3 = transform.eulerAngles.z;
            RotAngle3 = RotAngle3 + 0.1f;
            

            //if (RotAngle > 45)
            //{
            //    RotAngle = 44;
            //}
            transform.rotation = Quaternion.Euler(0, -270, RotAngle3);

            

        }

        if (Input.GetKey(KeyCode.O))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);

          
            RotAngle3 = transform.eulerAngles.z;
            RotAngle3 = RotAngle3 - 0.1f;

            //if (RotAngle < -50)
            //{
            //    RotAngle = -49;
            //}
            transform.rotation = Quaternion.Euler(0, -270, RotAngle3);
          

        }


        if (Input.GetKey(KeyCode.L))
        {
            //  ROV.transform.parent = Bailarina.transform;
            //TMS.transform.parent = Bailarina.transform;
            // Conjunto.transform.parent = Bailarina.transform;
            if (Lock1 > 1)
            {            
                Conjunto.AddComponent<FixedJoint>();

                Conjunto.GetComponent<FixedJoint>().connectedBody = BailarinaB;
                Lock1 = 0;
            }

        }

        if (Input.GetKey(KeyCode.K))
        {
           
            // TMS.transform.parent = Conjunto.transform;           

            Destroy(Conjunto.GetComponent<FixedJoint>());
            Lock1 = 2;

        }
        

        if (Input.GetKey(KeyCode.S))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngle4 = Winch.transform.eulerAngles.z;
            RotAngle4 = RotAngle4 + 1f;


            //if (RotAngle > 45)
            //{
            //    RotAngle = 44;
            //}
            Winch.transform.rotation = Quaternion.Euler(0, -90, RotAngle4);

        }

        if (Input.GetKey(KeyCode.W))
        {
            // transform.rotation = Quaternion.AngleAxis(RotAngle, Vector3.right);


            RotAngle4 = Winch.transform.eulerAngles.z;
            RotAngle4 = RotAngle4 - 1f;


            //if (RotAngle > 45)
            //{
            //    RotAngle = 44;
            //}
            Winch.transform.rotation = Quaternion.Euler(0, -90, RotAngle4);

        }


        




    }
}
