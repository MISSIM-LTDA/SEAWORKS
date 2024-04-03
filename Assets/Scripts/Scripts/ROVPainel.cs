using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROVPainel : MonoBehaviour {

    public GameObject LED1;
    public GameObject LED2;
    public GameObject LED3;
    public GameObject LEDBUTTONON;
    public GameObject LEDBUTTONOFF;
    public GameObject camera1;
    public GameObject ThrSpin;
    public GameObject ROVcameras;
    public GameObject ROVcameras2;
    public GameObject CAMERASON;
    public GameObject CAMERASOFF;
    public GameObject Sonar;
    public GameObject Profiler;
    public GameObject SonarButtonOn;
    public GameObject SonarButtonOff;
    public GameObject ProfilerButtonOn;
    public GameObject ProfilerButtonOff;
    float LEDTroubleShooting;
    public float LEDTroubleShootingStatus;


    private void Start()
    {
        camera1 = GameObject.FindGameObjectWithTag("MainCamera");
        ThrSpin = GameObject.FindGameObjectWithTag("thrusterprop1");
        
    }


    private void Update()
    {
        LEDTroubleShootingStatus = LEDTroubleShooting;
    }







    public void Painelclick()
    {
        camera1.GetComponent<ItemSelect>().enabled = false;

    }

    public void DamageClick()
    {
        camera1.GetComponent<ItemSelect>().enabled = false;

    }

    public void ThrusterCWON()
    {
        ThrSpin.GetComponent<ThrusterSpinCW>().enabled = true;
        ThrSpin.GetComponent<ThrusterSpinCCW>().enabled = false;

    }

    public void ThrusterCCWON()
    {
        ThrSpin.GetComponent<ThrusterSpinCCW>().enabled = true;
        ThrSpin.GetComponent<ThrusterSpinCW>().enabled = false;

    }

    public void ThrusterOFF()
    {
        ThrSpin.GetComponent<ThrusterSpinCCW>().enabled = false;
        ThrSpin.GetComponent<ThrusterSpinCW>().enabled = false;

    }

    public void CameraOFF()
    {
        ROVcameras.SetActive(false);
        ROVcameras2.SetActive(false);
        CAMERASON.SetActive(true);
        CAMERASOFF.SetActive(false);

    }

    public void CameraON()
    {
        ROVcameras.SetActive(true);
        ROVcameras2.SetActive(true);
        CAMERASON.SetActive(false);
        CAMERASOFF.SetActive(true);

    }

    public void LigarLed()
    {
        if (LEDTroubleShootingStatus == 0)
        {
            LED1.SetActive(true);
            LED2.SetActive(true);
            LED3.SetActive(true);
            LEDBUTTONON.SetActive(false);
            LEDBUTTONOFF.SetActive(true);
        }
        if (LEDTroubleShootingStatus == 1)
        {
            LED1.SetActive(true);
            LED2.SetActive(true);
            LED3.SetActive(false);
            LEDBUTTONON.SetActive(false);
            LEDBUTTONOFF.SetActive(true);
        }
        if (LEDTroubleShootingStatus == 2)
        {
            LED1.SetActive(true);
            LED2.SetActive(false);
            LED3.SetActive(true);
            LEDBUTTONON.SetActive(false);
            LEDBUTTONOFF.SetActive(true);
        }
        if (LEDTroubleShootingStatus == 3)
        {
            LED1.SetActive(false);
            LED2.SetActive(true);
            LED3.SetActive(true);
            LEDBUTTONON.SetActive(false);
            LEDBUTTONOFF.SetActive(true);
        }

    }

    public void FalhaLed1()
    {
        LEDTroubleShooting = 1;
    }

    public void FalhaLed2()
    {
        LEDTroubleShooting = 2;
    }

    public void FalhaLed3()
    {
        LEDTroubleShooting = 3;
    }

    public void ReparoLed1()
    {
        LEDTroubleShooting = 0;
    }




    public void DesligarLed()
    {

        LED1.SetActive(false);
        LED2.SetActive(false);
        LED3.SetActive(false);
        LEDBUTTONON.SetActive(true);
        LEDBUTTONOFF.SetActive(false);

    }

    public void SonarOn()
    {
        SonarButtonOn.SetActive(false);
        SonarButtonOff.SetActive(true);
        Sonar.SetActive(true);
    }

    public void SonarOff()
    {
        SonarButtonOn.SetActive(true);
        SonarButtonOff.SetActive(false);
        Sonar.SetActive(false);
    }

    public void ProfilerOn()
    {
        ProfilerButtonOn.SetActive(false);
        ProfilerButtonOff.SetActive(true);
        Profiler.SetActive(true);
    }

    public void ProfilerOff()
    {
        ProfilerButtonOn.SetActive(true);
        ProfilerButtonOff.SetActive(false);
        Profiler.SetActive(false);
    }




}
