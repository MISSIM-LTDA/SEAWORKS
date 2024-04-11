using UnityEngine;
using UnityEngine.UI;

public class CheckboxController1 : MonoBehaviour
{
    public Toggle m_Toggle;

    public Toggle[] checkboxes;
    private bool[] checkboxStates;

    public GameObject[] targetObjects;

    private void Start()
    {
        checkboxStates = new bool[checkboxes.Length];
        for (int i = 0; i < checkboxes.Length; i++)
        {
            checkboxStates[i] = checkboxes[i].isOn;
        }

        if(m_Toggle != null) { m_Toggle.isOn = true; }
    }

    private void Update()
    {
        for (int i = 0; i < checkboxes.Length; i++)
        {
            if (checkboxes[i].isOn != checkboxStates[i])
            {
                checkboxStates[i] = checkboxes[i].isOn;
                if (checkboxes[i].isOn)
                {
                    foreach (var otherCheckbox in checkboxes)
                    {
                        if (otherCheckbox != checkboxes[i])
                        {
                            otherCheckbox.isOn = false;
                        }
                    }
                    break;
                }
            }
        }

        // Loop through the checkboxes array
        for (int i = 0; i < checkboxes.Length; i++)
        {
            // Get the Toggle component of the checkbox
            Toggle toggle = checkboxes[i].GetComponent<Toggle>();

            // Set the interactable state of the checkbox
            if (toggle.isOn)
            {
                toggle.interactable = false;
            }
            else
            {
                toggle.interactable = true;
            }

            // Set the active state of the corresponding target object
            targetObjects[i].SetActive(toggle.isOn);
        }
    }
}
