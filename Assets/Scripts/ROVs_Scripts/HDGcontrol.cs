using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HDGcontrol : MonoBehaviour
{
    public Text text;
    public Transform rov;
    public float Angles;
    public float OffSetAngle;
    public string TurnText;

    void Start()
    {
        OffSetAngle = rov.transform.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        Angles = rov.transform.localEulerAngles.y - OffSetAngle;
        if (Angles < 0)
        {
            Angles = Angles + OffSetAngle*2;
        }
        TurnText = "HDG: " + Angles.ToString("f0") + "°";
        text.text = TurnText;
    }
}
