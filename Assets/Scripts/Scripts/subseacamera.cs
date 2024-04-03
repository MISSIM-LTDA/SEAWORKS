using UnityEngine;
public class subseacamera : MonoBehaviour
{
   
    public Color underWaterColor;
    public float underWaterVisiblity;
    public bool aboveWaterFogMode;
    public Color aboveWaterColor;
    public float aboveWaterVisiblity;
    public GameObject WaterParticles;
    public Projector Caustics;
    public bool checkedIfAboveWater;
    public float waterHeight;
    private AudioSource m_AudioSource;
    private AudioSource m_JumpInWaterAudioSource;
    private AudioSource m_JumpOutOfWaterAudioSource;



    void Start()
    {
        ParticleSystem.EmissionModule waterParticles_emission; // Locate and cache the underwater particles effect and enable it
        waterParticles_emission = WaterParticles.GetComponent<ParticleSystem>().emission;
        waterParticles_emission.enabled = true;
      
        Caustics.enabled = false; // Initially turn off the caustics effect as we start above water
                                  //checkedIfAboveWater = false;
        //underWaterVisiblity = 0.3f; // Set the Underwater Visibility - can be adjusted publicly
                                     // Cache the audiosources for underwater, splash in and splash out of water
      //  m_AudioSource = waterBodyUnder.GetComponent<AudioSource>();
        m_JumpInWaterAudioSource = GameObject.FindGameObjectWithTag("JumpInWater").GetComponent<AudioSource>();
        m_JumpOutOfWaterAudioSource = GameObject.FindGameObjectWithTag("JumpOutOfWater").GetComponent<AudioSource>();
        //Camera.main.nearClipPlane = 0.1f;
       // waterHeight = waterBody.transform.position.y; // This is critical! It is the height of the water plane to determine we are underwater or not

       // AssignAboveWaterSettings(); // Initially set above water settings
    }
    // Update is called once per frame
    void Update()
    {
        // the checkedifAboveWater stops it forcing it over and over every frame if we already know where we are
      
           // checkedIfAboveWater = false;
            //ApplyUnderWaterSettings();

        RenderSettings.fog = aboveWaterFogMode;
        RenderSettings.fogColor = underWaterColor;
        RenderSettings.fogDensity = underWaterVisiblity;
        Caustics.enabled = true;

        if (!WaterParticles.GetComponent<ParticleSystem>().isPlaying)
        {
            WaterParticles.GetComponent<ParticleSystem>().Play();
        }

    }
    // Initially assign current world fog ready for reuse later in above water 
    void AssignAboveWaterSettings()
    {
        aboveWaterFogMode = RenderSettings.fog;
        aboveWaterColor = RenderSettings.fogColor;
        aboveWaterVisiblity = RenderSettings.fogDensity;

    }
    // Apply Abovewater default settings - enabling the above water view and effects
  

    // Apply Underwater default settings - enabling the under water view and effects
    void ApplyUnderWaterSettings()
    {

       /// waterBody.SetActive(false);
       // waterBodyUnder.SetActive(false);
        //PlayEnterSplashSound();
       // PlayUnderWaterSound();
        
       

    }
    // Toggle flares on or off depending on whether we are underwater or not
    
    private void PlayUnderWaterSound()
    {
        m_AudioSource.Play();
    }
    private void StopUnderWaterSound()
    {
        m_AudioSource.Stop();
    }
    private void PlayEnterSplashSound()
    {
        m_JumpInWaterAudioSource.Play();
    }
    private void PlayExitSplashSound()
    {
        m_JumpOutOfWaterAudioSource.Play();
    }
}
