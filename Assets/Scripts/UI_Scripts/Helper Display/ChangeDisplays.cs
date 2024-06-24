using UnityEngine;
using UnityEngine.UI;


public class ChangeDisplays : MonoBehaviour
{
    private Canvas[] canvas;
    private void Start()
    {
        canvas = FindObjectsOfType<Canvas>();
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKeyUp(KeyCode.D)) {
            ChangeDisplay();
        }   
    }
    private void ChangeDisplay() 
    {
        foreach (Canvas c in canvas)
        {
            if (c.targetDisplay == 0) { c.targetDisplay = 1; }
            else { c.targetDisplay = 0; }
        }
    }
}
