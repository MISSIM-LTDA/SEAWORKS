using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RovToolingSelection : MonoBehaviour
{
    [Serializable]
    public class SelectionButton 
    {
        public string name;

        public List<GameObject> tools = new List<GameObject> ();

        public List<ObiRopeMeshRenderer>  obiRopeMeshRenderers =
            new List<ObiRopeMeshRenderer>();

        public Button[] toolButton;

        public ToggleGroup toggleGroup;
        public Toggle[] toolLocationToggle;
        public SelectionButton(string name,Button[] toolButton, 
            ToggleGroup toggleGroup, Toggle[] toolLocationToggle) 
        {
            this.name = name;
            this.toolButton = toolButton;
            this.toggleGroup = toggleGroup;
            this.toolLocationToggle = toolLocationToggle;

            for(int i = 0; i < this.toolLocationToggle.Length; i++) {
                int j = i;
                this.toolLocationToggle[j].onValueChanged.
                    AddListener(delegate { ToggleLocation(j); });
            }
        }
        public void ToggleLocation(int index)
        {
            if (toolLocationToggle[index].isOn){
                foreach(Button button in toolButton) {
                    button.gameObject.SetActive(false);
                }

                toolButton[index].gameObject.SetActive(true);
            }
        }
    }

    [SerializeField] private Transform toolsPacks;
    public SelectionButton[] toolsSelectionButtons;

    private GameObject onArmTool;
    private Button onArmToolButton;

    private GameObject onBasketTool;
    private Button onBasketToolButton;

    private GameObject flotTool;
    private Button flotToolButton;
    void Start()
    {
        toolsSelectionButtons = new SelectionButton[transform.childCount];
        for (int i = 0; i < toolsSelectionButtons.Length; i++) {
            Transform selectionButton = transform.GetChild(i);
            toolsSelectionButtons[i] = new SelectionButton(selectionButton.name,
                selectionButton.GetComponentsInChildren<Button>(true),
                selectionButton.GetComponentInChildren<ToggleGroup>(),
                selectionButton.GetComponentsInChildren<Toggle>());
        }

        foreach (Transform tool in toolsPacks) {
            foreach(SelectionButton toolSelectionButton in toolsSelectionButtons) {
                if (tool.name.Contains(toolSelectionButton.name) && 
                    toolSelectionButton.tools.Count < 2) {
                    toolSelectionButton.tools.Add(tool.gameObject);
                }
            }
        }

        foreach (SelectionButton toolSelectionButton in toolsSelectionButtons){
            foreach(GameObject tool in toolSelectionButton.tools) {
                foreach(ObiRopeMeshRenderer obiRopeMeshRenderers in 
                    tool.GetComponentsInChildren<ObiRopeMeshRenderer>()) {
                    toolSelectionButton.obiRopeMeshRenderers.
                        Add(obiRopeMeshRenderers);
                }
            }
        }

        foreach (SelectionButton selectionButton in toolsSelectionButtons) {
            if(selectionButton.name != "Flot") {
                selectionButton.toolButton[0].onClick.AddListener
                (() => TurnOnOffArmTool(selectionButton));
                if (selectionButton.toolButton.Length > 1){
                    selectionButton.toolButton[1].onClick.AddListener
                        (() => TurnOnOffBasketTool(selectionButton));
                }
            }

            else {
                for(int i = 0; i < selectionButton.toolButton.Length; i++) {
                    int j = i;
                    selectionButton.toolButton[i].onClick.AddListener
                    (() => TurnOnOffFlot(selectionButton,j));
                }
            }
        }

        foreach(SelectionButton selectionButton in toolsSelectionButtons) 
        {
           if(selectionButton.name != "Flot") 
           {
                if (selectionButton.tools.Count > 1 
                    && selectionButton.tools[1].activeSelf) 
                {
                    if (selectionButton.name == "HotStab") {
                        ChangeButtonColor(selectionButton.tools[1], 
                            selectionButton.toolButton[1]);
                    }

                    else {
                        onBasketTool = selectionButton.tools[1];
                        onBasketToolButton = selectionButton.toolButton[1];

                        ChangeButtonColor(onBasketTool, onBasketToolButton);
                    }
                }

                if (selectionButton.tools[0].activeSelf) 
                {
                    onArmTool = selectionButton.tools[0];
                    onArmToolButton = selectionButton.toolButton[0];

                    ChangeButtonColor(onArmTool, onArmToolButton);
                }
            }

           else 
           {
                if (selectionButton.tools[0].activeSelf) {
                    flotTool = selectionButton.tools[0];
                    flotToolButton = selectionButton.toolButton[0];

                    ChangeButtonColor(flotTool, flotToolButton);
                }

                else if (selectionButton.tools[1].activeSelf) {
                    flotTool = selectionButton.tools[1];
                    flotToolButton = selectionButton.toolButton[1];

                    ChangeButtonColor(flotTool, flotToolButton);
                }
            }
        }
    }
    public void TurnOnOffArmTool(SelectionButton toolSelection)
    {
        if(onArmTool) {
            onArmTool.SetActive(false);
            ChangeButtonColor(onArmTool, onArmToolButton);

            if (toolSelection.tools[0] == onArmTool) {
                onArmTool = null;
                onArmToolButton = null;

                return; 
            }
        }

        toolSelection.tools[0].SetActive(true);

        if (toolSelection.obiRopeMeshRenderers.Count > 0) {
            StartCoroutine(FixObiRopeMesh
                (toolSelection.obiRopeMeshRenderers[0]));
        }

        onArmTool = toolSelection.tools[0];
        onArmToolButton = toolSelection.toolButton[0];

        ChangeButtonColor(onArmTool,onArmToolButton);
    }
    public void TurnOnOffBasketTool(SelectionButton toolSelection) 
    {
        if (toolSelection.name == "HotStab") 
        {
            toolSelection.tools[1].SetActive
                (!toolSelection.tools[1].activeInHierarchy);

            StartCoroutine(FixObiRopeMesh
               (toolSelection.obiRopeMeshRenderers[1]));
            StartCoroutine(FixObiRopeMesh
              (toolSelection.obiRopeMeshRenderers[2]));

            ChangeButtonColor(toolSelection.tools[1], 
                toolSelection.toolButton[1]);
        }

        else {
            if (onBasketTool) {
                onBasketTool.SetActive(false);
                ChangeButtonColor(onBasketTool, onBasketToolButton);

                if (toolSelection.tools[1] == onBasketTool) {
                    onBasketTool = null;
                    onBasketToolButton = null;

                    return;
                } 
            }

            toolSelection.tools[1].SetActive(true);

            if (toolSelection.obiRopeMeshRenderers.Count > 0){
                StartCoroutine(FixObiRopeMesh
                    (toolSelection.obiRopeMeshRenderers[1]));
            }

            onBasketTool = toolSelection.tools[1];
            onBasketToolButton = toolSelection.toolButton[1];

            ChangeButtonColor(onBasketTool, onBasketToolButton);
        }
    }
    public void TurnOnOffFlot(SelectionButton toolSelection,int index) 
    {
        if (flotTool) {
            flotTool.SetActive(false);
            ChangeButtonColor(flotTool, flotToolButton);

            if (toolSelection.tools[index] == flotTool){
                flotTool = null;
                flotToolButton = null;

                return;
            }
        }

        toolSelection.tools[index].SetActive(true);

        flotTool = toolSelection.tools[index];
        flotToolButton = toolSelection.toolButton[index];

        ChangeButtonColor(flotTool, flotToolButton);
    }
    public void ChangeButtonColor(GameObject tool,Button toolButton) 
    {
        if (tool.activeSelf) { 
            toolButton.image.color = Color.cyan; }

        else { toolButton.image.color = Color.white; }
    }
    public IEnumerator FixObiRopeMesh(ObiRopeMeshRenderer obiRopeMeshRenderer) 
    {
        obiRopeMeshRenderer.enabled = false;
        yield return new WaitForEndOfFrame();
        obiRopeMeshRenderer.enabled = true;
    }
}
