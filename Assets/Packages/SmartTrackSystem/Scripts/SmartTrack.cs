using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using System.IO;

using Obi;
using SimpleFileBrowser;
using TMPro;
using System;

namespace SmartTrackSystem
{
    public class SmartTrack : MonoBehaviour
    {
        public static SmartTrack smartTrack { get; private set; }

        [SerializeField] private List<RecordedObject> recordedObjects = new List<RecordedObject>() { };

        [SerializeField] private List<RecordedObject> savePositionObjects = new List<RecordedObject>();
        [SerializeField, HideInInspector] private List<Button> savePositionsButtons = new List<Button>();

        [SerializeField, HideInInspector] private Camera mainCamera;

        [Serializable]
        public class RovItens
        {
            public List<FlyingDroneScript> flyingDroneScripts = new List<FlyingDroneScript>();
            public List<Rigmaster> rigmasters = new List<Rigmaster>();
            public List<Titan4> titan4s = new List<Titan4>();
            public List<Claw> claw = new List<Claw>();
            public List<GenericMovement> genericMovements = new List<GenericMovement>();

            public List<Animator> animators = new List<Animator>();

            public List<ObiRigidbody> obiRigidbodies = new List<ObiRigidbody>();
            public List<Rigidbody> rigidbodies = new List<Rigidbody>();


            public List<ObiCollider> obiColliders = new List<ObiCollider>();
            public List<Collider> colliders = new List<Collider>();
        }

        [SerializeField, HideInInspector] private RovItens rovItens = new RovItens();

        [Serializable]
        public class Matrix
        {
            public int[] array;
            public Matrix(int lenght)
            {
                this.array = new int[lenght];
            }
        }

        [SerializeField] private List<Matrix> ropesInitialIndexes = new List<Matrix>();

        [SerializeField, HideInInspector] private Transform objectsUI;
        private Transform contentPanel;

        [SerializeField, HideInInspector] private Button savePositionButton;
        [SerializeField, HideInInspector] private Button loadPositionButton;
        [SerializeField, HideInInspector] private Button saveAllPositionsButton;
        [SerializeField, HideInInspector] private Button loadAllPositionsButton;

        [SerializeField, HideInInspector] private GameObject selectedObject;
        public GameObject SelectedObject { set { selectedObject = value; } }

        [SerializeField, HideInInspector] private Transform smartTrackUIConsole;
        private Transform console;
        private Transform loadAlert;
        private TMP_Dropdown dropdown;

        private Text timeStamp;

        private TextMeshProUGUI multiplierText;
        private int multiplier = 1;

        private Slider slider;

        private Button playButton;
        private Sprite playSprite;
        private Sprite pauseSprite;

        private Button recordButton;
        private Sprite recordOnSprite;
        private Sprite recordOffSprite;

        private int slideT;

        private float recordingTime;
        private int recordedFrames;

        public float recordingRate;
        public int decimalPlaces;

        private Coroutine replayCoroutine;
        private bool preparedToReplay;

        private bool isRecording;

        [SerializeField, HideInInspector] private bool createdSetup;

        private string folderPath;

        private bool isReplaying;
        public bool IsReplaying { get { return isReplaying; } }
        private bool paused;

        private bool isSaving;

        private bool isLoading;
        private bool loadead;

        private Transform hitBody;
        private RecordedObject selectedBody;

        private void Awake()
        {
            if (smartTrack && smartTrack != this) { Destroy(this); }

            else { smartTrack = this; }
        }
        private void Start()
        {
            if (Application.isPlaying && createdSetup)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

                FileBrowser.SetFilters
                    (false, new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

                FileBrowser.SetDefaultFilter(".txt");
                FileBrowser.AddQuickLink("Users", "C:\\Users", null);

                SetupUI();
            }
        }
        public void Update()
        {
            if (createdSetup) 
            {
                if (Input.GetKey(KeyCode.LeftShift) && 
                    Input.GetKeyUp(KeyCode.Space)) { HideConsole(); }

                if (isRecording) { recordingTime += Time.deltaTime; }

                if (createdSetup) { RefreshTimer(); }

                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {

                    if (hitBody != hit.transform)
                    {
                        if (selectedBody) { selectedBody.mouseOver = false; }
                    }

                    hitBody = hit.transform;
                    selectedBody = hitBody.GetComponent<RecordedObject>();

                    if (selectedBody) { selectedBody.mouseOver = true; }
                }
            }
        }

