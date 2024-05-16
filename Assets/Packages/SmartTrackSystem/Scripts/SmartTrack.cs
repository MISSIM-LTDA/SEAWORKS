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
        [SerializeField] private List<GameObject> gameObjectsToRecord = new List<GameObject>() { };
        [SerializeField, HideInInspector] private List<RecordedObject> objectsToRecord = new List<RecordedObject>() { };

        [Serializable]
        public class RovItens 
        {
            public List<FlyingDroneScript> flyingDroneScripts = new List<FlyingDroneScript>();
            public List<Rigmaster> rigmasters = new List<Rigmaster>();
            public List<Titan4> titan4s = new List<Titan4>();
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

        //UI Elements
        [SerializeField, HideInInspector] private Transform smartTrackUIConsole;
        private Transform console;
        private Transform loadAlert;

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

        Coroutine replayCoroutine;
        private bool preparedToReplay;

        private bool isRecording;

        [SerializeField, HideInInspector] private bool createdSetup;

        private string folderPath;

        private bool isReplaying;

        private bool isSaving;

        private bool isLoading;
        private bool loadead;
        private void Start()
        {
            if (Application.isPlaying && createdSetup)
            {
                FileBrowser.SetFilters
                    (false, new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

                FileBrowser.SetDefaultFilter(".txt");
                FileBrowser.AddQuickLink("Users", "C:\\Users", null);

                SetupUIButtons();
            }
        }
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) { HideConsole(); }

            if (isRecording) { recordingTime += Time.deltaTime; }

            RefreshTimer();
        }

        #region Setup Functions
        //Setup all gameObjects that will be recorded, also find and set all UI elements
        public void CreateSetUp()
        {
            //ClearSetup(false);

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("XLX")){
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("ROVComponents")) {
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("Jaw7finger")){
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (GameObject rovComponent in GameObject.FindGameObjectsWithTag("Interectable")){
                gameObjectsToRecord.Add(rovComponent);
            }

            foreach (ObiActor ropes in GameObject.FindObjectsOfType<ObiActor>()){
                gameObjectsToRecord.Add(ropes.gameObject);
            }

            foreach (GameObject objectToRecord in gameObjectsToRecord) 
            {
                ObiActor rope = objectToRecord.GetComponent<ObiActor>();

                if (rope != null) 
                {
                    RecordedRope rr = objectToRecord.GetComponent<RecordedRope>();
                    if (rr == null){
                        rr = objectToRecord.gameObject.AddComponent<RecordedRope>();
                    }

                    List<ObiParticleGroup> groups = rope.sourceBlueprint.groups;

                    Transform startOfRope = null;
                    Transform endOfRope = null;

                    foreach (ObiParticleAttachment attach in rope.GetComponents<ObiParticleAttachment>())
                    {
                        if (attach.particleGroup == groups[0] && attach.target != rr.transform)
                        {
                            if (attach.target.name == "Cont (1)") { startOfRope = attach.target.parent; }
                            else { startOfRope = attach.target; }
                        }
                        else if (attach.particleGroup == groups[groups.Count - 1] && attach.target != rr.transform)
                        {
                            if (attach.target.name == "Cont (1)") { endOfRope = attach.target.parent; }
                            else { endOfRope = attach.target; }
                        }
                    }

                    rr.startOfRope = startOfRope;
                    rr.endOfRope = endOfRope;

                    objectsToRecord.Add(SetupRecordedObject(rr, rope, null));
                    rr.record.Name = rr.rope.sourceBlueprint.name;
                }

                else 
                {
                    RecordedObject rO = objectToRecord.GetComponent<RecordedObject>();
                    if (rO == null){
                        rO = objectToRecord.AddComponent<RecordedObject>();
                    }

                    objectsToRecord.Add(SetupRecordedObject(rO, null, null));
                    rO.record.Name = rO.name;
                }
            }

            foreach(RecordedObject recordedObject in objectsToRecord) 
            {
                if(recordedObject.GetComponent<RecordedRope>() != null) {
                    ropesInitialIndexes.Add(new Matrix(0));
                }
            }

            //Setup all UI elements
            Transform canvas = GameObject.Find("Canvas Display 1").transform;
            smartTrackUIConsole = CreatePrefabOnScene("SmartTrackConsole", canvas, new Vector3(0, 540, 0)).transform;

            createdSetup = true;

            Debug.Log("Setup Created");
        }
        public void ClearSetup(bool debug)
        {
            gameObjectsToRecord.Clear();

            foreach(RecordedObject recordedObject in objectsToRecord){
                DestroyImmediate(recordedObject);
            }
            objectsToRecord.Clear();

            ropesInitialIndexes.Clear();

            if (smartTrackUIConsole) {
                DestroyImmediate(smartTrackUIConsole.gameObject);
            }

            createdSetup = false;

            if (debug) { Debug.Log("Setup Cleaned"); }
        }
        public void SetupUIButtons() 
        {
            console = smartTrackUIConsole.Find("Console");
            loadAlert = console.Find("Load Alert");

            Button hideConsoleButton = smartTrackUIConsole.Find("ShowHide").GetComponentInChildren<Button>();
            hideConsoleButton.onClick.AddListener(HideConsole);

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
            confirmLoad.onClick.AddListener(delegate { StartCoroutine(ConfirmLoad(true));});

            Button denyLoad = loadAlert.Find("DenyButton").GetComponentInChildren<Button>();
            denyLoad.onClick.AddListener(delegate { StartCoroutine(ConfirmLoad(false));});
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
            if (!preparedToReplay) 
            {
                if (!isRecording)
                {
                    recordButton.image.sprite = recordOnSprite;
                    isRecording = true;

                    if (recordedFrames > 0)
                    {
                        FixTimer();

                        int index = (int)slider.value;
                        int count = recordedFrames - index;
                        int rope = 0;

                        foreach (RecordedObject recordedObject in objectsToRecord)
                        {
                            RecordedRope rr = recordedObject.GetComponent<RecordedRope>();
                            if (rr != null) {
                                int ropeIndex = ropesInitialIndexes[rope].array[index]; 
                                int ropeCount = rr.record.RecordObjectStore.Count - ropeIndex;

                                rr.record.RecordObjectStore.RemoveRange(ropeIndex,ropeCount);
                            }
                            else {
                                recordedObject.record.RecordObjectStore.RemoveRange(index, count);
                            }

                            slider.value = 0;
                            slider.maxValue = count;
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
                    ArrangeRopeIndexes(recordedFrames);
                }
            }

            else { Debug.Log("You only can record a physic ROV!"); }
        }
        public IEnumerator Recording() 
        {
            while (isRecording)
            {
                foreach (RecordedObject recordedObject in objectsToRecord) {
                    recordedObject.GetObjectPosition();
                }

                recordedFrames = objectsToRecord[0].record.RecordObjectStore.Count;

                yield return new WaitForSeconds(recordingRate);
            }
        }
        #endregion

        #region Replay Functions
        private void PlayOrPause() 
        {
            if (!isSaving && !isLoading && !isRecording && recordedFrames>0) 
            {
                if (playButton.image.sprite == playSprite)
                {
                    if(slider.value == slider.maxValue) { slider.value = 0; }

                    playButton.image.sprite = pauseSprite;
                    replayCoroutine = StartCoroutine(Play());
                }

                else
                {
                    playButton.image.sprite = playSprite;
                    StopCoroutine(replayCoroutine);
                    isReplaying = false;
                }
            }

            else if(recordedFrames == 0) { Debug.Log("You are trying to replay an empty recording!"); }

            else if (isSaving) { Debug.Log("You are saving a recording you can't replayed yet!"); }

            else if (isLoading) { Debug.Log("You are loading a recording you can't replayed yet!"); }

            else if (isRecording) { Debug.Log("You are recording a recording you can't replayed yet!"); }
        }
        private IEnumerator Play() 
        {
            if (!preparedToReplay) { PrepareToReplay(); }

            isReplaying = true;

            slider.interactable = true;
            for (slider.value = (int) slideT; slider.value < slider.maxValue; slider.value+=multiplier) {

                yield return new WaitForSeconds(recordingTime / recordedFrames);
            }

            PlayOrPause();
        }
        private void Stop() 
        {
            StopCoroutine(replayCoroutine);
            playButton.image.sprite = playSprite;

            slider.value = 0;

            isReplaying = false;
        }
        private void OnSliderValueChange(float frame)
        {
            if (preparedToReplay) 
            {
                slideT = (int)frame;

                int ropes = 0;
                foreach (RecordedObject recordObject in objectsToRecord)
                {
                    int index = slideT;

                    if (recordObject.GetComponent<RecordedRope>() != null){
                        index = ropesInitialIndexes[ropes].array[index];
                        ropes++;
                    }

                    recordObject.index = index;
                    recordObject.LoadPositions(false);
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
            if (!isSaving && !isLoading && !isRecording) {
                StartCoroutine(SaveCoroutine());
            }

            else if (isSaving) { Debug.Log("You already are saving an recording!"); }

            else if (isLoading) { Debug.Log("You can't save while you load a recording!"); }

            else if (isRecording) { Debug.Log("You can't save while recording!"); }
        }
        private IEnumerator SaveCoroutine()
        {
            if (objectsToRecord.Count > 0 && recordedFrames > 0)
            {
                isSaving = true;

                objectsToRecord[0].record.ThisIsRecordedObjectData = (int)recordingTime;

                yield return StartCoroutine(ChooseSaveDirectory());

                if(folderPath != null) 
                {
                    List<RecordedObjectInfo> records = new List<RecordedObjectInfo>() { };
                    foreach(RecordedObject recordedObject in objectsToRecord) {
                        records.Add(recordedObject.record);
                    }

                    string jsonString = JsonHelper.ToJson(records);
                    File.WriteAllText(folderPath, jsonString);

                    ClearRecordData(true);

                    folderPath = null;
                    isSaving = false;
                }
            }

            else { Debug.Log("Can't save an empty recording"); }
        }
        private IEnumerator ChooseSaveDirectory()
        {
            yield return FileBrowser.WaitForSaveDialog
                (FileBrowser.PickMode.Files, false, null, null, "Save Files", "Save");

            if (FileBrowser.Success){
                folderPath = FileBrowser.Result[0];
            }
        }

        #endregion

        #region Load Functions
        private void Load() 
        {
            if (!isSaving && !isLoading && !isRecording){
                if (recordedFrames > 0 && !loadead){
                    loadAlert.gameObject.SetActive(true);
                }
                else { StartCoroutine(ConfirmLoad(true)); }
            }

            else if (isSaving) { Debug.Log("Wait until the recording is save before loading a new one!"); }

            else if (isLoading) { Debug.Log("You already are loading a recording!"); }

            else if (isRecording) { Debug.Log("You can't load while recording!"); }
        }
        private IEnumerator ConfirmLoad(bool confirm) 
        {
            loadAlert.gameObject.SetActive(false);

            if (confirm)
            {
                isLoading = true;

                ClearRecordData(true);

                yield return StartCoroutine(ChooseLoadFile());

                if (folderPath != null)
                {
                    string jsonString = File.ReadAllText(folderPath);

                    List<RecordedObjectInfo> records = new List<RecordedObjectInfo>() { };
                    records = JsonHelper.FromJson<RecordedObjectInfo>(jsonString);

                    int i = 0;
                    if (objectsToRecord.Count == records.Count) {
                        for (int j = 0; j < objectsToRecord.Count; j++){
                            RecordedRope rr = objectsToRecord[j].GetComponent<RecordedRope>();
                            if(rr != null && objectsToRecord[j].rope.sourceBlueprint.name 
                                == records[j].Name) { i++; }
                            else if (objectsToRecord[j].name == records[j].Name){
                                i++;
                            }
                        }
                    }

                    if (i == objectsToRecord.Count){
                        for (int j = 0; j < objectsToRecord.Count; j++){
                            objectsToRecord[j].record = records[j];
                        }

                        recordingTime = objectsToRecord[0].record.ThisIsRecordedObjectData;
                        recordedFrames = objectsToRecord[0].record.RecordObjectStore.Count;

                        slider.value = 0;
                        slider.maxValue = recordedFrames - 1;
                        ArrangeRopeIndexes(recordedFrames);

                        loadead = true;
                    }

                    else {Debug.Log("You trying to load a different recording setup!");}

                    folderPath = null;
                    isLoading = false;
                }
            }

            else
            {
                yield return StartCoroutine(SaveCoroutine());
                StartCoroutine(ConfirmLoad(true));
            }
        }
        protected IEnumerator ChooseLoadFile()
        {
            yield return FileBrowser.WaitForLoadDialog
                (FileBrowser.PickMode.Files, false, null, null, "Load File", "Load");

            if (FileBrowser.Success){
                folderPath = FileBrowser.Result[0];
            }
        }

        #endregion

        #region Support Functions
        private GameObject CreatePrefabOnScene(string name,Transform parent,Vector3 pos)
        {
            GameObject prefab = Resources.Load<GameObject>(name);

            prefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);

            RectTransform rt = prefab.GetComponent<RectTransform>();
            rt.SetParent(parent);

            rt.anchoredPosition = pos;

            return prefab;
        }
        private RecordedObject SetupRecordedObject(RecordedObject recordedObject, ObiActor rope, RecordedObject otherConnector)
        {
            recordedObject.rope = rope;
            recordedObject.otherConnector = otherConnector;

            return recordedObject;
        }
        private void ArrangeRopeIndexes(int lenght) 
        {
            if (lenght > 0) {
                int rope = 0;
                foreach (RecordedObject recordedObject in objectsToRecord){
                    RecordedRope rr = recordedObject.GetComponent<RecordedRope>();
                    if (rr != null){
                        ropesInitialIndexes[rope].array = new int[lenght];
                        for (int i = 0,j=0; i < rr.record.RecordObjectStore.Count; i++){
                            if (rr.record.RecordObjectStore[i].initialIndex){
                                ropesInitialIndexes[rope].array[j] = i;
                                j++;
                            }
                        }
                        rope++;
                    }
                }
            }

            else {
                foreach(Matrix arrays in ropesInitialIndexes) { 
                    arrays.array = new int[lenght]; 
                } 
            }
        }
        private void FixTimer() 
        {
            string currentTime = timeStamp.text.Split("/")[0];
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

            if (recordedFrames != 0) {
                float current = (slider.value / recordedFrames) * recordingTime ;

                string currentHours = Mathf.Floor(current / 3600).ToString("00");
                string currentMinutes = Mathf.Floor(current / 60).ToString("00");
                string currentSeconds = (current % 60).ToString("00");

                timeStamp.text = currentHours + ":" + currentMinutes + ":" +
                currentSeconds + " / " + totalHours + ":"
                + totalMinutes + ":" + totalSeconds;
            }

            else {
                timeStamp.text = "00:00:00 / " + totalHours + ":"
                + totalMinutes + ":" + totalSeconds;
            }
        }
        private void ClearRecordData(bool passThrough) 
        {
            if (passThrough || (!isSaving && !isLoading && !isRecording)) 
            {
                slider.interactable = false;

                foreach (RecordedObject recordedObject in objectsToRecord)
                {
                    recordedObject.record.ThisIsRecordedObjectData = 0;
                    recordedObject.record.RecordObjectStore.Clear();
                }

                ArrangeRopeIndexes(0);

                slider.value = 0;
                slider.maxValue = 0;

                recordingTime = 0.0f;
                recordedFrames = 0;

                loadead = false;
            }

            else if(isSaving) { Debug.Log("You cannot clear the recording while saving it!"); }

            else if(isLoading) { Debug.Log("You cannot clear the recording while loading a new one!"); }

            else { Debug.Log("You cannot clear the recording until you stop recording it!"); }
        }
        private void PrepareToReplay()
        {
            GameObject rov = GameObject.FindGameObjectWithTag("ROV1");

            foreach(FlyingDroneScript fds in rov.GetComponentsInChildren<FlyingDroneScript>()) {
                if (fds.enabled){
                    rovItens.flyingDroneScripts.Add(fds);
                    fds.enabled = false;
                }
            }

            foreach (Rigmaster rmt in rov.GetComponentsInChildren<Rigmaster>()) {
                if (rmt.enabled){
                    rovItens.rigmasters.Add(rmt);
                    rmt.enabled = false;
                }
            }

            foreach (Titan4 t4 in rov.GetComponentsInChildren<Titan4>()) {
                if (t4.enabled){
                    rovItens.titan4s.Add(t4);
                    t4.enabled = false;
                }
            }

            foreach (GenericMovement gm in rov.GetComponentsInChildren<GenericMovement>()) {
                if (gm.enabled){
                    rovItens.genericMovements.Add(gm);
                    gm.enabled = false;
                }
            }

            foreach (Animator anim in rov.GetComponentsInChildren<Animator>()) {
                if (anim.enabled){
                    rovItens.animators.Add(anim);
                    anim.enabled = false;
                }
            }

            foreach (ObiRigidbody obiRb in rov.GetComponentsInChildren<ObiRigidbody>()) {
                if (obiRb.enabled){
                    rovItens.obiRigidbodies.Add(obiRb);
                    obiRb.enabled = false;
                }
            }

            foreach (Rigidbody rb in rov.GetComponentsInChildren<Rigidbody>()) {
                if (!rb.isKinematic){
                    rovItens.rigidbodies.Add(rb);
                    rb.isKinematic = true;
                }
            }

            foreach (ObiCollider obiColl in rov.GetComponentsInChildren<ObiCollider>()) {
                if (obiColl.enabled){
                    rovItens.obiColliders.Add(obiColl);
                    obiColl.enabled = false;
                }
            }

            foreach (Collider coll in rov.GetComponentsInChildren<Collider>()) {
                if (!coll.enabled){
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
                if (isReplaying) { PlayOrPause(); }

                foreach (FlyingDroneScript fds in rovItens.flyingDroneScripts){
                    fds.enabled = true;
                }
                foreach (Rigmaster rmt in rovItens.rigmasters){
                    rmt.enabled = true;
                }
                foreach (Titan4 t4 in rovItens.titan4s){
                    t4.enabled = true;
                }
                foreach (GenericMovement gm in rovItens.genericMovements){
                    gm.enabled = true;
                }
                foreach (Animator anim in rovItens.animators){
                    anim.enabled = true;
                }
                foreach (ObiRigidbody obiRb in rovItens.obiRigidbodies){
                    obiRb.enabled = true;
                }
                foreach (Rigidbody rb in rovItens.rigidbodies){
                    rb.isKinematic = false;
                }
                foreach (ObiCollider obiColl in rovItens.obiColliders){
                    obiColl.enabled = true;
                }
                foreach (Collider coll in rovItens.colliders){
                    coll.enabled = true;
                }

                int ropes = 0;
                foreach (RecordedObject recordObject in objectsToRecord){
                    if (recordObject.GetComponent<RecordedRope>() != null){
                        int index = ropesInitialIndexes[ropes].array[slideT];
                        ropes++;

                        recordObject.index = index;
                        recordObject.LoadPositions(true);
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

