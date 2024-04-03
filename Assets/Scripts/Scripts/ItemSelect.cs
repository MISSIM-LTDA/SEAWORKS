using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelect : MonoBehaviour {


    public GameObject Info;
    public bool Clicked = false;
    public GameObject camera1;

    public void FecharPause()
    {

        Info.SetActive(false);
    }

    public void ClickedOff()
    {
        camera1.GetComponent<ItemSelect>().enabled = true;
        Clicked = !Clicked;
    }

    public void ClickedROVPanel()
    {        
        Clicked = !Clicked;
    }

    private void Start()
    {
        camera1 = GameObject.FindGameObjectWithTag("MainCamera");
    }


    public void ClickedOn()
    {
        Clicked = !Clicked;
        camera1.GetComponent<ItemSelect>().enabled = false; 
    }
    


    void Update()
    {
       

            if (Input.GetMouseButtonDown(0))
            {
                Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit rhinfo;
                bool didHit = Physics.Raycast(toMouse, out rhinfo, 500.0f);
                if (didHit)
                {
                    Debug.Log(rhinfo.collider.name + ".." + rhinfo.point);
                    Destroyable destScript = rhinfo.collider.GetComponent<Destroyable>();
                    if (destScript)
                    {
                    if (Clicked==false)
                    {
                        destScript.RemoveMe();
                        Clicked = !Clicked;
                        camera1.GetComponent<ItemSelect>().enabled = false;
                     
                    }                  
                    
                }

                }
                else
                {
                    Debug.Log("clickou em nada");
                }
            }

        if (Input.GetMouseButtonDown(1))
        {
            Ray toMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rhinfo;
            bool didHit = Physics.Raycast(toMouse, out rhinfo, 500.0f);
            if (didHit)
            {
                Debug.Log(rhinfo.collider.name + ".." + rhinfo.point);
                Destroyable destScript = rhinfo.collider.GetComponent<Destroyable>();
                if (destScript)
                {
                    if (Clicked == false)
                    {
                        destScript.RightClick1();
                        Clicked = !Clicked;
                        camera1.GetComponent<ItemSelect>().enabled = false;

                    }

                }

            }
            else
            {
                Debug.Log("clickou em nada");
            }
        }

    }


}