        #region Setup Functions
        //Setup all gameObjects that will be recorded, also find and set all UI elements
        public void CreateSetUp()
        {
            ClearSetup(false);

            List<GameObject> gameObjectsToRecord = new List<GameObject>() { };

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("XLX"))
            {
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("ROVComponents"))
            {
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("Jaw7"))
            {
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("Jaw7finger"))
            {
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("Interectable"))
            {
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("Connectors Cap"))
            {
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (ObiRopeBase ropes in FindObjectsOfType<ObiRopeBase>())
            {
                gameObjectsToRecord.Add(ropes.gameObject);
            }

            Transform canvas = GameObject.Find("Lateral UI Panel ").transform;
            objectsUI = CreatePrefabOnScene("ObjectsOnScene", canvas, new Vector3(0, 225, 0));
            if (objectsUI) {
                contentPanel = objectsUI.transform.Find("Scroll Rect/Viewport/Content");
            }

            foreach (GameObject objectToRecord in gameObjectsToRecord) {
                ObiRopeBase rope = objectToRecord.GetComponent<ObiRopeBase>();

                Button newRecordButton = null;
                if (objectToRecord.CompareTag("Connectors Cap") ||
                    (rope && rope.sourceBlueprint.name != "Crane")) {
                    newRecordButton = CreateSavePositionButton(objectToRecord, savePositionsButtons.Count);
                    savePositionsButtons.Add(newRecordButton);
                }

                if (rope)
                {
                    RecordedRope rr = objectToRecord.GetComponent<RecordedRope>();
                    if (!rr) {
                        rr = objectToRecord.gameObject.AddComponent<RecordedRope>();
                    }

                    List<ObiParticleGroup> groups = rope.sourceBlueprint.groups;

                    Transform startOfRope;
                    Transform endOfRope;
                    RecordedConnector startOfRopeRo = null;
                    RecordedConnector endOfRopeRo = null;

                    foreach (ObiParticleAttachment attach in rope.GetComponents<ObiParticleAttachment>()) {
                        if (attach.target != rr.transform && attach.target != gameObjectsToRecord[0].transform) {
                            if (attach.particleGroup == groups[0]) {
                                if (attach.target.name == "Body") { startOfRope = attach.target.parent; }
                                else { startOfRope = attach.target; }

                                if (!startOfRope.GetComponent<RecordedObject>()) {
                                    startOfRopeRo = startOfRope.gameObject.AddComponent<RecordedConnector>();
                                }
                            }
                            else if (attach.particleGroup == groups[groups.Count - 1]) {
                                if (attach.target.name == "Body") { endOfRope = attach.target.parent; }
                                else { endOfRope = attach.target; }

                                if (!endOfRope.GetComponent<RecordedObject>()) {
                                    endOfRopeRo = endOfRope.gameObject.AddComponent<RecordedConnector>();
                                }
                            }
                        }
                    }


                    bool savePosition = rope.sourceBlueprint.name != "Crane";

                    rr = SetupRecordedObject(rr, rope);
                    recordedObjects.Add(rr);
                    if (savePosition) { savePositionObjects.Add(rr); }

                    if (startOfRopeRo) {
                        startOfRopeRo = SetupRecordedObject(startOfRopeRo, rope, endOfRopeRo, newRecordButton);
                        recordedObjects.Add(startOfRopeRo);
                        if (savePosition) {savePositionObjects.Add(startOfRopeRo);}
                    }
                    if (endOfRopeRo) {
                        endOfRopeRo = SetupRecordedObject(endOfRopeRo, rope, startOfRopeRo, newRecordButton);
                        recordedObjects.Add(endOfRopeRo);
                        if (savePosition) { savePositionObjects.Add(endOfRopeRo); }
                    }

                    ropesInitialIndexes.Add(new Matrix(0));
                }

                else
                {
                    RecordedObject rO = objectToRecord.GetComponent<RecordedObject>();
                    if (!rO) {
                        rO = objectToRecord.AddComponent<RecordedObject>();
                    }

                    recordedObjects.Add(SetupRecordedObject(rO, newRecordButton));

                    if (rO.CompareTag("Connectors Cap")) {
                        savePositionObjects.Add(SetupRecordedObject(rO, newRecordButton));
                    }
                }
            }

            //Setup all UI elements
            canvas = GameObject.Find("Canvas Display 1").transform;
            smartTrackUIConsole = CreatePrefabOnScene("SmartTrackConsole", canvas, new Vector3(0, 540, 0));

            savePositionButton = CreatePositionFunctionButtons("SaveButton", 0);
            loadPositionButton = CreatePositionFunctionButtons("LoadButton", 1);
            saveAllPositionsButton = CreatePositionFunctionButtons("SaveAllButton", 2);
            loadAllPositionsButton = CreatePositionFunctionButtons("LoadAllButton", 3);

            createdSetup = true;

            Debug.Log("Setup Created");
        }
        public void ClearSetup(bool debug)
        {
            foreach (RecordedObject recordedObject in recordedObjects) {
                DestroyImmediate(recordedObject);
            }
            recordedObjects.Clear();

            foreach (RecordedObject recordedObject in savePositionObjects) {
                DestroyImmediate(recordedObject);
            }
            savePositionObjects.Clear();

            ropesInitialIndexes.Clear();

            if (smartTrackUIConsole) {
                DestroyImmediate(smartTrackUIConsole.gameObject);
            }

            contentPanel = null;
            if (objectsUI){
                DestroyImmediate(objectsUI.gameObject);
            }

            savePositionsButtons.Clear();

            savePositionButton = null;
            loadPositionButton = null;
            saveAllPositionsButton = null;
            loadAllPositionsButton = null;

            createdSetup = false;

            if (debug) { Debug.Log("Setup Cleaned"); }
        }
        public void SetupUI()
        {
            console = smartTrackUIConsole.Find("Console");
            loadAlert = console.Find("Load Alert");

            Button returnPhysics = console.Find("ReturnROVPhysicsButton").GetComponentInChildren<Button>();
            returnPhysics.onClick.AddListener(ReturnPhysics);

            Button saveButton = console.Find("SaveButton").GetComponentInChildren<Button>();
            saveButton.onClick.AddListener(Save);

            timeStamp = console.Find("TimeStamp").GetComponentInChildren<Text>();

            Button multiplierButton = console.Find("Multiplier").GetComponentInChildren<Button>();
            multiplierText = multiplierButton.GetComponentInChildren<TextMeshProUGUI>();
            multiplierButton.onClick.AddListener(Multiplier);

            slider = console.GetComponentInChildren<Slider>();
            slider.onValueChanged.AddListener(OnSliderValueChange);

            Button stopButton = console.Find("StopButton").GetComponentInChildren<Button>();
            stopButton.onClick.AddListener(Stop);

            playButton = console.Find("PlayButton").GetComponentInChildren<Button>();
            playButton.onClick.AddListener(PlayOrPause);

            playSprite = Resources.Load<Sprite>("play");
            pauseSprite = Resources.Load<Sprite>("pause");

            recordButton = console.Find("RecordButton").GetComponentInChildren<Button>();
            recordButton.onClick.AddListener(Record);

            recordOnSprite = Resources.Load<Sprite>("recOn");
            recordOffSprite = Resources.Load<Sprite>("recOff");

            Button clearButton = console.Find("ClearButton").GetComponentInChildren<Button>();
            clearButton.onClick.AddListener(delegate { ClearRecordData(false); });

            Button loadButton = console.Find("LoadButton").GetComponentInChildren<Button>();
            loadButton.onClick.AddListener(Load);

            Button confirmLoad = loadAlert.Find("ConfirmButton").GetComponentInChildren<Button>();
            confirmLoad.onClick.AddListener(delegate { StartCoroutine(LoadFromDirectory(true)); });

            Button denyLoad = loadAlert.Find("DenyButton").GetComponentInChildren<Button>();
            denyLoad.onClick.AddListener(delegate { StartCoroutine(LoadFromDirectory(false)); });

            dropdown = console.GetComponentInChildren<TMP_Dropdown>();
            dropdown.onValueChanged.AddListener(delegate { StartCoroutine(ChooseFile()); });

            savePositionButton.onClick.AddListener(SaveNewPosition);
            loadPositionButton.onClick.AddListener(LoadNewPosition);
            saveAllPositionsButton.onClick.AddListener(SaveAllNewPositions);
            loadAllPositionsButton.onClick.AddListener(LoadAllNewPositions);
        }
        private void HideConsole()
        {
            console.gameObject.SetActive
                (!console.gameObject.activeSelf);
        }

