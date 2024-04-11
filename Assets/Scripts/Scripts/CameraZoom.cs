using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour {
    
    //public float m_FieldOfView;

    //public float m_FieldOfViewLimit;

    //public KeyCode ZoomIN;
    //public KeyCode ZoomOUT;

    public Slider slider;
    public Camera Cam;

    // Update is called once per frame
    void Update () {

        Cam.fieldOfView = slider.value;

        //if (Input.GetKey(ZoomIN))
        //{
        //    m_FieldOfView = m_FieldOfView - 1;
        //}

        //if (Input.GetKey(ZoomOUT))
        //{

        //    if (m_FieldOfView < m_FieldOfViewLimit)
        //    {
        //        m_FieldOfView = m_FieldOfView + 1;
        //    }
        //}

    }
}
