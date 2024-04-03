using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrusterSpinCCW : MonoBehaviour {

    public Transform blade1;
    public Transform blade2;
    public Transform blade3;
    public Transform blade4;
    public Transform blade5;
    public Transform blade6;
    public Transform blade7;
    public float rotRate;
    public float rotRate2;



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        blade1.Rotate(Vector3.down * Time.deltaTime * rotRate);
        blade2.Rotate(Vector3.down * Time.deltaTime * rotRate);
        blade3.Rotate(Vector3.down * Time.deltaTime * rotRate);
        blade4.Rotate(Vector3.down * Time.deltaTime * rotRate);
        blade5.Rotate(Vector3.down * Time.deltaTime * rotRate2);
        blade6.Rotate(Vector3.down * Time.deltaTime * rotRate2);
        blade7.Rotate(Vector3.down * Time.deltaTime * rotRate2);
    }
}
