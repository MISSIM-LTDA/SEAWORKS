using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGenerate : MonoBehaviour
{
    public float Speed;
    public bool ShouldMove;
    public bool Reverse;
    public bool Axis_X;
    public bool Axis_Y;
    public bool Axis_Z;
        
    public float Botao1input;
    public float Botao1inputR;

    public KeyCode Sent1;
    public KeyCode Sent2;

    
    // Use this for initialization
    void Start()
    {
        ShouldMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        

        //Botao1input = Mathf.Clamp(Botao1input.GetKeyDown(Sent1), -1f, 1f);
        //Botao1inputR = Botao1input * (-1);


        if (Input.GetKey(Sent1))//Input.GetKey(Control1)Input.GetButtonDown(Marcha2)
        {

            if (Axis_X)
            {

                if (Reverse)
                {
                    transform.Rotate(Vector3.right * Time.deltaTime * Speed);
                }
                else
                {
                    transform.Rotate(Vector3.left * Time.deltaTime * Speed);
                }

            }

            if (Axis_Y)
            {

                if (Reverse)
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * Speed);
                }
                else
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * Speed);
                }

            }

            if (Axis_Z)
            {

                if (Reverse)
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * Speed);
                }
                else
                {
                    transform.Rotate(Vector3.back * Time.deltaTime * Speed);
                }

            }
        }

        if (Input.GetKey(Sent2))
        {
            if (Axis_X)
            {

                if (Reverse)
                {
                    transform.Rotate(Vector3.left * Time.deltaTime * Speed);
                }
                else
                {
                    transform.Rotate(Vector3.right * Time.deltaTime * Speed);
                }

            }

            if (Axis_Y)
            {

                if (Reverse)
                {
                    transform.Rotate(Vector3.down * Time.deltaTime * Speed);
                }
                else
                {
                    transform.Rotate(Vector3.up * Time.deltaTime * Speed);
                }

            }

            if (Axis_Z)
            {

                if (Reverse)
                {
                    transform.Rotate(Vector3.back * Time.deltaTime * Speed);
                }
                else
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * Speed);
                }

            }
        }



    }






}