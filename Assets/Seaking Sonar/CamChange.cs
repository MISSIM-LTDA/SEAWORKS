using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamChange : MonoBehaviour {
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	public void Cam1 () {

        Camera1.SetActive(true);
        Camera2.SetActive(false);
        Camera3.SetActive(false);

    }

    public void Cam2()
    {

        Camera1.SetActive(false);
        Camera2.SetActive(true);
        Camera3.SetActive(false);

    }

    public void Cam3()
    {

        Camera1.SetActive(false);
        Camera2.SetActive(false);
        Camera3.SetActive(true);

    }
}