        #endregion

        #region Record Functions
        public void Record()
        {
            if (!preparedToReplay) {
                if (!isRecording) {
                    recordButton.image.sprite = recordOnSprite;
                    isRecording = true;

                    if (recordedFrames > 0) {
                        if (isReplaying) {
                            FixTimer();

                            int index = (int)slider.value;
                            int count = recordedFrames - index;
                            int rope = 0;

                            foreach (RecordedObject recordedObject in recordedObjects)
                            {
                                RecordedRope rr = recordedObject as RecordedRope;
                                if (rr) {
                                    int ropeIndex = ropesInitialIndexes[rope].array[index];
                                    ropeIndex += rr.recordRope.RecordObjectStore[ropeIndex].pC;

                                    int ropeCount = rr.recordRope.RecordObjectStore.Count - ropeIndex;

                                    rr.recordRope.RecordObjectStore.RemoveRange(ropeIndex, ropeCount);
                                }
                                else {
                                    recordedObject.record.RecordObjectStore.RemoveRange(index, count);
                                }
                            }

                            slider.value = 0;
                            slider.maxValue = count;

                            slideT = 0;
                        }
                    }

                    StartCoroutine(Recording());
                }

                else if (isRecording)
                {
                    recordButton.image.sprite = recordOffSprite;
                    isRecording = false;

                    slider.value = 0;
                    slider.maxValue = recordedFrames - 1;

                    slideT = 0;

                    ArrangeRopeIndexes(recordedFrames);
                }
            }

            else { Debug.Log("You only can record a physic ROV!"); }
        }
        public IEnumerator Recording()
        {
            while (isRecording) {
                foreach (RecordedObject recordedObject in recordedObjects) {
                    RecordedRope rr = recordedObject as RecordedRope;
                    if (rr) { rr.GetObjectPosition(ref rr.recordRope); }
                    else { recordedObject.GetObjectPosition(ref recordedObject.record); }
                }

                recordedFrames = recordedObjects[0].record.RecordObjectStore.Count;

                yield return new WaitForSeconds(recordingRate);
            }
        }
        #endregion

