using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeDisplays : MonoBehaviour
{

    public Canvas canvas1;
    public Canvas canvas2;
    public Canvas canvasBag;

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

    public void ChangeDisplay()
    {
        canvasBag.targetDisplay = canvas1.targetDisplay;
        canvas1.targetDisplay = canvas2.targetDisplay;
        canvas2.targetDisplay = canvasBag.targetDisplay;
        //Display.displays[1].SetParams(2560, 1080, 0, 0);
    }
}
