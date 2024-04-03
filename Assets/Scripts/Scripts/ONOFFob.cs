using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONOFFob : MonoBehaviour
{

    public GameObject ob1;
    public GameObject ob2;
    public GameObject ob3;
    public KeyCode liga;
    public KeyCode desliga;
    public int on;
    public bool cam1;
    public bool cam2;
    public bool cam3;

    // Use this for initialization
    void Start()
    {
        on = 1;

        cam1 = true;
        cam2 = false;
        cam3 = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(liga) && on == 1)
        {
            cam1 = true;
            cam2 = false;
            cam3 = false;
            on = 2;
        }

        else
        {

            if (Input.GetKeyDown(liga) && on == 2)
            {
                cam1 = false;
                cam2 = true;
                cam3 = false;
                on = 3;
            }
            else
            {

                if (Input.GetKeyDown(liga) && on == 3)
                {
                    cam1 = false;
                    cam2 = false;
                    cam3 = true;
                    on = 1;
                }
                
            }
        }





            if (cam1 == true)
            {
                ob1.SetActive(true);
                //on = 3;
            }

            if (cam2 == true)
            {
                ob2.SetActive(true);
                //on = 3;
            }

            if (cam3 == true)
            {
                ob3.SetActive(true);
                //on = 3;
            }

            if (cam1 == false)
            {
                ob1.SetActive(false);
                //on = 3;
            }

            if (cam2 == false)
            {
                ob2.SetActive(false);
                //on = 3;
            }

            if (cam3 == false)
            {
                ob3.SetActive(false);
                //on = 3;
            }






        }
    }

