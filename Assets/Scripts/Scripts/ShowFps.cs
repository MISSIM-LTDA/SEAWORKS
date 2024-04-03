using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShowFps : MonoBehaviour
{
    public Text fpsText;
    public float fps;
    public float deltaTime;
    public bool showingFPS;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
            showingFPS = !showingFPS;

        if (showingFPS == true)
            fpsText.gameObject.SetActive(true);
        else
            fpsText.gameObject.SetActive(false);

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
        fpsText.text = ("FPS:" + Mathf.Ceil(fps).ToString());

    }
}