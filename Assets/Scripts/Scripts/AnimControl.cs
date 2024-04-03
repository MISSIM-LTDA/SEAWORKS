using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AnimControl : MonoBehaviour
{
    private Animator anim;
    public GameObject indicator;
    public GameObject mesh;
    public KeyCode Play;
    public bool A;
    public bool B;
    public bool C;
    public bool checkIn;

    void Start()
    {
        anim = indicator.GetComponent<Animator>();
        A = true;
        B = false;
        C = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(Play) && checkIn)
        {
            if (A && !B)
            {
                StartAnim();
                A = false;
                B = true;

            }

            else if(!A && B)
            {
                StopAnim();
                A = true;
                B = false;    
            }
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        checkIn = true;
        if (mesh.GetComponent<MeshCollider>() == null)
        {
            mesh.AddComponent<MeshCollider>();
        }
    }


    private void OnTriggerExit(Collider collision)
    {
        checkIn = false;
        Destroy(mesh.GetComponent<MeshCollider>());
    }
    public void StartAnim(){anim.Play("1");}
    public void StopAnim(){anim.Play("2");}
    public void BackAnim(){ anim.Play("2");}
}