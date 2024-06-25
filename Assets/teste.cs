using Obi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class teste : MonoBehaviour
{
    public bool start;

    private void Update()
    {
        if (start)
        {
            foreach (ObiCollider t in transform.GetComponentsInChildren<ObiCollider>()) 
            {
                DestroyImmediate(t);
            }

            start = false;
        }
    }
}
