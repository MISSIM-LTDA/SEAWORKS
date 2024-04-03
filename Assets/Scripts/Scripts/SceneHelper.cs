using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneHelper : MonoBehaviour
{
    public GameObject obj;
    public Button hideUnhideButton;

    private void Start()
    {
        hideUnhideButton.onClick.AddListener(HideUnHideGameObject);
    }
    public void HideUnHideGameObject() 
    {
        if (obj.activeSelf) { hideUnhideButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Show ANM"; }
        else { hideUnhideButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Hide ANM"; }

        obj.SetActive(!obj.activeSelf); 
    }
}
