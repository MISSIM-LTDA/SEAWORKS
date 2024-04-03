using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UDindic : MonoBehaviour {


    public float Read;
    public Text UpDown;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Read = Input.GetAxis("DroneUpDown");
        Read = Read * 100;
        UpDown.text = Read.ToString(); ;

    }
}
