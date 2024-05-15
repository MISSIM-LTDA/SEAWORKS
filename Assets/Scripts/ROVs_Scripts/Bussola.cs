using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bussola : MonoBehaviour
{
    public RawImage image;
    public Transform objetoref;
    public int x;
    float OffsetAngle;

    private void Start()
    {
        OffsetAngle = objetoref.localEulerAngles.y;
    }
    private void Update()
    {
        float exactPositon = Mathf.Clamp(objetoref.localEulerAngles.y - OffsetAngle, -OffsetAngle, OffsetAngle);
        image.uvRect = new Rect(exactPositon/360.0f, 0, 1, 1);
    }
}
