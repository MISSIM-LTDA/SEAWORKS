using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureLine : MonoBehaviour
{
    public LineRenderer Line1;
    public Transform Start1;
    public Transform Stop1;
    public float largura;

    public float Distance;

    // Use this for initialization
    void Start()
    {
        Line1 = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 A = Start1.position;
        Vector3 B = Stop1.position;

        Line1.startColor = Color.black;
        Line1.endColor = Color.black;

        Line1.startWidth = largura;
        Line1.endWidth = largura;
        Line1.positionCount = 2;
        Line1.useWorldSpace = true;

        //For drawing line in the world space, provide the x,y,z values
        Line1.SetPosition(0, A); //x,y and z position of the starting point of the line
        Line1.SetPosition(1, B); //x,y and z position of the end point of the line

        Distance = Vector3.Distance(A, B);

        Distance = Distance * 0.15f;


    }

}