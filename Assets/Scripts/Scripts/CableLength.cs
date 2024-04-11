using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;
using UnityEngine.UI;

public class CableLength : MonoBehaviour {

    public float Comprimento;

    ObiRope rope;
    public Text tmsLenght;

    // Use this for initialization
    void Start () {
        rope = GetComponent<ObiRope>();
	}
	
	// Update is called once per frame
	void Update () {
        Comprimento = rope.CalculateLength();

        Comprimento = Comprimento * 0.15f;
        tmsLenght.text = "TMS: "+(Comprimento).ToString("0") +"m";
    }
}
