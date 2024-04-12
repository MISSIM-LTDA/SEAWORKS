using UnityEngine;
using UnityEngine.UI;
public class Overlay : MonoBehaviour
{
    [SerializeField] private RawImage compassBarImage;
    [SerializeField] private RawImage leftPitchBarImage;
    [SerializeField] private RawImage rightPitchBarImage;
    [SerializeField] private RawImage rollBarImage;

    private Transform rov;

    private float offsetAngleY;
    private void Start()
    {
        rov = GameObject.FindGameObjectWithTag("XLX").transform;

        RawImage[] barImages = transform.GetComponentsInChildren<RawImage>();
        compassBarImage = barImages[0];

        leftPitchBarImage = barImages[1];
        rightPitchBarImage = barImages[2];

        rollBarImage = barImages[3];


        offsetAngleY = rov.localEulerAngles.y;
    }
    private void Update()
    {
        if (compassBarImage) { Compass(); }
        if (rollBarImage) { Roll(); }
        if (leftPitchBarImage && rightPitchBarImage) { Pitch(); }
    }
    private void Compass() 
    {
        float compassPositon = Mathf.Clamp(rov.localEulerAngles.y - offsetAngleY, -offsetAngleY, offsetAngleY);
        compassBarImage.uvRect = new Rect(compassPositon / 360.0f, 0.0f, 1.0f, 1.0f);
    }
    private void Roll() 
    {
        rollBarImage.rectTransform.rotation = Quaternion.Euler(0.0f, 0.0f, rov.localEulerAngles.z);
    }
    private void Pitch() 
    {
        float pitchPosition = Mathf.Clamp(rov.localEulerAngles.x, 0.0f, 360.0f);
        rightPitchBarImage.uvRect = new Rect(0, pitchPosition / 360.0f, 1, 1);
        leftPitchBarImage.uvRect = new Rect(0, pitchPosition / 360.0f, 1, 1);
    }
}
