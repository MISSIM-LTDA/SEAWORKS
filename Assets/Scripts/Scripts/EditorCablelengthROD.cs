using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;


[ExecuteInEditMode]

public class EditorCablelengthROD : MonoBehaviour
{

    public float Comprimento;

    ObiRod rope;

    // Use this for initialization
    void Start()
    {
        rope = GetComponent<ObiRod>();
    }

    // Update is called once per frame
    void Update()
    {
        Comprimento = rope.CalculateLength();

        Comprimento = Comprimento * 0.15f;
    }
}
