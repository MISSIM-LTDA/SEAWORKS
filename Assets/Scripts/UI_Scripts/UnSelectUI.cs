using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnSelectUI: MonoBehaviour
{
    private EventSystem eventSystem;

    private void Start() 
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }
    private void Update() 
    {
        GameObject currentSelected = eventSystem.currentSelectedGameObject;

        if (currentSelected 
            && currentSelected.GetComponent<Slider>()
            && Input.GetMouseButtonUp(0))
        {
            eventSystem.SetSelectedGameObject(null);
        }
    }
}