        #region Replay Functions
        private void PlayOrPause()
        {
            if (!isSaving && !isLoading && !isRecording && recordedFrames > 0)
            {
                if (playButton.image.sprite == playSprite)
                {
                    if (slider.value == slider.maxValue) {
                        slider.value = 0;
                        slideT = 0;
                    }

                    else {
                        slideT = (int)slider.value;
                    }

                    playButton.image.sprite = pauseSprite;
                    replayCoroutine = StartCoroutine(Play());
                }

                else
                {
                    playButton.image.sprite = playSprite;
                    StopCoroutine(replayCoroutine);
                    paused = true;
                }
            }

            else if (recordedFrames == 0) { Debug.Log("You are trying to replay an empty recording!"); }

            else if (isSaving) { Debug.Log("You are saving a recording you can't replayed yet!"); }

            else if (isLoading) { Debug.Log("You are loading a recording you can't replayed yet!"); }

            else if (isRecording) { Debug.Log("You are recording a recording you can't replayed yet!"); }
        }
        private IEnumerator Play()
        {
            if (!preparedToReplay) { PrepareToReplay(); }

            isReplaying = true;
            paused = false;

            slider.interactable = true;
            for (slider.value = slideT; slider.value < slider.maxValue; slider.value += multiplier) {

                yield return new WaitForSeconds(recordingTime / recordedFrames);
            }

            PlayOrPause();
        }
        private void Stop()
        {
            playButton.image.sprite = playSprite;

            slider.value = 0;
            slideT = 0;

            if(replayCoroutine != null) {
                StopCoroutine(replayCoroutine);
            }

            isReplaying = false;
            paused = false;
        }
        private void OnSliderValueChange(float frame)
        {
            if (preparedToReplay && isReplaying) {
                slideT = (int)frame;

                int ropes = 0;
                foreach (RecordedObject recordObject in recordedObjects)
                {
                    RecordedRope rr = recordObject as RecordedRope;
                    if (rr)
                    {
                        int ropePosition = ropesInitialIndexes[ropes].array[slideT];
                        ropes++;

                        rr.index = ropePosition;
                        rr.LoadPositions(ref rr.recordRope, false);
                    }

                    else
                    {
                        recordObject.index = slideT;
                        recordObject.LoadPositions(ref recordObject.record, false);
                    }
                }
            }
        }
        private void Multiplier()
        {
            switch (multiplier)
            {
                case 1:
                    multiplier = 10;
                    break;
                case 10:
                    multiplier = 1;
                    break;
            }

            multiplierText.text = multiplier.ToString() + "x";
        }
        #endregion

