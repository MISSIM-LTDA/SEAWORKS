using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey;
    [SerializeField] private KeyCode toggleKeyboardKey;

    private Transform rovControls; 
    private GameObject[] controls;

    private int currentIndex;
    void Start()
    {
        rovControls = transform.Find("Rov Controls");
        controls = new GameObject[rovControls.childCount];
        for (int i = 0; i < controls.Length; i++) { 
            controls[i] = rovControls.GetChild(i).gameObject; 
        }

        // Ensure at least one GameObject is provided in the array
        if (controls.Length == 0){
            Debug.LogError("No game objects provided. Please assign game objects to the array.");
            return;
        }

        // Activate the first GameObject and deactivate the rest
        for (int i = 0; i < controls.Length; i++){
            controls[i].SetActive(i == 0);
        }
    }

    void Update()
    {
        if (controls.Length != 0 && (Input.GetKeyDown(toggleKey) || 
            (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(toggleKeyboardKey)))) {
            // Deactivate the current GameObject
            controls[currentIndex].SetActive(false);

            // Increment the index to switch to the next GameObject
            currentIndex = (currentIndex + 1) % controls.Length;

            // Activate the next GameObject
            controls[currentIndex].SetActive(true);
        }
    }
}
