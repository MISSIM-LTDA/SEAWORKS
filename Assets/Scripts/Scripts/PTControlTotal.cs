using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PTControlTotal : MonoBehaviour
{

    public float limitP;
    public float limitT;

    public float valueToLerpPan;
    public float lerpDuration;
    public float valueToLerpTilt;

    public float startValue = 0f;
    public float endValue;

    public GameObject Pan;
    public GameObject Tilt;

    public float shakeDuration;
    public float shakeAmount;

    public bool endMovePan = false;
    public bool endMoveTilt = false;


    public bool canShakeP = false;
    public bool canShakeT = false;

    public float _shakeTimer;

    public float timer;
    //-------------------------------
    public float originalrosPan;
    public float originalrosTilt;
    //---------------------------------


    bool control;

    bool lerpingPan;
    bool lerping;
    bool lerpingTilt;

    //-----------------------------------------------------
    public bool Moving = false;
    //-----------------------------------------------------

    private void Start()
    {
        originalrosPan = Pan.transform.localEulerAngles.z;
        originalrosTilt = Tilt.transform.localEulerAngles.x;
    }

    void Update()
    {
        if (Input.GetAxis("T4S") <= -0.8)
        {
            StopCoroutine(Lerp(Moving));
            canShakeP = false;

            if (originalrosPan <= limitP)
            {
                timer += Time.deltaTime;

                if (!lerpingPan) 
                {
                    StartCoroutine(Lerp(Moving));
                    StartCoroutine(CheckMovimentStopPan());
                }
                originalrosPan += valueToLerpPan;
                Pan.transform.Rotate(0, 0, valueToLerpPan);
            }
        }

        else if (Input.GetAxis("T4S") >= 0.8)
        {
            StopCoroutine(Lerp(Moving));
            
            canShakeP = false;

            if (originalrosPan > -limitP)
            {
                timer += Time.deltaTime;
             
                if (!lerpingPan) 
                {
                    StartCoroutine(Lerp(Moving));
                    StartCoroutine(CheckMovimentStopPan());
                }
                Pan.transform.Rotate(0, 0, -valueToLerpPan);
                originalrosPan = originalrosPan - valueToLerpPan;
            }
        }

        else
        {
            lerpingPan = false;
            valueToLerpPan = 0;
            
        }

        // TILT----------


        if (Input.GetAxis("T4UD") >= 0.8)
        {
            StopCoroutine(Lerp(Moving));

            canShakeT = false;

            if (originalrosTilt < limitT)
            {
                timer += Time.deltaTime;
                if (!lerpingTilt)
                {
                    StartCoroutine(Lerp(Moving));
                    StartCoroutine(CheckMovimentStopTilt());
                }

                Tilt.transform.Rotate(valueToLerpTilt, 0, 0);
                originalrosTilt = originalrosTilt + valueToLerpTilt;
            }
            
        }

        else if (Input.GetAxis("T4UD") <= -0.8)
        {
            StopCoroutine(Lerp(Moving));

            canShakeT = false;
            if (originalrosTilt > -limitT)
            {
                timer += Time.deltaTime;
                if (!lerpingTilt)
                {
                    StartCoroutine(Lerp(Moving));
                    StartCoroutine(CheckMovimentStopTilt());
                }
                Tilt.transform.Rotate(-valueToLerpTilt, 0, 0);
                originalrosTilt = originalrosTilt - valueToLerpTilt;
            }
        }

        else
        {
            lerpingTilt = false;
            valueToLerpTilt = 0;

        }


        if (canShakeP)
        {
            StartCameraShakeEffectPan();
        }

        if (canShakeT)
        {
            StartCameraShakeEffectTilt();
        }


        if ((canShakeT == true) && (canShakeP == true))
        {
            canShakeT = false;
            canShakeP = false;
            endMovePan = false;
            endMoveTilt = false;
        }

    }

    public void ShakeCameraTilt()
    {
        canShakeT = true;
        _shakeTimer = shakeDuration;
       // originalrosTilt = Tilt.transform.rotation;

    }

    public void ShakeCameraPan()
    {
        canShakeP = true;
        _shakeTimer = shakeDuration;
       // originalrosPan = Pan.transform.rotation;

    }

    public void StartCameraShakeEffectPan()
    {
        if (timer > 0.7){
            if (_shakeTimer > 0)
            {
                endMovePan = false;

                shakeAmount = shakeAmount * -1;

                Pan.transform.Rotate(0.0f, 0.0f, shakeAmount, Space.World);
                _shakeTimer -= Time.deltaTime;
            } else
            {
                canShakeP = false;
                endMovePan = false;
                timer = 0;
            }
        }
        
    }

    public void StartCameraShakeEffectTilt()
    {
        if (timer > 0.7)
        {
            if (_shakeTimer > 0)
            {
                endMoveTilt = false;
                shakeAmount = shakeAmount * -1;

                Tilt.transform.Rotate(shakeAmount, 0.0f, 0.0f, Space.World);
                _shakeTimer -= Time.deltaTime;
            }
            else
            {
                canShakeT = false;
                endMoveTilt = false;
                timer = 0;
            }
        }
    }


    public IEnumerator CheckMovimentStopPan()
    {
        yield return new WaitUntil(() => Input.GetAxis("T4S") == 0);
        control = false;
        canShakeP = true;
    }

    public IEnumerator CheckMovimentStopTilt()
    {
        Debug.Log("3");
        yield return new WaitUntil(() => Input.GetAxis("T4UD") == 0);
        control = false;
        canShakeT = true;
    }


    

    IEnumerator Lerp(bool  foward)
    {
        control = true;
        lerpingTilt = true;
        lerpingPan = true;
        lerping = true;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration && control)
        {

            if (foward)
            {
                valueToLerpTilt = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
                valueToLerpPan = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        lerping = false;

        valueToLerpTilt = endValue;
        valueToLerpPan = endValue;
    }

}