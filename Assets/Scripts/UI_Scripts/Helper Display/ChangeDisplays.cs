using UnityEngine;
using UnityEngine.UI;


public class ChangeDisplays : MonoBehaviour
{
    private void Start()
    {
        Button changeDisplay = GetComponent<Button>();
        changeDisplay.onClick.AddListener(ChangeDisplay);
    }
    private void ChangeDisplay() 
    {
        foreach (Canvas c in GameObject.FindObjectsOfType<Canvas>())
        {
            if (c.targetDisplay == 0) { c.targetDisplay = 1; }
            else { c.targetDisplay = 0; }
        }
    }
}
