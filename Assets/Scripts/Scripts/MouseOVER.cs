using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOVER : MonoBehaviour {
    public bool MouseOver = false;
    public GameObject MeshHighLight;
    public GameObject MeshHighLight2;
    public GameObject MeshHighLight3;
    public GameObject MeshHighLight4;
    public GameObject MeshHighLight5;
    public GameObject MeshHighLight6;
    public GameObject MeshHighLight7;

    private void Start()
    {
        MeshHighLight.GetComponent<Outline>().enabled = false;
        MeshHighLight2.GetComponent<Outline>().enabled = false;
        MeshHighLight3.GetComponent<Outline>().enabled = false;
        MeshHighLight4.GetComponent<Outline>().enabled = false;
        MeshHighLight5.GetComponent<Outline>().enabled = false;
        MeshHighLight6.GetComponent<Outline>().enabled = false;
        MeshHighLight7.GetComponent<Outline>().enabled = false;
    }
    // Use this for initialization
    void OnMouseEnter () {
        MouseOver = true;
        Debug.Log("Entrou" + name);
        MeshHighLight.GetComponent<Outline>().enabled = true;
        MeshHighLight2.GetComponent<Outline>().enabled = true;
        MeshHighLight3.GetComponent<Outline>().enabled = true;
        MeshHighLight4.GetComponent<Outline>().enabled = true;
        MeshHighLight5.GetComponent<Outline>().enabled = true;
        MeshHighLight6.GetComponent<Outline>().enabled = true;
        MeshHighLight7.GetComponent<Outline>().enabled = true;
    }

    // Update is called once per frame
    void OnMouseExit()
    {
        MouseOver = false;
        Debug.Log("Saiu" + name);
        MeshHighLight.GetComponent<Outline>().enabled = false;
        MeshHighLight2.GetComponent<Outline>().enabled = false;
        MeshHighLight3.GetComponent<Outline>().enabled = false;
        MeshHighLight4.GetComponent<Outline>().enabled = false;
        MeshHighLight5.GetComponent<Outline>().enabled = false;
        MeshHighLight6.GetComponent<Outline>().enabled = false;
        MeshHighLight7.GetComponent<Outline>().enabled = false;
    }
}
