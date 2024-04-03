using UnityEngine;
using System.Collections;

public class ViperFish_Thruster : MonoBehaviour
{

    public Transform blade1;
    public Transform blade2;
    public Transform blade3;
    public Transform blade4;
    public Transform blade5;
    public Transform blade6;
    public Transform blade7;
    public float rotRate = 1000.0f;
    public float rotRate2 = 1000.0f;
  


    void Update()
    {
        
            blade1.Rotate(Vector3.back * Time.deltaTime * rotRate);
            blade2.Rotate(Vector3.back * Time.deltaTime * rotRate);
            blade3.Rotate(Vector3.back * Time.deltaTime * rotRate);
            blade4.Rotate(Vector3.back * Time.deltaTime * rotRate);
            blade5.Rotate(Vector3.back * Time.deltaTime * rotRate);
            blade6.Rotate(Vector3.back * Time.deltaTime * rotRate);
            blade7.Rotate(Vector3.back * Time.deltaTime * rotRate);
        
    }

}
