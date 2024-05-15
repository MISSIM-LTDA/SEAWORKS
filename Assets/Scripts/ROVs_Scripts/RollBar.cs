using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollBar : MonoBehaviour
{

    public float Angle;
    public RawImage barra;
    public Transform Rov;
    public Vector3 NewAngle;

    // Update is called once per frame
    void Update()
    {
        NewAngle = new Vector3(Rov.localEulerAngles.x, Rov.localEulerAngles.y, Angle);
        Rov.transform.rotation =  Quaternion.Euler(NewAngle);
        barra.rectTransform.rotation = Quaternion.Euler(0, 0, Angle);
    }
}