        #region Save Functions
        private void Save()
        {
            if (!isSaving && !isLoading && !isRecording)
            {
                StartCoroutine(SaveCoroutine());
            }

            else if (isSaving) { Debug.Log("You already are saving an recording!"); }

            else if (isLoading) { Debug.Log("You can't save while you load a recording!"); }

            else if (isRecording) { Debug.Log("You can't save while recording!"); }
        }
        private IEnumerator SaveCoroutine()
        {
            if (recordedObjects.Count > 0 && recordedFrames > 0)
            {
                isSaving = true;

                yield return StartCoroutine(ChooseSaveFile());

                if (!string.IsNullOrEmpty(folderPath))
                {
                    List<RecordedInfo<ObjectTransformToRecord>> recorded = 
                        new List<RecordedInfo<ObjectTransformToRecord>>() { };

                    List<RecordedInfo<RopeTransformToRecord>> recordedRopes = 
                        new List<RecordedInfo<RopeTransformToRecord>>() { };

                    foreach (RecordedObject recordedObject in recordedObjects) {
                        RecordedRope rr = recordedObject as RecordedRope;
                        if (rr) { recordedRopes.Add(rr.recordRope); }
                        else { recorded.Add(recordedObject.record); }
                    }

                    string recordedObjectsjsonString = JsonHelper.ToJson(recorded);
                    recordedObjectsjsonString = 
                        recordedObjectsjsonString.Replace("Items", "RecordedObjects");

                    string recordedRopesjsonString = JsonHelper.ToJson(recordedRopes);
                    recordedRopesjsonString =
                        recordedRopesjsonString.Replace("Items", "RecordedRopes");

                    recordedRopesjsonString = "," + recordedRopesjsonString.Substring(1);
                    recordedRopesjsonString =
                        recordedRopesjsonString.Remove(recordedRopesjsonString.Length - 1);

                    int lastIndex = recordedObjectsjsonString.Length - 1;

                    recordedObjectsjsonString = 
                        recordedObjectsjsonString.Insert(lastIndex, recordedRopesjsonString);

                    recordedObjectsjsonString = recordedObjectsjsonString.Insert(1,
                        "\"TIME\":[{ \"RecordingTime\":\"" + (int)recordingTime + "\"}],");

                    File.WriteAllText(folderPath, recordedObjectsjsonString);

                    ClearRecordData(true);

                    folderPath = null;
                    isSaving = false;
                }
            }

            else { Debug.Log("Can't save an empty recording"); }
        }
        private IEnumerator ChooseSaveFile()
        {
            yield return FileBrowser.WaitForSaveDialog
                (FileBrowser.PickMode.Files, false, null, null, "Save Files", "Save");

            if (FileBrowser.Success) {
                folderPath = FileBrowser.Result[0];
            }

            else { isSaving = false; }
        }
        IEnumerator ChooseSaveDirectory()
        {
            yield return FileBrowser.WaitForSaveDialog
                (FileBrowser.PickMode.Folders, false, null, null, "Save Files and Folders", "Save All");

            if (FileBrowser.Success) { folderPath = FileBrowser.Result[0]; }
        }
        public void SaveNewPosition()
        {
            if (selectedObject)
            {
                if (selectedObject.gameObject.GetComponent<RecordedObject>())
                {
                    selectedObject.GetComponent<RecordedObject>().SaveNewPosition(null);
                }
            }

            else { Debug.Log("None object Selected!"); }
        }
        public void SaveAllNewPositions() { StartCoroutine(SaveAllPositionsCoroutine()); }
        IEnumerator SaveAllPositionsCoroutine()
        {
            yield return StartCoroutine(ChooseSaveDirectory());

            for (int i = 0; i < savePositionObjects.Count; i++)
            {
                savePositionObjects[i].SaveNewPosition(folderPath);
                if (savePositionObjects[i].GetComponent<RecordedRope>()) { i += 2; }
            }
        }
        #endregion

