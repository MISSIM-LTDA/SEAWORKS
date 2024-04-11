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

    // Update is called once per frame
    void Update()
    {
        if (start) 
        {
            foreach (MeshFilter mr in gameObject.GetComponentsInChildren<MeshFilter>())
            {
                if(mr.sharedMesh == null)
                   DestroyImmediate(mr.gameObject);
            }

            start = false;
        }
    }
}
