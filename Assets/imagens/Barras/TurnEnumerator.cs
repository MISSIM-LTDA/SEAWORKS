using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnEnumerator : MonoBehaviour
{
    public Text text;
    public GameObject ROV;
    public float ROVAngle;
    public float Turn;
    public bool controlL = true;
    bool controlR = true;
    public float joyInput;
    public float Turns ;
    public string TurnText;
    public float direction;
    float OffSetAngle;


    void Start()
    {
        OffSetAngle = ROV.transform.localEulerAngles.y;
    }
    // Update is called once per frame
    void Update()
    {
        joyInput = Input.GetAxis("DroneRotate");
        
        ROVAngle = ROV.transform.localEulerAngles.y - OffSetAngle;
        
        //set the direction of the ROV based on the input
        if(joyInput > 0) { direction = 1; }
        else if (joyInput < 0) { direction = -1; }

        //Count 1 every time the ROV do a 360 to the right
        if (ROVAngle > 179.0f && direction > 0 && controlR)
        {
            Turns++;
            controlR = false;
        }
        //Count 1 every time the ROV do a 360 to the left        
        else if (ROVAngle > 179.7f && direction < 0 && controlL)
        {
            controlL = false;
            Turns--;

        }

        //Count 1 every time the ROV do a 360 to the right and when the joystick stop moving while it goes past the 360 mark
        if (ROVAngle / 360 + Turns > 0.98f + Turns && controlR && direction > 0)
        {
            Turns++;
            controlR = false;

        }

        //Count 1 every time the ROV do a 360 to the left and when the joystick stop moving while it goes past the 360 mark
        if (ROVAngle / 360 + Turns > 0.987f + Turns && controlL && direction < 0)
        {
            Turns--;
            controlL = false;

        }

        //after the 360 it resets the control variables 
        if ((ROVAngle+OffSetAngle)/360 < 0.987f  && !controlL)
        {
            controlL = true;
        }
        else if (ROVAngle < 1 && !controlR)
        {
            controlR = true;
        }


        Turn = ROVAngle / 360 + Turns;
        TurnText = "TRN: " + Turn.ToString("f1");
        text.text = TurnText.Replace(",",".");
    }

}
