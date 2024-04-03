using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimLLOT : MonoBehaviour
{
  
   
    private Animator anim2;
    public KeyCode Play;
    public bool A;
    public bool B;



    void Start()
    {

        anim2 = GetComponent<Animator>();
        //anim["spin"].layer = 123;
        A = true;
        B = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(Play))
        {
            if (A == true && B == false)
            {
                StartAnim();
                A = false;
                B = true;

            }
            else
            {

                if (A == false && B == true)
                {
                    StopAnim();
                    A = true;
                    B = false;

                }



            }



        }
    }

 

    public void StartAnim()
    {
        anim2.SetBool("start", true);
        anim2.SetBool("voltar", false);
    }

    public void StopAnim()
    {
        anim2.SetBool("start", false);
        anim2.SetBool("voltar", true);
    }

    public void BackAnim()
    {
        anim2.SetBool("start", false);
        anim2.SetBool("voltar", false);
    }

}