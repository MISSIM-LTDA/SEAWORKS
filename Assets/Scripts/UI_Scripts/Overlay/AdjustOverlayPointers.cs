using UnityEngine;
using UnityEngine.UI;
public class AdjustOverlayPointers : MonoBehaviour
{
    private RawImage[] movingBars;

    private RawImage compassImage;
    private RawImage centerBarImage;

    private Transform rov;

    private float offsetAngleY;
    private void Start()
    {
        movingBars = GetComponentsInChildren<RawImage>();

        compassImage = movingBars[0];
        centerBarImage = movingBars[1];

        rov = GameObject.FindGameObjectWithTag("XLX").transform;

        offsetAngleY = rov.localEulerAngles.y;
    }
    private void Update()
    {
        float exactPositon = Mathf.Clamp(rov.localEulerAngles.y - offsetAngleY, -offsetAngleY, offsetAngleY);
        compassImage.uvRect = new Rect(exactPositon / 360.0f, 0, 1, 1);

        centerBarImage.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
