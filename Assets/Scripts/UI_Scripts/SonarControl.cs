using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class SonarControl : MonoBehaviour
{
    private FieldOfView fieldOfView;

    private Camera sonarCamera;
    private float[] cameraValues = { 53.2f, 107.3f, 161.0f, 213.5f, 273.0f };

    private Toggle turnOnOffRadar;

    private Toggle[] angles;
    private int[] angleValues = { 45, 90, 120, 180 };
    private int actualAngle;

    private Toggle[] radius;
    private int[] radiusValues = { 50, 100, 150, 200,250 };
    private int actualRadius;
    void Start()
    {
        fieldOfView = FindObjectOfType<FieldOfView>(true);
        sonarCamera = GameObject.FindGameObjectWithTag("SonarCamera").GetComponent<Camera>();

        turnOnOffRadar = GetComponentInChildren<Toggle>();
        turnOnOffRadar.onValueChanged.AddListener(delegate { StartCoroutine(RadarOnOff(turnOnOffRadar.isOn)); });

        angles = transform.Find("Angles").GetComponentsInChildren<Toggle>();
        for (int i = 0; i < angles.Length; i++){
            int j = i;
            angles[i].onValueChanged.AddListener(delegate{
                ChangeSonarAngle(angles[j].isOn, j, angleValues[j]); });
        }
        actualAngle = 90;
        angles[1].isOn = true;

        radius = transform.Find("Radius").GetComponentsInChildren<Toggle>();
        for (int i = 0; i < radius.Length; i++){
            int j = i;
            radius[i].onValueChanged.AddListener(delegate{
                ChangeSonarRadius(radius[j].isOn, j,
                radiusValues[j], cameraValues[j]);
            });
        }
        actualRadius = 50;
        radius[0].isOn = true;
    }
    private IEnumerator RadarOnOff(bool turn) 
    {
        fieldOfView.gameObject.SetActive(turn);

        yield return new WaitForSeconds(0.01f);

        sonarCamera.gameObject.SetActive(turn);
    }
    private void ChangeSonarAngle(bool change,int index,int angleValue) 
    {
        if(actualAngle == angleValue) { 
            angles[index].isOn = true; 
        }

        else if(change) {
            fieldOfView.viewAngle = actualAngle = angleValue;

            for (int i = 0; i < angles.Length; i++){
                if (i != index) { angles[i].isOn = false; }
            }
        }
    }
    private void ChangeSonarRadius(bool change, int index, int radiusValue,float cameraValue) 
    {
        if (actualRadius == radiusValue) {
            radius[index].isOn = true;
        }

        else if (change)
        {
            fieldOfView.viewRadius = actualRadius = radiusValue;
            sonarCamera.orthographicSize = cameraValue;

            for (int i = 0; i < radius.Length; i++){
                if (i != index) { radius[i].isOn = false; }
            }
        }
    }
}
