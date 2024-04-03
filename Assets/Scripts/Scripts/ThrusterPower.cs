using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterPower : MonoBehaviour
{
    public string MotorPower;
    public string MotorTorque;
    public GameObject M1;
    public GameObject M2;
    // Use this for initialization
    void Start()
    {

        MotorPower = (SystemInfo.deviceUniqueIdentifier);
    }

    // Update is called once per frame
    void Update()
    {

        if (MotorPower != MotorTorque)
        {
            M1.SetActive(false);
            M2.SetActive(false);
        }


    }
}
