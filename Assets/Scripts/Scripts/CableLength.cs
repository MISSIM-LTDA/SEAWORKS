using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class CableLength : MonoBehaviour {

    public float Comprimento;

    ObiRope rope;

    // Use this for initialization
    void Start () {
        rope = GetComponent<ObiRope>();
	}
	
	// Update is called once per frame
	void Update () {
        Comprimento = rope.CalculateLength();

        Comprimento = Comprimento * 0.15f;
    }
}
