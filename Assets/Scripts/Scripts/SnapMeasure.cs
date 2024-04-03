using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapMeasure : MonoBehaviour {


    public GameObject objectPrefab;

    private GameObject currentPrefab;

    Ray ray;
    RaycastHit hit;

    // Update is called once per frame
    void Update()
    {
        if (currentPrefab != null)
        {
            CurrentPrefabMove();
        }
    }

    public void PlacePrefab()
    {
        if (currentPrefab == null)
        {
            currentPrefab = Instantiate(objectPrefab);
        }
        else
        {
            Destroy(currentPrefab);
        }
    }

    private void CurrentPrefabMove()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            currentPrefab.transform.position = hit.point;
            currentPrefab.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }
}




