using UnityEngine;

public class ActiveSecondDisplay : MonoBehaviour
{
    private void Start()
    {
        if (Display.displays.Length > 1){
            Display.displays[1].Activate();
        }

        else {
            gameObject.SetActive(false);
        }
    }
}