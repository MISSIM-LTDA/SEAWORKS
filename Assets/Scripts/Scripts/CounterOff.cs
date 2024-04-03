using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterOff : MonoBehaviour {


    public int OpenC;
    public GameObject Tela1;
    public GameObject ROV;
    public GameObject Tela2;

    private int goldAmount = 0;

    void Start()
    {
        OpenC = PlayerPrefs.GetInt("OpenC");
        OpenC = OpenC + 1;
        PlayerPrefs.SetInt("OpenC", OpenC);
        PlayerPrefs.Save();
    }


    void Update()
    {
        if (OpenC > 45 || OpenC == 45)
        {
            Tela1.SetActive(true);
            ROV.SetActive(false);
            Tela2.SetActive(false);
        }

        
    }


    

    





}
