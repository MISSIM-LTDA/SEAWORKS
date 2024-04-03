using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T4DemoScript : MonoBehaviour
{

    public float speed1;
    public float speed2;

    void Update()
    {

        if (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed1);
        }

        if (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed1);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed1);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed1);
        }

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed1);
        }

        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed1);
        }
        ///

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed2);
        }

        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed2);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed2);
        }

        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed2);
        }

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed2);
        }

        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.down * Time.deltaTime * speed2);
        }


    }





}
