using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour {

    private Camera cam;
    private Slider slider;

    private void Start()
    {
        cam = Camera.main;
        slider = GetComponentInChildren<Slider>();

        if (cam && slider) {
            slider.onValueChanged.AddListener(OnSliderValueChange);
        }
    }
    private void OnSliderValueChange(float value) {
        cam.fieldOfView = value;
    }
}
