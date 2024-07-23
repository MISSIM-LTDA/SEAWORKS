using Obi;
using UnityEngine;
using UnityEngine.UI;

public class RovInfo : MonoBehaviour
{
    private Transform rov;
    private float offsetAngleY;
   
    private Text dptText;

    [Header("Choose the surface height for the simulation")]
    [SerializeField] float surfaceHeight;

    private Text altText;
    private Transform center;

    private Text hdgText;
    private Text tmsText;
    private ObiRope tmsRope;

    private Text turnsText;
    private float lastAngle;
    private int turnsCounter;
    private float totalRotation;

    private Text fpsText;
    private float deltaTime;
    private void Start()
    {
        rov = GameObject.Find("FORUM XLX").transform;

        if (rov){
            offsetAngleY = rov.transform.localEulerAngles.y;

            center = rov.Find("Center");

            tmsRope = rov.parent.GetComponentInChildren<ObiRope>();
        }

        dptText = transform.GetChild(0).GetComponent<Text>();
        altText = transform.GetChild(1).GetComponent<Text>();
        hdgText = transform.GetChild(2).GetComponent<Text>();
        tmsText = transform.GetChild(3).GetComponent<Text>();
        turnsText = transform.GetChild(4).GetComponent<Text>();
        fpsText = transform.GetChild(5).GetComponent<Text>();
    }
    private void Update()
    {
        if (rov) {
            Depth();
            if (center) { Altitude(); }
            if(tmsRope) { TMS(); }
            Heading();
            Turn();
            FPS();
        }
    }
    private void Depth() 
    {
        float depth = (surfaceHeight - rov.position.y);
        dptText.text = "DPT: " + (depth).ToString("f0") + "m";
    }
    private void Altitude() 
    {
        Vector3 halfExtents = new Vector3(6.5f, 500.0f, 12.5f);
        Collider[] hitColliders = Physics.OverlapBox(center.position - new Vector3(0.0f, 501.0f, 0.0f), halfExtents);

        float altDistance = Mathf.Infinity;
        float distance = 0.0f;
        foreach (Collider collider in hitColliders) {
            if (collider.GetComponent<TerrainCollider>()) {
                RaycastHit hit;
                int layerMask = 1 << 3;

                Physics.Raycast(center.position, center.TransformDirection(Vector3.down),
                    out hit, Mathf.Infinity, layerMask);

                distance = hit.distance;
            }

            else {
                distance = Mathf.Abs(collider.ClosestPoint(center.position).y - center.position.y);
            }

            if (altDistance > distance) { altDistance = distance; }
        }
        
        altText.text = "ALT: " + (altDistance).ToString("f0") + "m";
    }
    private void TMS() 
    {
        float lenght = tmsRope.CalculateLength() * 0.15f;
        tmsText.text = "TMS: " + (lenght).ToString("0") + "m";
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
    private void FPS() 
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }
}
