using UnityEngine;

public class JoystickControl : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Space;
    public GameObject[] gameObjects;
    private int currentIndex = 0;

    void Start()
    {
        // Ensure at least one GameObject is provided in the array
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogError("No game objects provided. Please assign game objects to the array.");
            return;
        }

        // Activate the first GameObject and deactivate the rest
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(i == 0);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            // Deactivate the current GameObject
            gameObjects[currentIndex].SetActive(false);

            // Increment the index to switch to the next GameObject
            currentIndex = (currentIndex + 1) % gameObjects.Length;

            // Activate the next GameObject
            gameObjects[currentIndex].SetActive(true);
        }
    }
}
