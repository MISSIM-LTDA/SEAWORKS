using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class SettingsScript : MonoBehaviour
{
    public RenderPipelineAsset[] qualityLevels;
    public int quality;

    public GameObject botaoLow;
    public GameObject botaoMedium;
    public GameObject botaoHigh;
    public GameObject botaoUltra;
    public GameObject PostEffects;
    void Start()
    {
        qualityLevels = new RenderPipelineAsset[4];

        quality = QualitySettings.GetQualityLevel();

        if(quality == 0) 
        {
            botaoLow.SetActive(true);
            botaoMedium.SetActive(false);
            botaoHigh.SetActive(false);
            botaoUltra.SetActive(false);
            PostEffects.SetActive(false);
        }

        if (quality == 1)
        {
            botaoLow.SetActive(false);
            botaoMedium.SetActive(true);
            botaoHigh.SetActive(false);
            botaoUltra.SetActive(false);
            PostEffects.SetActive(false);
        }

        if (quality == 2)
        {
            botaoLow.SetActive(false);
            botaoMedium.SetActive(false);
            botaoHigh.SetActive(true);
            botaoUltra.SetActive(false);
            PostEffects.SetActive(false);
        }

        if (quality == 3)
        {
            botaoLow.SetActive(false);
            botaoMedium.SetActive(false);
            botaoHigh.SetActive(false);
            botaoUltra.SetActive(true);
            PostEffects.SetActive(true);
        }
    }

    public void ChangeLevel() 
    {
        QualitySettings.SetQualityLevel(quality);
        QualitySettings.renderPipeline = qualityLevels[quality];
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void LOW() 
    {
        quality = 0;
        PostEffects.SetActive(false);
        ChangeLevel();
    }

    public void MEDIUM()
    {
        quality = 1;
        PostEffects.SetActive(false);
        ChangeLevel();
    }
    public void HIGH()
    {
        quality = 2;
        PostEffects.SetActive(false);
        ChangeLevel();
    }
    public void ULTRA()
    {
        quality = 3;
        PostEffects.SetActive(true);
        ChangeLevel();
    }
}
