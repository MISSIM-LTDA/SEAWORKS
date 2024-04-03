using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followtether : MonoBehaviour {

   
    
    public Transform houseTransform;
    float mSpeed = 10.0f;
    // Use this for initialization
   
	
	// Update is called once per frame
	void LateUpdate () {

        transform.position = houseTransform.position;
       
    }
}