        #region Load Functions
        private void Load()
        {
            if (!isSaving && !isLoading && !isRecording) {
                if (recordedFrames > 0 && !loadead) {
                    loadAlert.gameObject.SetActive(true);
                }
                else { 
                    StartCoroutine(LoadFromDirectory(true));
                }
            }

            else if (isSaving) { Debug.Log("Wait until the recording is save before loading a new one!"); }

            else if (isLoading) { Debug.Log("You already are loading a recording!"); }

            else if (isRecording) { Debug.Log("You can't load while recording!"); }
        }
        private IEnumerator LoadFromDirectory(bool confirm) 
        {
            loadAlert.gameObject.SetActive(false);

            if (confirm) {
                if (isReplaying) { Stop(); }

                isLoading = true;

                yield return StartCoroutine(ChooseLoadDirectory());

                if (!string.IsNullOrEmpty(folderPath)) {
                    string[] files = Directory.GetFiles(folderPath);
                    int numberOfFiles = files.Length;

                    if (numberOfFiles > 0) {
                        if (numberOfFiles <= 4) {
                            dropdown.template.anchoredPosition = new Vector2(0, 30 + numberOfFiles * 22.5f);
                            dropdown.template.sizeDelta = new Vector2(0, 30 + numberOfFiles * 30);
                        }

                        else {
                            dropdown.template.anchoredPosition = new Vector2(0, 120);
                            dropdown.template.sizeDelta = new Vector2(0, 97.5f);
                        }

                        dropdown.options.Clear();

                        string[] fileNames = new string[numberOfFiles];
                        for (int i = 0; i < numberOfFiles; i++) {
                            fileNames[i] = files[i].Replace(folderPath + "\\", "").Trim();

                            TMP_Dropdown.OptionData optionData =
                                new TMP_Dropdown.OptionData(fileNames[i]);

                            dropdown.options.Add(optionData);
                        }

                        dropdown.RefreshShownValue();
                        dropdown.interactable = true;

                        isLoading = false;

                        StartCoroutine(ChooseFile());
                    }

                    else { Debug.Log("No files Found"); }
                }
            }

            else {
                yield return StartCoroutine(SaveCoroutine());
                StartCoroutine(LoadFromDirectory(true));
            }
        }
        private IEnumerator ChooseFile() 
        {
            Stop();

            int dropdownIndex = dropdown.value;
            string selectedOption = dropdown.options[dropdownIndex].text;

            string path = folderPath + "\\" + selectedOption;

            ClearRecordData(true);

            string jsonString = File.ReadAllText(path);

            int indexOf = jsonString.IndexOf("RecordingTime") + 16;
            int lastIndexOf = jsonString.IndexOf("\"", indexOf);

            string time = jsonString.Substring(indexOf, lastIndexOf - indexOf);

            recordingTime = float.Parse(time);

            indexOf = jsonString.IndexOf("RecordedObjects")-1;
            lastIndexOf = jsonString.IndexOf("RecordedRopes")-2;

            string recordedObject = "{" + jsonString.Substring(indexOf, lastIndexOf - indexOf).Trim() + "}";
            recordedObject = recordedObject.Replace("RecordedObjects", "Items");

            List<RecordedInfo<ObjectTransformToRecord>> records = 
                JsonHelper.FromJson<RecordedInfo<ObjectTransformToRecord>>(recordedObject);

            indexOf = jsonString.IndexOf("RecordedRopes") - 1;
            lastIndexOf = jsonString.Length;

            string recordedRope = "{" + jsonString.Substring(indexOf, lastIndexOf - indexOf).Trim();
            recordedRope = recordedRope.Replace("RecordedRopes", "Items");

            List<RecordedInfo<RopeTransformToRecord>> recordsRopes =
                JsonHelper.FromJson<RecordedInfo<RopeTransformToRecord>>(recordedRope);

            int j=0,k=0; 
            if (recordedObjects.Count == (records.Count+recordsRopes.Count)) {
                for (int i = 0; i < recordedObjects.Count;i++) {
                    RecordedRope rr = recordedObjects[i] as RecordedRope;
                    if (rr && rr.rope.sourceBlueprint.name == recordsRopes[j].Name) {
                        j++;
                    }
                    else if (recordedObjects[i].name == records[k].Name) {
                        k++;
                    }
                }
            }

            if ((j+k) == recordedObjects.Count) {
                j = k = 0;
                for (int i = 0; i < recordedObjects.Count; i++) {
                    RecordedRope rr = recordedObjects[i] as RecordedRope;
                    if (rr && rr.rope.sourceBlueprint.name == recordsRopes[j].Name){
                        rr.recordRope = recordsRopes[j];
                        j++;
                    }
                    else if (recordedObjects[i].name == records[k].Name){
                        recordedObjects[i].record = records[k];
                        k++;
                    }
                }

                recordedFrames = 
                    recordedObjects[0].record.RecordObjectStore.Count;

                slider.value = 0;
                slider.maxValue = recordedFrames - 1;

                slideT = 0;

                ArrangeRopeIndexes(recordedFrames);

                PlayOrPause();

                float sliderValue = (int)slider.value;

                yield return new WaitUntil(() => slider.value == (sliderValue + multiplier));

                PlayOrPause();

                loadead = true;
            }

            else { Debug.Log("You trying to load a different recording setup!"); }
        }
        IEnumerator ChooseLoadDirectory()
        {
            yield return FileBrowser.WaitForLoadDialog
                (FileBrowser.PickMode.Folders, false, null, null, "Load Files and Folders", "Load All");

            if (FileBrowser.Success) {
                folderPath = FileBrowser.Result[0];
            }

            else { isLoading = false; }
        }
        public void LoadNewPosition()
        {
            if (selectedObject)
            {
                if (selectedObject.gameObject.GetComponent<RecordedObject>())
                {
                    selectedObject.GetComponent<RecordedObject>().LoadNewPosition(null);
                }
            }

            else { Debug.Log("None object Selected!"); }
        }
        public void LoadAllNewPositions() { StartCoroutine(LoadAllPositionsCoroutine()); }
        IEnumerator LoadAllPositionsCoroutine()
        {
            yield return StartCoroutine(ChooseLoadDirectory());

            for (int i = 0; i < savePositionObjects.Count; i++)
            {
                savePositionObjects[i].LoadNewPosition(folderPath);
                if (savePositionObjects[i].GetComponent<RecordedRope>() != null) { i += 2; }
            }
        }
        #endregion

