using System.Collections;
using UnityEngine;

public class PanAndTiltControl : MonoBehaviour
{
    #region Pan Variables
    [Header("Pan")]
    [SerializeField] private Transform pan;

    [SerializeField] private float panMaxLimit;
    [SerializeField] private float panMinLimit;

    [SerializeField] private float panRotationSpeed =  0.3f;
    [Space(10)]
    #endregion

    #region Tilt Variables
    [Header("Tilt")]
    [SerializeField] private Transform tilt;

    [SerializeField] private float tiltMaxLimit;
    [SerializeField] private float tiltMinLimit;

    [SerializeField] private float tiltRotationSpeed = 0.3f;
    #endregion

    private void FixedUpdate()
    {
        if (Input.GetAxis("T4S") <= -0.8f){
            if (pan.localEulerAngles.y < panMaxLimit){
                pan.Rotate(0.0f, 0.0f, panRotationSpeed, Space.Self);
            }
        }

        else if (Input.GetAxis("T4S") >= 0.8f){
            if (pan.localEulerAngles.y > panMinLimit){
                pan.Rotate(0.0f, 0.0f, -panRotationSpeed, Space.Self);
            }
        }
        Debug.Log(WrapAngle(tilt.localEulerAngles.x));
        if (Input.GetAxis("T4UD") >= 0.8f){
            if (WrapAngle(tilt.localEulerAngles.x) < tiltMaxLimit){
                tilt.Rotate(tiltRotationSpeed, 0.0f, 0.0f, Space.Self);
            }
        }

        else if (Input.GetAxis("T4UD") <= -0.8f){
            if (WrapAngle(tilt.localEulerAngles.x) > tiltMinLimit){
                tilt.Rotate(-tiltRotationSpeed, 0.0f, 0.0f, Space.Self);
            }
        }
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
}
