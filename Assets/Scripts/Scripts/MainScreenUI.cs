using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

public class MainScreenUI : MonoBehaviour {
    
    public GameObject OrbitalView;
    public GameObject FPVView;
    public GameObject Camera;
    public GameObject CameraPivot;
    public GameObject FPVViewObj;
    public GameObject OrbitalViewObj;
    public Transform FPVCamTransform;


    public void FPVOn()
    {
        OrbitalView.SetActive(true);
        FPVView.SetActive(false);
        FPVViewObj.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Camera.GetComponent<CameraOrbit>().enabled = false;
        Camera.transform.parent = FPVViewObj.transform;
        Camera.transform.position = FPVCamTransform.position;
        CameraPivot.transform.eulerAngles = new Vector3(0, 0, 0);



    }

    public void OrbitalOn()
    {
        OrbitalView.SetActive(false);
        FPVView.SetActive(true);
        FPVViewObj.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

        OrbitalViewObj.SetActive(true);
        Camera.GetComponent<CameraOrbit>().enabled = true;
        Camera.transform.parent = OrbitalViewObj.transform;
        Camera.transform.eulerAngles = new Vector3(0, 0, 0);
        CameraPivot.transform.eulerAngles = new Vector3(0, 0, 0);


    }

    public void Camreset()
    {

        CameraPivot.transform.eulerAngles = new Vector3(0, 0, 0);


    }

   







}
