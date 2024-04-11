using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DptAltControl : MonoBehaviour
{
    public Transform rov;
    public Transform ground;
    public Transform Surface;

    public Text DPT;
    public Text ALT;

    public float joyInput;
    public float difference;
    public float Dpt;

    public float DepthTotal;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        joyInput = Input.GetAxis("DroneUpDown");
        if (ground != null) {
            //get the difference between the ROV and the ground
            difference = ((rov.position.y - ground.position.y) / 7);
        }
        else{
            Debug.Log("No ground found");
        }

        //set the surface Y in the Depth needed for the simulation
        Surface.transform.position = new Vector3(Surface.position.x, DepthTotal * 7, Surface.position.z);

        if (Surface != null) {
            //get the difference between the ROV and the surface

            Dpt = ((Surface.position.y - rov.position.y) / 7);
        }
        else
        {
            Debug.Log("No Surface found");
        }
       

        ALT.text = "ALT: " + (difference).ToString("f0") + "m";
        DPT.text = "DPT: " + (Dpt).ToString("f0") + "m";
        }
    }

