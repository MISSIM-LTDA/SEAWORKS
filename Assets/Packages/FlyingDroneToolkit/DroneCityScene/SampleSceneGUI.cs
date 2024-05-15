using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SampleSceneGUI : MonoBehaviour {

    // Main toolbar.
	private int toolbarInt = -1;
	private int saveToolbarInt = -1;
	private string[] toolbarStrings = { "Briefing Missão", "Manual", "ROV Control", "ROV Maintenance", "Restart"};
	private Rect toolbarRect;
	private bool changed=false;
    private bool paused = false;
    public GameObject theDrone;
    public GameObject saucerDrone;
    public GameObject barco;
    public GameObject ROV;
    public GameObject briefing;
    public GameObject arrived;
    public GameObject endlevel;
    public GameObject endreturn;
    public GameObject thrusterH;
    public Button yourButton;


    // Camera toolbar.
    private int camToolbarInt = -1;
    private int camSaveToolbarInt = -1;
    private string[] camToolbarStrings = { "Back Cam", "Sonar", "No Cam", "Light On", "Light Off" };
    private Rect camToolbarRect;
    private bool camChanged = false;
    public GameObject saucerCam;
    public GameObject KyleCam;
    public GameObject Light;
    public GameObject LarsControl;
    public GameObject ROVControl;
    public GameObject HPU;
    public GameObject frame;
    public GameObject sonar;
    public GameObject cameracolor;
    public GameObject cameralow;
    public GameObject pantilt;
    public GameObject garrafa;
    public GameObject LEDLight;
    public GameObject manip;
    public GameObject filtro;
    public GameObject comp;
    public GameObject press;
    public GameObject sensors;
    public GameObject cooler;
    public GameObject profiler;
    public GameObject valvepack;
    public GameObject painelrov;
    public GameObject DamagePanel;
    public GameObject profilermanutencao;
    public GameObject LEDmanutencao;



    private FlyingDroneScript drScript;

	void Start()
	{
		drScript = theDrone.GetComponent<FlyingDroneScript>();        

    }

    public void FecharPause()
    {
       
        briefing.SetActive(false);
        arrived.SetActive(false);
        thrusterH.SetActive(false);
        HPU.SetActive(false);
        frame.SetActive(false);
        sonar.SetActive(false);
        cameracolor.SetActive(false);
        cameralow.SetActive(false);
        pantilt.SetActive(false);
        garrafa.SetActive(false);
        LEDLight.SetActive(false);
        manip.SetActive(false);
        filtro.SetActive(false);
        comp.SetActive(false);
        press.SetActive(false);
        sensors.SetActive(false);
        cooler.SetActive(false);
        profiler.SetActive(false);
        valvepack.SetActive(false);
        painelrov.SetActive(false);
        profilermanutencao.SetActive(false);
        LEDmanutencao.SetActive(false);
        DamagePanel.SetActive(false);

    }

    public void FecharPause2()
    {

        arrived.SetActive(false);
    }

    public void FecharPause3()
    {

        endlevel.SetActive(false);
        endreturn.SetActive(false);
    }



    void OnGUI () 
	{
        

        // Manage camera toolbar.
        camToolbarRect = gameObject.GetComponent<Camera>().pixelRect;
        camToolbarRect.xMin += 150;
        camToolbarRect.yMin = camToolbarRect.height - 60;
        camToolbarRect.width /= 1;
        camToolbarRect.height = 30;
        camToolbarInt = GUI.Toolbar(camToolbarRect, camToolbarInt, camToolbarStrings);
        if (camToolbarInt != camSaveToolbarInt)
            camChanged = true;
        else
            camChanged = false;
        camSaveToolbarInt = camToolbarInt;
        if (camChanged)
        {
            //Debug.Log(string.Format("camToolbar changed to {0}", camToolbarInt));
            switch (camToolbarInt)
            {
                case 0:       // Saucer Cam
                    saucerCam.SetActive(true);
                    KyleCam.SetActive(false);
                    break;
                case 1:       // Kyle Cam
                    saucerCam.SetActive(false);
                    KyleCam.SetActive(true);
                    break;
                case 2:       // No Cam
                    saucerCam.SetActive(false);
                    KyleCam.SetActive(false);
                    break;
                case 3:       // Light On
                    Light.SetActive(true);
                    break;
                case 4:       // Light Off
                    Light.SetActive(false);
                    break;
            }
        }

    }
		
}
