using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveScrew : MonoBehaviour {
    public GameObject Screw1;
    int Loop = 0;
    int install = 0;

    public Vector3 targetPosition;
    public Vector3 targetPosition2;
    public float smoothFactor = 2;

    
    // Update is called once per frame
    public void RemoveScrewvoid () {

        if (install == 0)
        {
            Loop = 70;
            install = 1;
        }
        
    }

    public void InstallScrewvoid()
    {

        if (install == 1)
        {
            Loop = -70;
            install = 0;
        }

    }

    private void Update()
    {
        if (Loop > 0)
        {
            //Screw1.transform.Translate(Vector3.right * 2);
            Screw1.transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothFactor);
            Loop = Loop - 1;
        }

        if (Loop < 0)
        {
            //Screw1.transform.Translate(Vector3.left * 2);
            Screw1.transform.position = Vector3.Lerp(transform.position, targetPosition2, Time.deltaTime * smoothFactor);
            Loop = Loop + 1;
        }


        
        

    }



}
