using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeaveOsc : MonoBehaviour
{
    
    public Rigidbody rb;
    public float ForcesUp;
    public float ForcesDown;
    float MaxT = 10;  
    public float time;
    public float timeUp;
    public float timeDown;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        time += Time.deltaTime;

        if(time<timeUp)
        {
            rb.AddForce(transform.up * ForcesUp);

        }

        if(time>timeUp && time<timeDown)
        {

            rb.AddForce(-transform.up * ForcesDown);
            
        }

        if (time > timeDown)
        {
            time = 0;
        }

            Debug.Log((time));
    }

     

}
