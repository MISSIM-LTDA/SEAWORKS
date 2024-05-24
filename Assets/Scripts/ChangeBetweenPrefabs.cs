using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBetweenPrefabs : MonoBehaviour
{
    public GameObject[] objectsToHideAndUnhide;

    public GameObject[] physicsObjects;
    public GameObject[] kinematicObjects;

    public int start;
    public int end;

    public Button shiftButton;

    bool physicsEnable;
    public bool shiftPrefabs;
    public bool hideUnHide;

    public string buttonText1;
    public string buttonText2;
    void Start()
    {
        physicsEnable = true;

        if(hideUnHide) {shiftButton.onClick.AddListener(delegate { HideUnHideGameObject(objectsToHideAndUnhide, buttonText1, buttonText2); });}
    
        else if (shiftPrefabs) { shiftButton.onClick.AddListener(delegate { ShiftPrefabs(start, end); }); }
    }

    public void HideUnHideGameObject(GameObject[] hideUnhideObj,string text1,string text2)
    {
        if (shiftButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text == text2) { shiftButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = text1; }
        else { shiftButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = text2; }

        foreach(GameObject obj in hideUnhideObj) { obj.SetActive(!obj.activeSelf); }
    }
    public void ShiftPrefabs(int start,int end) 
    {
        if(physicsEnable) 
        {
            for (int i = start; i < end; i++)
            {
                physicsObjects[i].SetActive(false);

                kinematicObjects[i].transform.position = physicsObjects[i].transform.position;
                kinematicObjects[i].transform.rotation = physicsObjects[i].transform.rotation;

                kinematicObjects[i].SetActive(true);
            }

            physicsEnable = false;
        }

        else if (!physicsEnable)
        {
            for (int i = start; i < end; i++)
            {
                kinematicObjects[i].SetActive(false);

                physicsObjects[i].transform.position = kinematicObjects[i].transform.position;
                physicsObjects[i].transform.rotation = kinematicObjects[i].transform.rotation;

                physicsObjects[i].SetActive(true);
            }

            physicsEnable = true;
        }
    }
}
