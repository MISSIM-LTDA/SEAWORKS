using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PitchControl : MonoBehaviour
{
    public RawImage image;
    public RawImage image2;
    public Transform objetoref;
    public int x;
    public float AngleROV;
    public Vector3 NewAngle;
    
    void Update()
    {
        NewAngle = new Vector3 (AngleROV, objetoref.localEulerAngles.y, objetoref.localEulerAngles.z);
        objetoref.transform.rotation = Quaternion.Euler(NewAngle);
        image.uvRect = new Rect(objetoref.localEulerAngles.x / x, 0, 1, 1);
        image2.uvRect = new Rect(objetoref.localEulerAngles.x / -x, 0, 1, 1);
    }
}
