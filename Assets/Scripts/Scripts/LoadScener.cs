using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScener : MonoBehaviour {

	public void LoadLevel1()
    {
        SceneManager.LoadScene("WORK CLASS LEARNING TOOL1812", LoadSceneMode.Single);
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("OK-missao-inspecao-bulleye-2509-desktop", LoadSceneMode.Single);
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("MaintenanceLEDLight_18122", LoadSceneMode.Single);
    }
}
