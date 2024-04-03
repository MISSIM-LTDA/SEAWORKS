using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistaomove : MonoBehaviour {
    public GameObject target;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 targetPosition = new Vector3(transform.position.x, target.transform.position.y, target.transform.position.z);

        transform.LookAt(targetPosition);

	}
}
