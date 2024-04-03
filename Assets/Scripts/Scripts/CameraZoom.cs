using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {
    public Camera Cam;
    public float m_FieldOfView;

    public float m_FieldOfViewLimit;

    public KeyCode ZoomIN;
    public KeyCode ZoomOUT;

    // Update is called once per frame
    void Update () {

        Cam.fieldOfView = m_FieldOfView;

        if (Input.GetKey(ZoomIN))
        {
            m_FieldOfView = m_FieldOfView - 1;
        }

        if (Input.GetKey(ZoomOUT))
        {

            if (m_FieldOfView < m_FieldOfViewLimit)
            {
                m_FieldOfView = m_FieldOfView + 1;
            }
        }

    }
}
