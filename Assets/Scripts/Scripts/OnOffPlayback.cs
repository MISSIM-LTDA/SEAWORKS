using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnOffPlayback : MonoBehaviour

{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject cam5;

    public GameObject NoCam;

    public Camera cam1C;
    public Camera cam2C;
    public Camera cam3C;
    public Camera cam4C;
    public Camera cam5C;

    public bool on;

    public bool free;

    public Toggle Cam1Main;
    public Toggle Cam2Main;
    public Toggle Cam3Main;
    public Toggle Cam4Main;
    public Toggle Cam5Main;

    public Toggle Cam1Aux;
    public Toggle Cam2Aux;
    public Toggle Cam3Aux;
    public Toggle Cam4Aux;
    public Toggle Cam5Aux;


    void Start()
    {
        cam1 = GameObject.FindGameObjectWithTag("CAM1");
        cam2 = GameObject.FindGameObjectWithTag("CAM2");
        cam3 = GameObject.FindGameObjectWithTag("CAM3");
        cam4 = GameObject.FindGameObjectWithTag("CAM4");
        cam5 = GameObject.FindGameObjectWithTag("CAM5");
        cam1C = cam1.GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (free == false)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                free = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked; ;
                free = false;

            }
        }


        if (Cam1Main.isOn == true)
        {
            if (Cam1Aux.isOn == true)
            {
                Cam1Aux.isOn = false;
            }

            cam1.SetActive(true);
            cam1C.rect = new Rect(0, 0, 1, 1);
        }

        else if (Cam1Aux.isOn == true)
        {
            if (Cam1Main.isOn == true)
            {
                Cam1Main.isOn = false;
            }

            cam1.SetActive(true);
            cam1C.rect = new Rect(0.6f, 0.6f, 0.4f, 0.4f);
        }

        else
        {
            cam1.SetActive(false);
        }

        if (Cam5Main.isOn == true)
        {
            cam5.SetActive(true);

        }

        else
        {
            cam5.SetActive(false);
        }

        if (Cam1Main.isOn == false && Cam2Main.isOn == false && Cam3Main.isOn == false && Cam4Main.isOn == false && Cam5Main.isOn == false)
        {
            NoCam.SetActive(false);
        }

    }
}