        #region Support Functions
        private Transform CreatePrefabOnScene(string name, Transform parent, Vector3 pos = default(Vector3))
        {
            GameObject prefab = Resources.Load<GameObject>(name);

            prefab = Instantiate(prefab, pos, Quaternion.identity);

            RectTransform rt = prefab.GetComponent<RectTransform>();
            rt.SetParent(parent, false);

            return prefab.transform;
        }
        public Button CreateSavePositionButton(GameObject recordedObject, int offset)
        {
            offset++;

            contentPanel.GetComponent<RectTransform>().sizeDelta =
                new Vector2(713, 70) + new Vector2(0, offset * 70);

            Vector2 rectTransformOffset = new Vector2(360, -30);

            GameObject objectButton = Resources.Load<GameObject>("ObjectButton");
            GameObject newButton = Instantiate(objectButton, Vector3.zero, Quaternion.identity);
            newButton.transform.SetParent(contentPanel.transform, false);

            RectTransform rt = newButton.GetComponent<RectTransform>();
            rt.anchoredPosition = rectTransformOffset - new Vector2(0, offset * 70);
            rt.sizeDelta = new Vector2(700, 60);

            TextMeshProUGUI buttonText =
                newButton.GetComponentInChildren<TextMeshProUGUI>();

            string name = recordedObject.name;

            if (recordedObject.GetComponent<ObiActor>())
            {
                name = recordedObject.
                    GetComponent<ObiActor>().sourceBlueprint.name;
            }

            buttonText.text = name;
            newButton.name = name;

            return newButton.GetComponent<Button>();
        }
        public Button CreatePositionFunctionButtons(string name, int offset)
        {
            GameObject prefab = CreatePrefabOnScene(name, objectsUI.transform).gameObject;
            Button button = null;

            if (prefab)
            {
                button = prefab.GetComponent<Button>();
                button.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(-290 + (offset * (190)), -300);
            }

            return button;
        }
        private RecordedObject SetupRecordedObject(RecordedObject recordedObject, Button objectButton)
        {
            recordedObject.record.Name = recordedObject.gameObject.name;
            recordedObject.recordPosition.Name = recordedObject.gameObject.name;

            recordedObject.decimalPlaces = "F" + decimalPlaces;

            recordedObject.button = objectButton;

            return recordedObject;
        }
        private RecordedConnector SetupRecordedObject(RecordedConnector recordedObject,
            ObiRopeBase rope, RecordedConnector otherConnector, Button objectButton)
        {
            recordedObject.record.Name = recordedObject.gameObject.name;
            recordedObject.recordPosition.Name = recordedObject.gameObject.name;

            recordedObject.rope = rope;
            recordedObject.otherConnector = otherConnector;

            recordedObject.decimalPlaces = "F" + decimalPlaces;

            recordedObject.button = objectButton;

            return recordedObject;
        }
        private RecordedRope SetupRecordedObject(RecordedRope recordedObject, ObiRopeBase rope)
        {
            recordedObject.recordRope.Name = rope.sourceBlueprint.name;
            recordedObject.recordRopePosition.Name = rope.sourceBlueprint.name;

            recordedObject.rope = rope;

            recordedObject.decimalPlaces = "F" + decimalPlaces;

            return recordedObject;
        }
        private void ArrangeRopeIndexes(int lenght)
        {
            if (lenght > 0) {
                int rope = 0;
                foreach (RecordedObject recordedObject in recordedObjects) {
                    RecordedRope rr = recordedObject as RecordedRope;
                    if (rr && rope < ropesInitialIndexes.Count) {
                        ropesInitialIndexes[rope].array = new int[lenght];
                        for (int i = 0, j = 0; i < rr.recordRope.RecordObjectStore.Count && j < lenght; i++) {
                            if (rr.recordRope.RecordObjectStore[i].i) {
                                ropesInitialIndexes[rope].array[j] = i;
                                j++;
                            }
                        }
                        rope++;
                    }
                }
            }

            else {
                foreach (Matrix arrays in ropesInitialIndexes) {
                    arrays.array = new int[lenght];
                }
            }
        }
        private void FixTimer() 
        {
            string currentTime = timeStamp.text.Split("/")[1];
            currentTime = currentTime.Trim();

            string[] splitTime = currentTime.Split(":");

            int currentHour = int.Parse(splitTime[0]) * 3600;
            int currentMinutes = int.Parse(splitTime[1]) * 60;
            int currentSeconds = int.Parse(splitTime[2]);

            recordingTime = currentHour + currentMinutes + currentSeconds;
        }
        private void RefreshTimer()
        {
            float total = recordingTime;

            string totalHours = Mathf.Floor(total / 3600).ToString("00");
            string totalMinutes = Mathf.Floor(total / 60).ToString("00");
            string totalSeconds = (total % 60).ToString("00");


            float current = recordedFrames != 0 ? 
                (slider.value / recordedFrames) * recordingTime : 0.0f; 

            string currentHours = Mathf.Floor(current / 3600).ToString("00");
            string currentMinutes = Mathf.Floor(current / 60).ToString("00");
            string currentSeconds = (current % 60).ToString("00");

            timeStamp.text = currentHours + ":" + currentMinutes + ":" +
            currentSeconds + " / " + totalHours + ":"
            + totalMinutes + ":" + totalSeconds;
        }
        private void ClearRecordData(bool passThrough)
        {
            if (passThrough || (!isSaving && !isLoading && !isRecording))
            {
                slider.interactable = false;

                foreach (RecordedObject recordedObject in recordedObjects) {
                    recordedObject.record.RecordObjectStore.Clear();
                }

                ArrangeRopeIndexes(0);

                slider.value = 0;
                slider.maxValue = 0;

                slideT = 0;

                recordingTime = 0.0f;
                recordedFrames = 0;

                loadead = false;
            }

            else if (isSaving) { Debug.Log("You cannot clear the recording while saving it!"); }

            else if (isLoading) { Debug.Log("You cannot clear the recording while loading a new one!"); }

            else { Debug.Log("You cannot clear the recording until you stop recording it!"); }
        }
        private void PrepareToReplay()
        {
            GameObject rov = GameObject.FindGameObjectWithTag("ROV1");

            foreach (FlyingDroneScript fds in rov.GetComponentsInChildren<FlyingDroneScript>())
            {
                if (fds.enabled)
                {
                    rovItens.flyingDroneScripts.Add(fds);
                    fds.enabled = false;
                }
            }

            foreach (Rigmaster rmt in rov.GetComponentsInChildren<Rigmaster>())
            {
                if (rmt.enabled)
                {
                    rovItens.rigmasters.Add(rmt);
                    rmt.enabled = false;
                }
            }

            foreach (Titan4 t4 in rov.GetComponentsInChildren<Titan4>())
            {
                if (t4.enabled)
                {
                    rovItens.titan4s.Add(t4);
                    t4.enabled = false;
                }
            }

            foreach (Claw claw in rov.GetComponentsInChildren<Claw>())
            {
                if (claw.enabled)
                {
                    rovItens.claw.Add(claw);
                    claw.enabled = false;
                }
            }

            foreach (GenericMovement gm in rov.GetComponentsInChildren<GenericMovement>())
            {
                if (gm.enabled)
                {
                    rovItens.genericMovements.Add(gm);
                    gm.enabled = false;
                }
            }

            foreach (Animator anim in rov.GetComponentsInChildren<Animator>())
            {
                if (anim.enabled)
                {
                    rovItens.animators.Add(anim);
                    anim.enabled = false;
                }
            }

            foreach (ObiRigidbody obiRb in rov.GetComponentsInChildren<ObiRigidbody>())
            {
                if (obiRb.enabled)
                {
                    rovItens.obiRigidbodies.Add(obiRb);
                    obiRb.enabled = false;
                }
            }

            foreach (Rigidbody rb in rov.GetComponentsInChildren<Rigidbody>())
            {
                if (!rb.isKinematic)
                {
                    rovItens.rigidbodies.Add(rb);
                    rb.isKinematic = true;
                }
            }

            foreach (ObiCollider obiColl in rov.GetComponentsInChildren<ObiCollider>())
            {
                if (obiColl.enabled)
                {
                    rovItens.obiColliders.Add(obiColl);
                    obiColl.enabled = false;
                }
            }

            foreach (Collider coll in rov.GetComponentsInChildren<Collider>())
            {
                if (!coll.enabled)
                {
                    rovItens.colliders.Add(coll);
                    coll.enabled = false;
                }
            }

            preparedToReplay = true;
        }
        private void ReturnPhysics()
        {
            if (preparedToReplay)
            {
                if (isReplaying && !paused) { PlayOrPause(); }

                foreach (FlyingDroneScript fds in rovItens.flyingDroneScripts)
                {
                    fds.enabled = true;
                }
                foreach (Rigmaster rmt in rovItens.rigmasters)
                {
                    rmt.enabled = true;
                }
                foreach (Titan4 t4 in rovItens.titan4s)
                {
                    t4.enabled = true;
                }
                foreach (Claw claw in rovItens.claw)
                {
                    claw.enabled = true;
                }
                foreach (GenericMovement gm in rovItens.genericMovements)
                {
                    gm.enabled = true;
                }
                foreach (Animator anim in rovItens.animators)
                {
                    anim.enabled = true;
                }
                foreach (ObiRigidbody obiRb in rovItens.obiRigidbodies)
                {
                    obiRb.enabled = true;
                }
                foreach (Rigidbody rb in rovItens.rigidbodies)
                {
                    rb.isKinematic = false;
                }
                foreach (ObiCollider obiColl in rovItens.obiColliders)
                {
                    obiColl.enabled = true;
                }
                foreach (Collider coll in rovItens.colliders)
                {
                    coll.enabled = true;
                }

                int ropes = 0;
                foreach (RecordedRope recordRope in recordedObjects) {
                    if (recordRope.GetComponent<RecordedRope>() != null) {
                        int index = ropesInitialIndexes[ropes].array[slideT];
                        ropes++;

                        recordRope.index = index;
                        recordRope.LoadPositions(ref recordRope.recordRope, true);
                    }
                }

                slider.interactable = false;
                preparedToReplay = false;
                loadead = false;
            }
        }
        #endregion
    }
}

