using UnityEngine;
public class SubmergedGravity : MonoBehaviour 
{
	public GameObject waterBody;
    public GameObject waterBodyUnder;	
	private GameObject Player;
	public bool checkedIfAboveWater;
	private float waterHeight;
    private Rigidbody rigidbody2;


    void Start () 
	{
	    rigidbody2 = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player2"); // Cache the player		
		checkedIfAboveWater = false;
        
        waterHeight = waterBody.transform.position.y; // This is critical! It is the height of the water plane to determine we are underwater or not
		AssignAboveWaterSettings (); // Initially set above water settings
	}
	// Update is called once per frame
	void Update () 
	{
        // the checkedifAboveWater stops it forcing it over and over every frame if we already know where we are
        // If tghe player is above water and we haven't confirmed this yet, then set settings for above water and confirm
		if (transform.position.y >= waterHeight && checkedIfAboveWater == false) 
		{
            checkedIfAboveWater = true;
			ApplyAboveWaterSettings ();
			ToggleFlares (true);
		}
        // If we are under water and we haven't confirmed this yet, then set for under water and confirm
		if (transform.position.y < waterHeight && checkedIfAboveWater == true) 
		{
			checkedIfAboveWater = false;
			ApplyUnderWaterSettings ();
			ToggleFlares (false);
		}
	}
    // Initially assign current world fog ready for reuse later in above water 
	void AssignAboveWaterSettings () 
	{
        rigidbody2.useGravity = true; // turn gravity on


    }
    // Apply Abovewater default settings - enabling the above water view and effects
    void ApplyAboveWaterSettings () 
	{
        rigidbody2.useGravity = true; // turn gravity on
    }

    // Apply Underwater default settings - enabling the under water view and effects
    void ApplyUnderWaterSettings () 
	{

        rigidbody2.useGravity = false; // turn gravity on
    }
    // Toggle flares on or off depending on whether we are underwater or not
	void ToggleFlares (bool state) 
	{
		LensFlare[] lensFlares = FindObjectsOfType(typeof(LensFlare)) as LensFlare[];
		foreach (LensFlare currentFlare in lensFlares) 
		{
			currentFlare.enabled = state;
		}
	}
	
	
	
	
}
