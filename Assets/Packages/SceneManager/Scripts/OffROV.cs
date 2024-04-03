using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffROV : MonoBehaviour {
    public GameObject ROV;

	// Use this for initialization
	void Start () {
        ROV.SetActive(false);
	}
	
	// Update is called once per frame
	public void ROVon () {
        ROV.SetActive(true);
    }

    public void ROVoff()
    {
        ROV.SetActive(false);
    }
}
