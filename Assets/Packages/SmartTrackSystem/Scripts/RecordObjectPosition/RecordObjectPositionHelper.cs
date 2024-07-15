using Obi;
using SimpleFileBrowser;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SmartTrackSystem
{
    [RequireComponent(typeof(ObiSolver))]
    [ExecuteInEditMode]
    public class RecordObjectPositionHelper : MonoBehaviour
    {
        [SerializeField] private List<RecordedObject> recordedObjects = new List<RecordedObject>();

        [SerializeField, HideInInspector] private List<Button> objectsButtons = new List<Button>();

        [SerializeField, HideInInspector] private Button saveButton;
        [SerializeField, HideInInspector] private Button loadButton;
        [SerializeField, HideInInspector] private Button saveAllButton;
        [SerializeField, HideInInspector] private Button loadAllButton;

        [SerializeField, HideInInspector] private bool createdSetup;

        [SerializeField, HideInInspector] private GameObject selectedObject;
        public GameObject SelectedObject { set { selectedObject = value; } }
        public void CreateSetup()
        {
            //saveButton = CreateFunctionButtons("SaveButton", 0);
            //loadButton = CreateFunctionButtons("LoadButton", 1);
            //saveAllButton = CreateFunctionButtons("SaveAllButton", 2);
            //loadAllButton = CreateFunctionButtons("LoadAllButton", 3);

            //createdSetup = true;

            //Debug.Log("Setup Created");
        }
        public void ClearSetup(bool debug)
        {
            //foreach (RecordedObject rO in recordedObjects){
            //    DestroyImmediate(rO);
            //}
            //recordedObjects.Clear();

            //objectsButtons.Clear();

            //saveButton = null;
            //loadButton = null;
            //saveAllButton = null;
            //loadAllButton = null;

            //createdSetup = false;

            //if (debug) { Debug.Log("Setup Cleaned"); }
        }
        public void SetButtonsEvents()
        {
            //for (int i = 0, j = 0; i < objectsButtons.Count; i++, j++){
            //    if (recordedObjects[j].GetComponent<RecordedRope>() != null) { j += 2; }
            //    objectsButtons[i].onClick.AddListener(recordedObjects[j].SelectObject);
            //}

            //saveButton.onClick.AddListener(SaveNewPath);
            //loadButton.onClick.AddListener(LoadNewPath);
            //saveAllButton.onClick.AddListener(SaveAllNewPaths);
            //loadAllButton.onClick.AddListener(LoadAllNewPaths);
        }
        public Button CreateFunctionButtons(string name, int offset)
        {

            //GameObject prefab = null;// CreatePrefabOnScene(name);
            Button button = null;

            //if (prefab){
            //    button = prefab.GetComponent<Button>();
            //    button.transform.SetParent(objectsUI.transform, false);
            //    button.GetComponent<RectTransform>().anchoredPosition = new Vector2(830, 20 + (offset * (-40)));
            //}

            return button;
        }
        public RecordedObject SetupRecordedObject(RecordedObject recordedObject, ObiActor rope, RecordedObject otherConnector, Button objectButton)
        {
            recordedObject.rope = rope;
            recordedObject.otherConnector = otherConnector;
            recordedObject.button = objectButton;

            return recordedObject;
        }

        //public void SaveNewPath()
        //{
        //    if (selectedObject){
        //        if (selectedObject.gameObject.GetComponent<RecordedObject>()){
        //            selectedObject.GetComponent<RecordedObject>().SaveNewPath(null);
        //        }
        //    }

        //    else { Debug.Log("None object Selected!"); }
        //}
        //public void LoadNewPath()
        //{
        //    if (selectedObject) {
        //        if (selectedObject.gameObject.GetComponent<RecordedObject>()) {
        //            selectedObject.GetComponent<RecordedObject>().LoadNewPath(null);
        //        }
        //    }

        //    else { Debug.Log("None object Selected!"); }
        //}
        //public void SaveAllNewPaths() { StartCoroutine(SaveAllPathsCoroutine()); }
        //IEnumerator SaveAllPathsCoroutine()
        //{
        //    yield return StartCoroutine(ChooseSaveDirectory());

        //    for (int i = 0; i < recordedObjects.Count; i++) {
        //        recordedObjects[i].SaveNewPath(folderPath);
        //        if (recordedObjects[i].GetComponent<RecordedRope>() != null) { i += 2; }
        //    }
        //}
        //public void LoadAllNewPaths() { StartCoroutine(LoadAllPathsCoroutine()); }
        //IEnumerator LoadAllPathsCoroutine()
        //{
        //    yield return StartCoroutine(ChooseLoadDirectory());

        //    for (int i = 0; i < recordedObjects.Count; i++) {
        //        recordedObjects[i].LoadNewPath(folderPath);
        //        if (recordedObjects[i].GetComponent<RecordedRope>() != null) { i += 2; }
        //    }
        //}
    }
}
