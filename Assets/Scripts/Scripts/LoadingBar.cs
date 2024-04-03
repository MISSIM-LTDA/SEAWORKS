using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

    public GameObject ContTest;
    public GameObject InsulTest;
    public GameObject InsulTesFail;
    public GameObject ContTestFail;
    public GameObject PainelMaintenance;
    public float SparePart;
    public Text SpareText;
    public float DamageStatus;
    float DamageStatus1;


    private void Update()
    {
        SpareText.text = SparePart +"Left";
    }
 

    public void ContinuityTest()
    { 
            ContTest.SetActive(true);
            InsulTest.SetActive(false);          

    }

    public void InsulationTest()
    {
        InsulTest.SetActive(true);
        ContTest.SetActive(false);
    }

    public void FecharPainel()
    {
        PainelMaintenance.SetActive(false);
    }

    public void UsarSpare1()
    {
        //SpareText = GetComponent<Text>();
        SparePart = SparePart - 1;
        if(SparePart<0)
        {
            SparePart = 0;
        }

    }

    public void UsarSpare2()
    {
        //SpareText = GetComponent<Text>();
        SparePart = SparePart - 1;
        if (SparePart < 0)
        {
            SparePart = 0;
        }

    }

    public void UsarSpare3()
    {
        //SpareText = GetComponent<Text>();
        SparePart = SparePart - 1;
        if (SparePart < 0)
        {
            SparePart = 0;
        }

    }






}