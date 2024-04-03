using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : MonoBehaviour {

    public Color startColor;
    public Color mouseOverColor;
    bool mouseOver = false;

	// Use this for initialization
	void OnMouseEnter () {


        mouseOver = true;
        GetComponent<Renderer>().material.SetColor("_color", mouseOverColor);

		
	}

    void OnMouseExit()
    {


        mouseOver = false;
        GetComponent<Renderer>().material.SetColor("_color", startColor);


    }


}
