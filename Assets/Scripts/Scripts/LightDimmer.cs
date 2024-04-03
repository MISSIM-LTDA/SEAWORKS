using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDimmer : MonoBehaviour {

    public Light FrontLight;
    public float Intensity = 1;

    public KeyCode LigthUP;
    public KeyCode LigthDOWN;
    public float speed = 0.02f; 


    // Update is called once per frame
    void Update()
    {

        FrontLight.intensity = Intensity;

        if (Input.GetKey(LigthDOWN))
        {
            if(Intensity > 0.05f)
            {
                Intensity = Intensity - speed;
            }
      
        }

        if (Input.GetKey(LigthUP))
        {

            if (Intensity < 5)
            {
                Intensity = Intensity + speed;
            }
        }

    }
}
