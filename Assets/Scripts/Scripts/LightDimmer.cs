using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightDimmer : MonoBehaviour
{
    public Light[] lights;

    public Button[] button;
    public Slider[] sliders;
    public Slider slider;

    public Toggle _toggle;

    bool setaActive = true;
    float clickcount;

    private void Start()
    {
        button[0].onClick.AddListener(() => StartCoroutine(ButtonOnClick(button[0], sliders[0], lights[0])));
        button[1].onClick.AddListener(() => StartCoroutine(ButtonOnClick(button[1], sliders[1], lights[1])));
        button[2].onClick.AddListener(() => StartCoroutine(ButtonOnClick(button[2], sliders[2], lights[2])));
        button[3].onClick.AddListener(() => StartCoroutine(ButtonOnClick(button[3], sliders[3], lights[3])));
        _toggle.onValueChanged.AddListener(delegate { SelectAll(); });
    }

    void Update()
    {
        lights[0].intensity = sliders[0].value;
        lights[1].intensity = sliders[1].value;
        lights[2].intensity = sliders[2].value;
        lights[3].intensity = sliders[3].value;

        if (_toggle.isOn == true)
        {
            lights[0].intensity = slider.value;
            lights[1].intensity = slider.value;
            lights[2].intensity = slider.value;
            lights[3].intensity = slider.value;
        }
    }
    public void SelectAll()
    {
        if (_toggle.isOn == true)
        {
            slider.gameObject.SetActive(true);

            lights[0].intensity = slider.value;
            lights[1].intensity = slider.value;
            lights[2].intensity = slider.value;
            lights[3].intensity = slider.value;

            for (int x = 0; x < 4; x++)
            {
                if (sliders[x].gameObject.activeInHierarchy == true)
                {
                    sliders[x].gameObject.SetActive(false);
                }
                lights[x].gameObject.SetActive(true);
                button[x].gameObject.GetComponent<Image>().color = Color.cyan;
            }
        }
        else if (_toggle.isOn == false)
        {
            for (int x = 0; x < 4; x++)
            {

                slider.gameObject.SetActive(false);
                lights[x].gameObject.SetActive(false);
                button[x].gameObject.GetComponent<Image>().color = Color.grey;
            }
        }
    }

    public IEnumerator ButtonOnClick(Button botao, Slider slider, Light light)
    {


        Image im = botao.GetComponent<Image>();
        clickcount += 1;


        yield return new WaitForSeconds(0.25f);

        if (clickcount == 1)
        {
            if (slider.gameObject.activeInHierarchy == true)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.gameObject.SetActive(true);
            }
            if (slider.gameObject.activeInHierarchy == true) { im.color = Color.white; }
            else if ((im.color == Color.white) && (slider.gameObject.activeInHierarchy == false)) { im.color = Color.gray; }

        }
        if (clickcount > 1)
        {
            if (light.gameObject.activeInHierarchy == true)
            {
                light.gameObject.SetActive(false);
            }
            else { light.gameObject.SetActive(true); }

            if (light.gameObject.activeInHierarchy == true) { im.color = Color.cyan; }
            else if ((light.gameObject.activeInHierarchy == false) && (im.color == Color.cyan)) { im.color = Color.grey; }


        }

        clickcount = 0;

        yield return null;
        for (int x = 0; x < 4; x++)
        {
            if ((sliders[x].gameObject.activeInHierarchy == true) && (sliders[x] != slider))
            {
                sliders[x].gameObject.SetActive(false);
            }
            if (button[x] != botao)
            {
                button[x].gameObject.GetComponent<Image>().color = Color.gray;
            }
            if ((lights[x].gameObject.activeInHierarchy == true) && (button[x].gameObject.GetComponent<Image>().color == Color.grey))
            {
                button[x].gameObject.GetComponent<Image>().color = Color.cyan;
            }
            if((lights[x].gameObject.activeInHierarchy == true) && (button[x].gameObject.GetComponent<Image>().color == Color.white))
            {
                button[x].gameObject.GetComponent<Image>().color = Color.cyan;
            }
        }

    }
}
