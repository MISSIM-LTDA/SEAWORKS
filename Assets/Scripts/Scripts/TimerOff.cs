using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerOff : MonoBehaviour
{

    public float timeLeft;
    public GameObject Tela1;
    public GameObject ROV;
    public GameObject Tela2;

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0 || timeLeft ==0)
        {
            Tela1.SetActive(true);
            ROV.SetActive(false);
            Tela2.SetActive(false);
        }
    }

}
