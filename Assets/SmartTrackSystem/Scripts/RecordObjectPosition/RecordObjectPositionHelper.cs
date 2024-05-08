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
        private GameObject mainCamera;

        [SerializeField] private List<RecordedObject> recordedObjects = new List<RecordedObject>();

        [SerializeField, HideInInspector] private GameObject objectsUI;
        [SerializeField, HideInInspector] private List<Button> objectsButtons = new List<Button>();
        private Transform contentPanel;

        [SerializeField, HideInInspector] private Button saveButton;
        [SerializeField, HideInInspector] private Button loadButton;
        [SerializeField, HideInInspector] private Button saveAllButton;
        [SerializeField, HideInInspector] private Button loadAllButton;

        [SerializeField, HideInInspector] private bool createdSetup;

        [SerializeField, HideInInspector] private GameObject selectedObject;
        public GameObject SelectedObject { set { selectedObject = value; } }

        string folderPath;
        private void Start()
        {
            if (Application.isPlaying)
            {
                if (createdSetup) { SetButtonsEvents(); }

                FileBrowser.SetFilters(false, new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
                FileBrowser.SetDefaultFilter(".txt");
                FileBrowser.AddQuickLink("Users", "C:\\Users", null);
            }
        }
        public void CreateSetup()
        {
            ClearSetup(false);

            //Find all objects and put them on the recordedObjects's list
            //Add the RecordedObject script in the gameObjects that will be recorded

            objectsUI = CreatePrefabOnScene("ObjectsUI");
            if (objectsUI != null) { contentPanel = objectsUI.transform.Find("ObjectsOnScene/Viewport/Content"); }

            mainCamera = GameObject.FindGameObjectWithTag("Main");
            if (mainCamera.GetComponent<OutlineEffect>() == null) { mainCamera.AddComponent<OutlineEffect>(); }

            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                if (child.GetComponent<ObiActor>() != null)
                {
                    RecordedRope rr = child.GetComponent<RecordedRope>();
                    if (rr == null)
                    {
                        rr = child.gameObject.AddComponent<RecordedRope>();
                    }

                    ObiActor childActor = child.GetComponent<ObiActor>();
                    List<ObiParticleGroup> groups = childActor.sourceBlueprint.groups;

                    Button newRecordButton = CreateRecordButton(child.gameObject, objectsButtons.Count);
                    objectsButtons.Add(newRecordButton);

                    Transform startOfRope = null;
                    Transform endOfRope = null;

                    foreach (ObiParticleAttachment attach in child.GetComponents<ObiParticleAttachment>())
                    {
                        if (attach.particleGroup == groups[0])
                        {
                            if (attach.target.name == "Cont (1)") { startOfRope = attach.target.parent; }
                            else { startOfRope = attach.target; }
                            if (startOfRope.GetComponent<RecordedObject>() == null) { startOfRope.gameObject.AddComponent<RecordedObject>(); }
                        }
                        else if (attach.particleGroup == groups[groups.Count - 1])
                        {
                            if (attach.target.name == "Cont (1)") { endOfRope = attach.target.parent; }
                            else { endOfRope = attach.target; }
                            if (endOfRope.GetComponent<RecordedObject>() == null) { endOfRope.gameObject.AddComponent<RecordedObject>(); }
                        }
                    }

                    rr.startOfRope = startOfRope;
                    rr.endOfRope = endOfRope;

                    recordedObjects.Add(SetupRecordedObject(rr, childActor, null, newRecordButton));
                    recordedObjects.Add(SetupRecordedObject(startOfRope.GetComponent<RecordedObject>(), childActor, endOfRope.GetComponent<RecordedObject>(), newRecordButton));
                    recordedObjects.Add(SetupRecordedObject(endOfRope.GetComponent<RecordedObject>(), childActor, startOfRope.GetComponent<RecordedObject>(), newRecordButton));
                }

                else if ((child.tag == "EFL_Parent" || child.tag == "HFL_Parent") && !recordedObjects.Contains(child.gameObject.GetComponent<RecordedObject>()))
                {
                    RecordedObject rO = child.GetComponent<RecordedObject>();
                    if (rO == null)
                    {
                        rO = child.gameObject.AddComponent<RecordedObject>();
                    }

                    Button newRecordButton = CreateRecordButton(child.gameObject, objectsButtons.Count);
                    objectsButtons.Add(newRecordButton);

                    recordedObjects.Add(SetupRecordedObject(rO, null, null, newRecordButton));
                }
            }

            saveButton = CreateFunctionButtons("SaveButton", 0);
            loadButton = CreateFunctionButtons("LoadButton", 1);
            saveAllButton = CreateFunctionButtons("SaveAllButton", 2);
            loadAllButton = CreateFunctionButtons("LoadAllButton", 3);

            createdSetup = true;

            Debug.Log("Setup Created");
        }
        public void ClearSetup(bool debug)
        {
            foreach (RecordedObject rO in recordedObjects)
            {
                DestroyImmediate(rO);
            }
            recordedObjects.Clear();

            objectsUI = GameObject.Find("ObjectsUI(Clone)");
            if (objectsUI != null)
            {
                DestroyImmediate(objectsUI);
                objectsUI = null;
            }

            objectsButtons.Clear();

            saveButton = null;
            loadButton = null;
            saveAllButton = null;
            loadAllButton = null;

            mainCamera = null;

            createdSetup = false;

            if (debug) { Debug.Log("Setup Cleaned"); }
        }
        public void SetButtonsEvents()
        {
            for (int i = 0, j = 0; i < objectsButtons.Count; i++, j++)
            {
                if (recordedObjects[j].GetComponent<RecordedRope>() != null) { j += 2; }
                objectsButtons[i].onClick.AddListener(recordedObjects[j].SelectObject);
            }

            saveButton.onClick.AddListener(SaveNewPath);
            loadButton.onClick.AddListener(LoadNewPath);
            saveAllButton.onClick.AddListener(SaveAllNewPaths);
            loadAllButton.onClick.AddListener(LoadAllNewPaths);
        }
        public Button CreateFunctionButtons(string name, int offset)
        {

            GameObject prefab = CreatePrefabOnScene(name);
            Button button = null;

            if (prefab != null)
            {
                button = prefab.GetComponent<Button>();
                button.transform.SetParent(objectsUI.transform, false);
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(830, 20 + (offset * (-40)));
            }

            return button;
        }
        public RecordedObject SetupRecordedObject(RecordedObject recordedObject, ObiActor rope, RecordedObject otherConnector, Button objectButton)
        {
            recordedObject.positionHelper = GetComponent<RecordObjectPositionHelper>();
            recordedObject.rope = rope;
            recordedObject.otherConnector = otherConnector;
            recordedObject.button = objectButton;

            return recordedObject;
        }
        public Button CreateRecordButton(GameObject recordedObject, int offset)
        {
            Vector2 rectTransformOffset = new Vector2(0, 120);

            GameObject objectButton = Resources.Load<GameObject>("ObjectButton");
            GameObject newButton = Instantiate(objectButton, Vector3.zero, Quaternion.identity);
            newButton.transform.SetParent(contentPanel.transform, false);

            RectTransform rt = newButton.GetComponent<RectTransform>();
            rt.anchoredPosition = rectTransformOffset - new Vector2(0, offset * 75);
            rt.sizeDelta = new Vector2(450, 60);

            TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (recordedObject.GetComponent<ObiActor>() != null)
            {
                buttonText.text = recordedObject.GetComponent<ObiActor>().sourceBlueprint.name;
            }

            else { buttonText.text = recordedObject.name; }

            return newButton.GetComponent<Button>();
        }
        public GameObject CreatePrefabOnScene(string name)
        {
            GameObject prefab = Resources.Load<GameObject>(name);

            if (prefab != null) { return Instantiate(prefab, Vector3.zero, Quaternion.identity); }

            return null;
        }
        public void SaveNewPath()
        {
            if (selectedObject != null)
            {
                if (selectedObject.gameObject.GetComponent<RecordedObject>() != null)
                {
                    selectedObject.GetComponent<RecordedObject>().SaveNewPath(null);
                }
            }

            else { Debug.Log("None object Selected!"); }
        }
        public void LoadNewPath()
        {
            if (selectedObject != null)
            {
                if (selectedObject.gameObject.GetComponent<RecordedObject>() != null)
                {
                    selectedObject.GetComponent<RecordedObject>().LoadNewPath(null);
                }
            }

            else { Debug.Log("None object Selected!"); }
        }
        public void SaveAllNewPaths() { StartCoroutine(SaveAllPathsCoroutine()); }
        IEnumerator SaveAllPathsCoroutine()
        {
            yield return StartCoroutine(ChooseSaveDirectory());

            for (int i = 0; i < recordedObjects.Count; i++)
            {
                recordedObjects[i].SaveNewPath(folderPath);
                if (recordedObjects[i].GetComponent<RecordedRope>() != null) { i += 2; }
            }
        }
        public void LoadAllNewPaths() { StartCoroutine(LoadAllPathsCoroutine()); }
        IEnumerator LoadAllPathsCoroutine()
        {
            yield return StartCoroutine(ChooseLoadDirectory());

            for (int i = 0; i < recordedObjects.Count; i++)
            {
                recordedObjects[i].LoadNewPath(folderPath);
                if (recordedObjects[i].GetComponent<RecordedRope>() != null) { i += 2; }
            }
        }
        IEnumerator ChooseSaveDirectory()
        {
            yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Folders, false, null, null, "Save Files and Folders", "Save All");

            if (FileBrowser.Success) { folderPath = FileBrowser.Result[0]; }
        }
        IEnumerator ChooseLoadDirectory()
        {
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Folders, false, null, null, "Load Files and Folders", "Load All");

            if (FileBrowser.Success) { folderPath = FileBrowser.Result[0]; }
        }
    }
}
