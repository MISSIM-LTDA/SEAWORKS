using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBeam : MonoBehaviour
{
    public GameObject ObjDesaparecer;
    public GameObject ObjDesaparecer2;
    public GameObject ObjDesaparecer3;
    public GameObject ObjDesaparecer4;
    public GameObject ObjDesaparecer5;
    bool Clicked = true;

    // Use this for initialization
    public void clicked1()
    {
        ObjDesaparecer.SetActive(true);


    }

    public void clicked2()
    {
        ObjDesaparecer.SetActive(false);


    }

    public void clicked3()
    {
        ObjDesaparecer2.SetActive(true);


    }

    public void clicked4()
    {
        ObjDesaparecer2.SetActive(false);


    }

    public void clicked5()
    {
        ObjDesaparecer3.SetActive(true);


    }

    public void clicked6()
    {
        ObjDesaparecer3.SetActive(false);


    }

    public void clicked7()
    {
        ObjDesaparecer4.SetActive(true);


    }

    public void clicked8()
    {
        ObjDesaparecer5.SetActive(true);


    }
}
