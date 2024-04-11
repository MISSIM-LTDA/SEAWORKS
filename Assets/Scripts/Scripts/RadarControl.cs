using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarControl : MonoBehaviour
{

    public Toggle[] checkboxes;
    public bool[] checkboxStates;


    public bool[] checkboxStates2;
    public Toggle[] checkboxes2;

    public FieldOfView fieldOfView;

    public Camera cameraRadar;

    void Start()
    {
        fieldOfView = GameObject.FindObjectOfType<FieldOfView>(true);

        checkboxStates = new bool[checkboxes.Length];
        for (int i = 0; i < checkboxes.Length; i++)
        {
            checkboxStates[i] = checkboxes[i].isOn;
        }


        checkboxStates2 = new bool[checkboxes2.Length];
        for (int i = 0; i < checkboxes2.Length; i++)
        {
            checkboxStates2[i] = checkboxes2[i].isOn;
        }

    }

    // Update is called once per frame
    void Update()
    {
        RadarSettings(checkboxes, checkboxStates);
        RadarSettings(checkboxes2, checkboxStates2);
        RadarON();
    }

    void RadarSettings(Toggle[] checkboxes, bool[] checkboxStates)
    {
        for (int i = 0; i < checkboxes.Length; i++)
        {
            if (checkboxes[i].isOn != checkboxStates[i])
            {
                checkboxStates[i] = checkboxes[i].isOn;
                if (checkboxes[i].isOn)
                {
                    foreach (var otherCheckbox in checkboxes)
                    {
                        if (otherCheckbox != checkboxes[i])
                        {
                            otherCheckbox.isOn = false;
                        }
                    }
                    break;
                }
            }
        }
        for (int i = 0; i < checkboxes.Length; i++)
        {
            // Get the Toggle component of the checkbox
            Toggle toggle = checkboxes[i].GetComponent<Toggle>();

            // Set the interactable state of the checkbox
            if (toggle.isOn)
            {
                toggle.interactable = false;
            }
            else
            {
                toggle.interactable = true;
            }

        }
    }
    void RadarON()
    {
        if(checkboxes[0].isOn)
        {
            fieldOfView.viewAngle = 45;
        }else if (checkboxes[1].isOn)
        {
            fieldOfView.viewAngle = 90;
        }
        else if (checkboxes[2].isOn)
        {
            fieldOfView.viewAngle = 120;
        }
        else if (checkboxes[3].isOn)
        {
            fieldOfView.viewAngle = 180;
        }


        //-----------------------------------------------------
        if (checkboxes2[0].isOn == true)
        {
            fieldOfView.viewRadius = 50;
            cameraRadar.orthographicSize = 53.2f;
        }
        else if (checkboxes2[1].isOn == true)
        {
            fieldOfView.viewRadius = 100;
            cameraRadar.orthographicSize = 107.3f;
            
        }
        else if (checkboxes2[2].isOn == true)
        {
            fieldOfView.viewRadius = 150;
            cameraRadar.orthographicSize = 161;
        }
        else if (checkboxes2[3].isOn == true)
        {
            fieldOfView.viewRadius = 200;
            cameraRadar.orthographicSize = 213.5f;
        }
        else if (checkboxes2[4].isOn == true)
        {
            fieldOfView.viewRadius = 250;
            cameraRadar.orthographicSize = 273;
        }
    }
}
