using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderOrBaked : MonoBehaviour
{
    [SerializeField]
    private GameObject render;
    [SerializeField]
    private GameObject bakedRender;
    [SerializeField]
    private GameObject collidersObject;

    [SerializeField]
    private bool baked;
    [SerializeField]
    private bool colliders;

    bool controlBaked;
    bool controlColliders;
    void Start()
    {
        render = this.gameObject.transform.GetChild(0).gameObject;
        bakedRender = this.gameObject.transform.GetChild(1).gameObject;
        collidersObject = this.gameObject.transform.GetChild(2).gameObject;
    }

    private void Update()
    {
        if(controlBaked != baked) 
        {
            TurOnOffBaked();
            controlBaked = baked;
        }

        if (controlColliders != colliders)
        {
            TurOnOffColliders();
            controlColliders = colliders;
        }

    }

    private void TurOnOffColliders()
    {
        collidersObject.SetActive(colliders);

    }

    private void TurOnOffBaked()
    {
        render.SetActive(!render.activeSelf);
        bakedRender.SetActive(!bakedRender.activeSelf);
    }
}
