using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class EditObjectChilds : MonoBehaviour
{

    GameObject render;
    GameObject bakedRender;
    GameObject collidersObject;

    [SerializeField] bool OriginalRender;
    [SerializeField] bool BakedRender;

    public void Initialize()
    {
        render = gameObject.transform.GetChild(0).gameObject;
        bakedRender = gameObject.transform.GetChild(1).gameObject;
        collidersObject = gameObject.transform.GetChild(2).gameObject;
    }

    public void TurOnOffColliders()
    {
        Initialize();

        try
        {
            if (collidersObject)
                collidersObject.SetActive(!collidersObject.activeSelf);
        }

        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void TurOnOffBaked()
    {
        Initialize();

        try
        {
            if (render) 
            {
                render.SetActive(!render.activeSelf);
                OriginalRender = render.activeSelf;
            }
              

            if (bakedRender) 
            {
                bakedRender.SetActive(!bakedRender.activeSelf);
                BakedRender = bakedRender.activeSelf;
            }
        }

        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
