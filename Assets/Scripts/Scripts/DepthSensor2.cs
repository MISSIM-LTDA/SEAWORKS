using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DepthSensor2 : MonoBehaviour
{
    public Transform Ob1;
    public Transform Ob2;
    public float depth;
    public Text Profundidade;

    void Update()
    {
        
            //float dist = Vector3.Distance(other.position, transform.position);
            float dist = Ob1.position.y - Ob2.position.y;
            //print("Distance to other: " + dist);
            depth = dist * 0.15f;
            Profundidade.text = depth.ToString() + " m"; 
       
    }
}