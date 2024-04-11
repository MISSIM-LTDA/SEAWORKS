using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnim : MonoBehaviour {
    public Animator Animt;

    // Use this for initialization
    void Start() {
        Animt = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void Remove()
    {
        Animt.Play("travaout");
    }

    public void Install()
    {
        Animt.Play("travaIn");
    }





}
