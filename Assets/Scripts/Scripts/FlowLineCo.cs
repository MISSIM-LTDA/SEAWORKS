using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowLineCo : MonoBehaviour {

    public GameObject FimMissao;

    private void OnTriggerEnter(Collider other)
    {
        FimMissao.SetActive(true);

    }

    public void FecharFim()
    {
        FimMissao.SetActive(false);
    }
}
