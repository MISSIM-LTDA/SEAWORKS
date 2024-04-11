using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LightDimmer : MonoBehaviour
{
    private Transform lightsParent;

    private Light[] lights;

    private Button[] lightButtons;
    class DoubleClickHandler : MonoBehaviour,IPointerClickHandler
    {
        public float clickedCount;
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            clickedCount = eventData.clickCount;
        }
    }
    private DoubleClickHandler[] buttonHandlers;

    private Slider[] intensityControllers;

    private Toggle toggle;

    bool coroutineRunning;

    Color turnOffColor = Color.gray;
    Color selected = Color.white;
    Color turnOnColor = Color.cyan;

    private void Start()
    {
        lightsParent = GameObject.Find("Rov Lights").transform;
        lights = lightsParent.GetComponentsInChildren<Light>();

        intensityControllers = transform.GetComponentsInChildren<Slider>(true);
        for(int i = 0; i < intensityControllers.Length; i++) {
            int j = i;
            if (i < intensityControllers.Length-1) {
                intensityControllers[i].onValueChanged.AddListener(delegate {
                    OnSliderValueChange(intensityControllers[j].value, j);
                });
            }
            else {
                for(int k = 0; k < lights.Length; k++) {
                    int l = k;
                    intensityControllers[i].onValueChanged.AddListener(delegate {
                        OnSliderValueChange(intensityControllers[j].value, l);
                    });
                }
            }
        }

        toggle = transform.GetComponentInChildren<Toggle>();

        Button[] buttons = transform.GetComponentsInChildren<Button>();

        buttons[buttons.Length - 2].onClick.AddListener(() => TurnAllOnOff(true));
        buttons[buttons.Length - 1].onClick.AddListener(() => TurnAllOnOff(false));

        lightButtons = new Button[buttons.Length-2];

        for(int i=0;i<lightButtons.Length; i++) { lightButtons[i] = buttons[i]; }

        buttonHandlers = new DoubleClickHandler[lightButtons.Length];
        for (int i = 0; i < lightButtons.Length; i++){
            buttonHandlers[i] =
                lightButtons[i].gameObject.AddComponent<DoubleClickHandler>();

            int j = i;
            lightButtons[i].onClick.AddListener(() => StartCoroutine(ButtonOnClick(j)));
        }

        for (int i = 0; i < lights.Length; i++){
            if (lights[i].gameObject.activeInHierarchy){
                lightButtons[i].image.color = turnOnColor;
            }
            else{
                lightButtons[i].image.color = turnOffColor;
            }
        }

        toggle.onValueChanged.AddListener(delegate {SelectAll(); });
    }
    private void SelectAll()
    {
        if (toggle.isOn) {
            foreach(Button button in lightButtons) {
                button.image.color = selected;
            }

            for(int i = 0;i < intensityControllers.Length;i++){
                intensityControllers[i].gameObject.SetActive(i == intensityControllers.Length - 1);
            }
        }

        else{
            FixAllLights();
            intensityControllers[intensityControllers.Length - 1].gameObject.SetActive(false);
        }
    }
    private IEnumerator ButtonOnClick(int index)
    {
        if (!coroutineRunning) {
            coroutineRunning = true;

            yield return new WaitForSeconds(0.5f);

            foreach(Slider slider in intensityControllers) {
                slider.gameObject.SetActive(false);
            }

            toggle.isOn = false;

            if (buttonHandlers[index].clickedCount == 1) {

                FixAllLights(index);

                if (lightButtons[index].image.color != selected){
                    lightButtons[index].image.color = selected;
                    intensityControllers[index].value = lights[index].intensity;
                    intensityControllers[index].gameObject.SetActive(true);
                }

                else{FixLights(index);}
            }

            else {
                lights[index].gameObject.SetActive(!lights[index].gameObject.activeInHierarchy);
                FixAllLights();
            }

            buttonHandlers[index].clickedCount = 0;
            coroutineRunning = false;
        }
    }
    private void TurnAllOnOff(bool turn)
    {
        toggle.isOn = false;

        foreach(Slider slider in intensityControllers) {
            slider.gameObject.SetActive(false);
        }

        foreach(Light light in lights) {
            light.gameObject.SetActive(turn);
        }

        FixAllLights();
    }
    private void FixAllLights(int exception) 
    {
        for (int i = 0; i < lights.Length; i++){
            if (i != exception) { FixLights(i); }
        }
    }
    private void FixAllLights()
    {
        for (int i = 0; i < lights.Length; i++){
            FixLights(i);
        }
    }
    private void FixLights(int index) 
    {
        if (lights[index].gameObject.activeInHierarchy){
            lightButtons[index].image.color = turnOnColor;
        }
        else{
            lightButtons[index].image.color = turnOffColor;
        }
    }
    private void OnSliderValueChange(float value,int index) {lights[index].intensity = value;}
}
