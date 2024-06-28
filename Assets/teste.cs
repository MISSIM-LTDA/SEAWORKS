using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class teste : MonoBehaviour
{
    public bool start;
    void Start()
    {
        
    }

    void Update()
    {
        if(start) 
        {
            foreach (ObiCollider collider in GetComponentsInChildren<ObiCollider>()) 
            {
                DestroyImmediate(collider);
            }

            start = false;
        }
    }
}
