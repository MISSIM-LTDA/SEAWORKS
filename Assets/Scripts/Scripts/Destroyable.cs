using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour {

    public GameObject Info;
    public GameObject Info2;

    public void RemoveMe()
    {
        Debug.Log("clickou em " + name);
        Info.SetActive(true);
        //Destroy(gameObject);
    }

    public void RightClick1()
    {
        Debug.Log("clickou em " + name);
        Info2.SetActive(true);
        //Destroy(gameObject);
    }


    public void FecharPause()
    {

        Info.SetActive(false);
    }


}
