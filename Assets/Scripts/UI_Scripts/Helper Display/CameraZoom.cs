using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour {

    private CamerasControl camerasControl;

    private Camera mainCamera;
    public Camera auxCamera;

    private Slider mainCameraslider;
    private Slider auxCameraslider;

    private void Start()
    {
        camerasControl = FindObjectOfType<CamerasControl>();

        Slider[] slider = GetComponentsInChildren<Slider>();
        mainCameraslider = slider[0];
        auxCameraslider = slider[1];
    }
    private void Update()
    {
        if(camerasControl.actualCamera != null) {
            mainCamera = camerasControl.actualCamera.GetComponent<Camera>();

            mainCameraslider.interactable = true;
            mainCamera.fieldOfView = mainCameraslider.value;
        }
        else { 
            mainCamera = null;
            mainCameraslider.interactable = false;
        }

        if (camerasControl.auxCamera != null){
            auxCamera = camerasControl.auxCamera.GetComponent<Camera>();

            auxCameraslider.interactable = true;
            auxCamera.fieldOfView = auxCameraslider.value;
        }
        else{
            auxCamera = null;
            auxCameraslider.interactable = false;
        }
    }
}
