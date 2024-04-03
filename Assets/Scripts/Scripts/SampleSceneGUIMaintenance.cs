using UnityEngine;
using UnityEngine.UI;
using CnControls;
using UnityEngine.SceneManagement;
using System.Collections;

public class SampleSceneGUIMaintenance : MonoBehaviour {

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
    public GameObject info1;
    public GameObject endlevel;
    public GameObject endreturn;
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


    private FlyingDroneScript drScript;
    

    

    public void FecharPause2()
    {

        info1.SetActive(false);
       
    }

  

    void OnGUI () 
	{
        // Manage main toolbar.
		toolbarRect = gameObject.GetComponent<Camera>().pixelRect;
		toolbarRect.xMin += 10;
		toolbarRect.yMin += 10;
		toolbarRect.width -= 20;
		toolbarRect.height = 30;//
		toolbarInt = GUI.Toolbar (toolbarRect, toolbarInt, toolbarStrings);
		if (toolbarInt != saveToolbarInt)
			changed = true;
		else
			changed = false;
		saveToolbarInt = toolbarInt;
		if (changed)
		{
			//Debug.Log(string.Format("toolbar changed to {0}", toolbarInt));
			switch(toolbarInt)
			{
			case 0:       // Briefing Missão
                    briefing.SetActive(true);              
                    
                    //drScript.SetHoverMode();
                    break;
			case 1:       // Manual
				drScript.SetManualMode();
				break;
			case 2:       // ROV Maintenance
                          //drScript.SetLandMode();
                ROVControl.SetActive(true);
                LarsControl.SetActive(false);
                break;
			case 3:       // Patrol
                          //drScript.SetPatrolMode();
                    ROVControl.SetActive(false);
                    LarsControl.SetActive(true);
                    break;
			case 4:       // Follow Saucer
                    SceneManager.LoadScene("missao-inspecao-bulleye-2009desktop", LoadSceneMode.Single);
                    //drScript.followee = saucerDrone;
                    //drScript.SetFollowMode();
                    break;
			case 5:       // Follow Kyle
				drScript.followee = barco;
				drScript.SetFollowMode();
				break;
			}
		}

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
