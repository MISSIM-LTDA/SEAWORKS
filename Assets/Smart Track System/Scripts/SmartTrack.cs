using Obi;
using rj.ghost.runtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace SmartTrackSystem
{
    public class SmartTrack : MonoBehaviour
    {
        [Tooltip("Set up the objects that will be recorded")]
        public List<GameObject> objectsToRecord = new List<GameObject>() { };

        private GameObject rov;
        private GameObject[] rovComponents;

        //UI Elements
        private GameObject smartTrackUIConsole;
        private Slider slider;
        private Text timeStamp;
        private Button recordButton;

        public float recordRate = 0.0f;
        public int sliderValue
        {
            get { return (int)slider.value; }
            set { slider.value = value; }
        }

        private int slideT;
        private int slideT2;

        private int recordedFrames = 0;

        private bool isPlaying = false;
        public bool IsPlaying
        {
            get { return isPlaying; }
            set { isPlaying = value; }
        }

        private bool isRecording = false;

        int saveSlot;

        private void Start()
        {
            StartSetUp();
        }

        private void Update()
        {
            //RefreshTimer();
        }

        //Setup all gameObjects that will be recorded, also find and set all UI elements
        void StartSetUp()
        {
            //Find GameObjects in scene that will be recorded
            rov = GameObject.FindGameObjectWithTag("ROV1");
            rovComponents = GameObject.FindGameObjectsWithTag("ROVComponents");

            foreach (GameObject components in rovComponents)
            {
                if (components.activeSelf)
                    objectsToRecord.Add(components);
            }

            //Setup all GameObjects, Ropes and Rods with the necessary scripts to record their positions
            foreach (GameObject objectToRecord in objectsToRecord)
            {
                RecordMovement record;

                if (objectToRecord.GetComponent<RecordMovement>() == null)
                {
                    if (objectToRecord.GetComponent<ObiActor>() != null)
                        record = objectToRecord.AddComponent<RecordMovementParticle>();
                    
                    else
                        record = objectToRecord.AddComponent<RecordMovementObject>();
                }

                else
                    record = objectToRecord.GetComponent<RecordMovement>(); 

                record.playBackSystem = gameObject.GetComponent<SmartTrack>();
            }

            //Setup all UI elements
            smartTrackUIConsole = GameObject.FindGameObjectWithTag("SmartTrackConsole");

            slider = smartTrackUIConsole.GetComponentInChildren<Slider>();
            slider.onValueChanged.AddListener(OnSliderValueChange);

            timeStamp = smartTrackUIConsole.transform.Find("TimeStamp").GetComponentInChildren<Text>();

            recordButton = smartTrackUIConsole.transform.Find("RecordButton").GetComponentInChildren<Button>();
            recordButton.onClick.AddListener(RecordingPosition);

            Text myName = smartTrackUIConsole.transform.Find("myName").GetComponentInChildren<Text>();
            myName.text = "ROV Log";

            //Button playButton = smartTrackUIConsole.transform.Find("PlayButton").GetComponentInChildren<Button>();
            //playButton.onClick.AddListener(PlayOrPauseGhost);

            //Button clearButton = smartTrackUIConsole.transform.Find("ClearButton").GetComponentInChildren<Button>();
            //clearButton.onClick.AddListener(ClearRecordData);

            //Button saveButton = smartTrackUIConsole.transform.Find("SaveButton").GetComponentInChildren<Button>();
            //saveButton.onClick.AddListener(jsonSave);

            //Button loadButton = smartTrackUIConsole.transform.Find("LoadButton").GetComponentInChildren<Button>();
            //loadButton.onClick.AddListener(jsonLoad);

            Button showHideButton = smartTrackUIConsole.transform.parent.Find("showHide").GetComponentInChildren<Button>();
            showHideButton.onClick.AddListener(ShowHideConsole);

            //Dropdown saveDrop = smartTrackUIConsole.transform.Find("SaveDrop").GetComponentInChildren<Dropdown>();
            //saveDrop.onValueChanged.AddListener(OnSaveDropdown);
        }

        //Record or not record the positions of objects and ropes
        public void RecordingPosition()
        {
            if (isRecording == false)
            {
                //Change the button color to yellow when it's recording
                if (recordButton != null) { recordButton.GetComponent<Image>().color = Color.yellow; }
                foreach (GameObject recordObjects in objectsToRecord) 
                {
                    recordObjects.GetComponent<RecordMovement>().recordRate = recordRate;
                    StartCoroutine(recordObjects.GetComponent<RecordMovement>().StartRecord()); 
                }
                isRecording = true;
            }
            else
            {
                //Change the button color to white when it's not recording
                if (recordButton != null) { recordButton.GetComponent<Image>().color = Color.white; }
                foreach (GameObject recordObjects in objectsToRecord) { StopCoroutine(recordObjects.GetComponent<RecordMovement>().StartRecord()); }
                isRecording = false;
            }
        }
        public void PlayOrPauseGhost()
        {
            if (isPlaying == false)
            {
                foreach (GameObject recordObjects in objectsToRecord)
                    if (recordObjects.GetComponent<RecordMovementObject>() != null)
                        recordObjects.GetComponent<RecordMovementObject>().CreateReplayObject();

                foreach (GameObject recordObjects in objectsToRecord)
                    StartCoroutine(recordObjects.GetComponent<RecordMovementObject>().PlayGhost());

                StartCoroutine(PlayGhost());
                isPlaying = true;
            }
            else
            {
                foreach (GameObject recordObjects in objectsToRecord)
                    StopCoroutine(recordObjects.GetComponent<RecordMovementObject>().PlayGhost());

                StopCoroutine(PlayGhost());
                isPlaying = false;
            }
        }
        IEnumerator PlayGhost()
        {
            for (int i = sliderValue; i < recordedFrames; i++)
            {
                if (isPlaying == true)
                {
                    slideT2 = i;
                    slider.value = slideT2;
                    if (i == recordedFrames - 1) { isPlaying = false; }
                }
                yield return null;
            }
        }
        private void ClearRecordData()
        {
            foreach (GameObject recordObjects in objectsToRecord) 
            {
                recordObjects.GetComponent<RecordMovement>().ghostR.Clear();
                if(recordObjects.GetComponent<RecordMovementObject>() != null) { recordObjects.GetComponent<RecordMovementObject>().isReplayObject = false; }
                recordObjects.GetComponent<RecordMovement>().ghost.SetActive(false);
            }

        }
        private string chooseSlot(GameObject recordedObject)
        {
           return "/SaveData/" + recordedObject.name + "_JsonData" + saveSlot + ".txt";
        }
        public void jsonSave()
        {
            //Create a folder if it does not exist
            if (!Directory.Exists(Application.streamingAssetsPath + "/SaveData"))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/SaveData");
                print("SaveData folder created");
            }

            foreach (GameObject recordObjects in objectsToRecord) 
            {
                recordObjects.GetComponent<RecordMovement>().saveName = chooseSlot(recordObjects);
                recordObjects.GetComponent<RecordMovement>().jsonSave();
            } 

        }
        public void jsonLoad()
        {
            ClearRecordData();
            isPlaying = false;
            isRecording = false;
            sliderValue = 0;

            foreach (GameObject recordObjects in objectsToRecord) 
            {
                recordObjects.GetComponent<RecordMovement>().saveName = chooseSlot(recordObjects);
                recordObjects.GetComponent<RecordMovement>().jsonLoad();
            }
               
        }
        private void OnSaveDropdown(int d) { saveSlot = d;}
        void RefreshTimer()
        {
            if (slider != null)
            {
                slider.maxValue = recordedFrames;//update the slider max value every frame
                float current = sliderValue;
                float total = (recordedFrames);
                string currentMinutes = Mathf.Floor(current / 60).ToString("00");
                string currentSeconds = (current % 60).ToString("00");
                string totalMinutes = Mathf.Floor(total / 60).ToString("00");
                string totalSeconds = (total % 60).ToString("00");
                timeStamp.text = currentMinutes + ":" + currentSeconds + " / " + totalMinutes + ":" + totalSeconds;
            }
        }
        public void ResetProgress()
        {
            sliderValue = 0;
        }
        public void ShowHideConsole() {smartTrackUIConsole.SetActive(!smartTrackUIConsole.activeSelf);}
        public void OnSliderValueChange(float i)
        {
            slideT = (int)i;

            foreach (GameObject recordObjects in objectsToRecord) 
            {
                if (recordObjects.GetComponent<RecordMovement>().ghostR[0] != null) 
                {
                    if (recordObjects.GetComponent<RecordMovementObject>() != null) 
                    {
                        if (recordObjects.GetComponent<RecordMovementObject>().isReplayObject == true)
                        {
                            recordObjects.GetComponent<RecordMovementObject>().ghost.transform.position = recordObjects.GetComponent<RecordMovementObject>().ghostR[slideT].LR;
                            recordObjects.GetComponent<RecordMovementObject>().ghost.transform.rotation = recordObjects.GetComponent<RecordMovementObject>().ghostR[slideT].LRR;
                        }
                    }

                    else 
                    {
                        recordObjects.GetComponent<RecordMovementObject>().ghost.transform.position = recordObjects.GetComponent<RecordMovementObject>().ghostR[slideT].LR;
                        recordObjects.GetComponent<RecordMovementObject>().ghost.transform.rotation = recordObjects.GetComponent<RecordMovementObject>().ghostR[slideT].LRR;
                    }
                }
            }
        }
    }
}

