using UnityEngine;
using UnityEngine.UI;

public class RovInfo : MonoBehaviour
{
    private Transform rov;

    private float offsetAngleY;
   
    private Text dptText;
    private Text altText;
    private Text hdgText;
    private Text tmsText;

    private Text turnsText;
    private float lastAngle;
    private int turnsCounter;
    private float totalRotation;
    private void Start()
    {
        rov = GameObject.Find("FORUM XLX").transform;

        if (rov){
            offsetAngleY = rov.transform.localEulerAngles.y;
        }

        dptText = transform.GetChild(0).GetComponent<Text>();
        altText = transform.GetChild(1).GetComponent<Text>();
        hdgText = transform.GetChild(2).GetComponent<Text>();
        tmsText = transform.GetChild(3).GetComponent<Text>();
        turnsText = transform.GetChild(4).GetComponent<Text>();
    }
    private void Update()
    {
        if (rov) {
            Heading();
            Turn();
        }
    }
    private void Heading() 
    {
        float rovAngleY = rov.transform.localEulerAngles.y - offsetAngleY;

        if (rovAngleY < 0){
            rovAngleY = rovAngleY + offsetAngleY * 2;
        }

        hdgText.text = "HDG: " + rovAngleY.ToString("f0") + "°";
    }
    private void Turn()
    {
        float rovAngleY = rov.localEulerAngles.y - offsetAngleY;
        float difference = rovAngleY - lastAngle;

        if (difference > 180)  { difference -= 360; }
        if (difference < -180) { difference += 360; }

        totalRotation += difference;

        if (Mathf.Abs(totalRotation) >= 360){
            turnsCounter += (int)Mathf.Sign(totalRotation);
            totalRotation %= 360;
        }

        lastAngle = rovAngleY;

        float turn = (totalRotation/360) + turnsCounter;
        turnsText.text = ("TRN: " + turn.ToString("f1")).Replace(",", ".");
    }
}
