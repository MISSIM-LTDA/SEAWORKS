using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using rj.ghost.runtime;


namespace rj.ghost.editor
{
    public class GhostRecorder : MonoBehaviour
    {
        public List<ghostRecord> grr = new List<ghostRecord>();
        public List<ghostRecord> ghostR = new List<ghostRecord>();
        public List<GameObject> footsObj = new List<GameObject>();
        public List<int> footPos = new List<int>();
        private bool isR = true;
        [Tooltip("Set up Ghost object")]
        public GameObject myghost;
        [Tooltip("Show or hide Footprints")]
        public bool showFootprint = true;
        [Tooltip("Set up Footprint object")]
        public GameObject myFootprint;
        [Tooltip("Set Footprint Frequency")]
        public float FootprintRate;
        [Tooltip("Set up the slider console")]
        public GameObject SliderCon;
        private Transform footP;
        private int slideT;
        private int slideT2;
        private Slider mySlider;
        private bool isPlaying = false;
        private Text myTimestamp;
        private Button recordButton;
        private bool isRecording = false;
        public bool isGhost = false;
        public GameObject ghost;
        private int saveSlot;
        private string saveName;

        private GameObject footprint;
        // Start is called before the first frame update
        void Start()
        {

            //Set up all UI elements, if you don't need it, comment it out
            StartSetUp();
            //Create a parent to contain all instantiated footprints
            footP = new GameObject("foot prints parent").transform;
            if (!showFootprint) { footprint = null; } else { footprint = myFootprint; }
        }

        // Update is called once per frame
        void Update()
        {
            RefreshTimer();//update the timer every frame
                           //Cursor.lockState = CursorLockMode.None;  //Make the pointer show up
                           //Cursor.visible = true;

            ////The following code is for developer testing only
            //
            ////Press T to clear all slider consoles' data
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    print("Clear All data");
            //    ClearRecordData();
            //}
            ////Hold right button to rewind all slider consoles' ghost
            //if(Input.GetKeyDown(KeyCode.Mouse1))
            //{
            //    print("Play All Ghost");
            //    StartCoroutine("PlayGhost");
            //    isPlaying = true;
            //}
            ////release right button to stop rewind all slider consoles' ghost
            //if (Input.GetKeyUp(KeyCode.Mouse1))
            //{
            //    print("Stop All Replay"); 
            //    StopCoroutine("PlayGhost");
            //    isPlaying = false;
            //}
            ////Press R to record all Ghost recorders' position
            //if ( Input.GetKeyDown(KeyCode.R))
            //{
            //    print("Recording All position");
            //    RecordingPosition();
            //}
            ////Press P to Hide/Show all slider consoles
            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    ShowOrHideFootprints();
            //}
            ////The above code is for developer testing only
        }

        void StartSetUp()
        {
            //Setup all UI elements
            SliderCon = GameObject.Find("Player console"); 
            mySlider = SliderCon.GetComponentInChildren<Slider>();
            myTimestamp = SliderCon.transform.Find("TimeStamp").GetComponentInChildren<Text>();
            Button playButton = SliderCon.transform.Find("PlayButton").GetComponentInChildren<Button>();
            recordButton = SliderCon.transform.Find("RecordButton").GetComponentInChildren<Button>();
            Button clearButton = SliderCon.transform.Find("ClearButton").GetComponentInChildren<Button>();
            Button saveButton = SliderCon.transform.Find("SaveButton").GetComponentInChildren<Button>();
            Button loadButton = SliderCon.transform.Find("LoadButton").GetComponentInChildren<Button>();
            Button footprintButton = SliderCon.transform.Find("FootprintButton").GetComponentInChildren<Button>();
            Text myName = SliderCon.transform.Find("myName").GetComponentInChildren<Text>();
            myName.text = "ROV Log";//SliderCon.name;
            Dropdown saveDrop = SliderCon.transform.Find("SaveDrop").GetComponentInChildren<Dropdown>();

            recordButton.onClick.AddListener(RecordingPosition);
            playButton.onClick.AddListener(PlayOrPauseGhost);
            clearButton.onClick.AddListener(HideTheFootprints);
            clearButton.onClick.AddListener(ClearRecordData);
            footprintButton.onClick.AddListener(ShowOrHideFootprints);

            saveButton.onClick.AddListener(jsonSave);
            loadButton.onClick.AddListener(jsonLoad);

            mySlider.onValueChanged.AddListener(OnSliderValueChange);
            saveDrop.onValueChanged.AddListener(OnSaveDropdown);
        }
        //Clear all current recorded data, position, ghost and footsteps
        private void ClearRecordData()
        {
            ghostR.Clear();
            isGhost = false;
            for (int i = footsObj.Count - 1; i >= 0; i--)
            {
                GameObject FF = footsObj[i];
                Destroy(FF);
            }
            if (ghost) { ghost.SetActive(false); }
        }
        //Change the position of the ghost when the slider change the value
        public void OnSliderValueChange(float i)
        {
            slideT = (int)i;
            if (isGhost == true && ghostR[0] != null)
            {
                ghost.transform.position = ghostR[slideT].LR;
                ghost.transform.rotation = ghostR[slideT].LRR;
            }
        }
        //Play or Pause the slider play
        public void PlayOrPauseGhost()
        {
            if (isPlaying == false)
            {
                StartCoroutine("PlayGhost");
                isPlaying = true;
            }
            else
            {
                StopCoroutine("PlayGhost");
                isPlaying = false;
            }
        }
        //The following methods are exclusively for external access
        public void PlayTheGhost()
        {
            if (isPlaying == false)
            {
                StartCoroutine("PlayGhost");
                isPlaying = true;
            }
        }
        public void StopTheGhost()
        {
            if (isPlaying == true)
            {
                StopCoroutine("PlayGhost");
                isPlaying = false;
            }
        }
        public void StartRecording()
        {
            if (isRecording == false)
            {
                StartCoroutine("LeaveFoot");
                StartCoroutine("StartRecord");
                isRecording = true;
            }
        }
        public void StopRecording()
        {
            if (isRecording == true)
            {
                StopCoroutine("LeaveFoot");
                StopCoroutine("StartRecord");
                isRecording = false;
            }
        }
        public void ShowSliderConsole()
        {
            SliderCon.SetActive(true);
        }
        public void HideSliderConsole()
        {
            SliderCon.SetActive(false);
        }
        public void ShowTheFootprints()
        {
            footprint = myFootprint;
            showFootprint = true;
            ShowAllFoot();
        }
        public void HideTheFootprints()
        {
            showFootprint = false;
            for (int i = footsObj.Count - 1; i >= 0; i--)
            {
                GameObject FF = footsObj[i];
                Destroy(FF);
            }
        }
        public void ShowOrHideFootprints()
        {
            if (showFootprint == false)
            {
                ShowTheFootprints();
            }
            else
            {
                HideTheFootprints();
            }
        }
        public void ResetProgress()
        {
            mySlider.value = 0;
        }
        //The above methods are specifically for external access

        //Record or not record the position of object
        public void RecordingPosition()
        {
            if (isRecording == false)
            {
                //Change the button color to yellow when it's recording
                if (recordButton != null) { recordButton.GetComponent<Image>().color = Color.yellow; }
                StartCoroutine("LeaveFoot");
                StartCoroutine("StartRecord");
                isRecording = true;
            }
            else
            {
                //Change the button color to white when it's not recording
                if (recordButton != null) { recordButton.GetComponent<Image>().color = Color.white; }
                StopCoroutine("LeaveFoot");
                StopCoroutine("StartRecord");
                isRecording = false;
            }
        }
        //A Timer for the slider console
        void RefreshTimer()
        {
            if (mySlider != null)
            {
                mySlider.maxValue = ghostR.Count;//update the slider max value every frame
                float current = mySlider.value;
                float total = (ghostR.Count);
                string currentMinutes = Mathf.Floor(current / 60).ToString("00");
                string currentSeconds = (current % 60).ToString("00");
                string totalMinutes = Mathf.Floor(total / 60).ToString("00");
                string totalSeconds = (total % 60).ToString("00");
                myTimestamp.text = currentMinutes + ":" + currentSeconds + " / " + totalMinutes + ":" + totalSeconds;
            }
        }
        //Instantiate the ghost 
        public void CreateGhost()
        {
            if (isGhost != true && ghostR[0] != null)//Make sure the ghost only instantiate once
            {
                ghost = Instantiate(myghost, transform.position, transform.rotation) as GameObject;
                isGhost = true;
            }

            if (ghostR[0] != null)
            {
                ghost.SetActive(true);
            }
        }
        //Instantiate the footprints from loadded data
        private void ShowAllFoot()
        {

            //print(footprint.gameObject.name);
            for (int i = 0; i < footPos.Count; i++)
            {
                for (int k = 0; k < ghostR.Count; k++)
                {
                    if (footPos[i] == k)
                    {//print("ghostR " + ghostR.Count);
                        GameObject footPrints;
                        footPrints = Instantiate(footprint, ghostR[k].LR, transform.rotation) as GameObject;
                        footPrints.transform.SetParent(footP);
                        footsObj.Add(footPrints);
                        //print("foot " + footPos.Count);
                    }
                }
            }
        }
        //Select the save slot
        private void chooseSlot()
        {
            switch (saveSlot)
            {
                case 0:
                    saveName = "/SaveData/" + gameObject.name + "_JsonData1.txt";
                    break;
                case 1:
                    saveName = "/SaveData/" + gameObject.name + "JsonData2.txt";
                    break;
                case 2:
                    saveName = "/SaveData/" + gameObject.name + "JsonData3.txt";
                    break;
                case 3:
                    saveName = "/SaveData/" + gameObject.name + "JsonData4.txt";
                    break;
                case 4:
                    saveName = "/SaveData/" + gameObject.name + "JsonData5.txt";
                    break;
                case 5:
                    saveName = "/SaveData/" + gameObject.name + "JsonData6.txt";
                    break;
                case 6:
                    saveName = "/SaveData/" + gameObject.name + "JsonData7.txt";
                    break;
                case 7:
                    saveName = "/SaveData/" + gameObject.name + "JsonData8.txt";
                    break;
                case 8:
                    saveName = "/SaveData/" + gameObject.name + "JsonData9.txt";
                    break;
                case 9:
                    saveName = "/SaveData/" + gameObject.name + "JsonData10.txt";
                    break;
            }
        }
        //Change the slot when select different slot
        private void OnSaveDropdown(int d)
        {
            saveSlot = d;
        }
        //Store current data into the Save class
        Save createSave()
        {
            Save save = new Save();
            save.ghostRstore = ghostR;
            save.footPo = footPos;
            return save;
        }
        //Write current data to json file
        public void jsonSave()
        {
            chooseSlot();//Select the save slot
            Save save = createSave();
            string jsonString = JsonUtility.ToJson(save);
            //Create a folder if it does not exist
            if (!Directory.Exists(Application.streamingAssetsPath + "/SaveData"))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/SaveData");
                print("SaveData folder created");
            }
            //Create StreamWrite to create json file
            StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + saveName);
            sw.Write(jsonString);
            print("Write " + ghostR.Count);
            sw.Close();
            print("Use Json to save project successed");
        }
        //Read current data from json file
        public void jsonLoad()
        {
            ClearRecordData();
            isPlaying = false;
            isRecording = false;
            isGhost = false;
            mySlider.value = 0;
            chooseSlot();
            if (File.Exists(Application.streamingAssetsPath + saveName))
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + saveName);
                string JsonString = sr.ReadToEnd();
                sr.Close();
                Save save = JsonUtility.FromJson<Save>(JsonString);
                ghostR = save.ghostRstore;
                footPos = save.footPo;
                ShowAllFoot();
                print("Load " + ghostR.Count);
                print("Json Load successed");
            }
            else
            {
                print("Not found Json Save File");
            }
        }
        //Record current position data
        IEnumerator StartRecord()
        {
            while (isR)
            {
                ghostR.Add(new ghostRecord(transform.position, transform.rotation));
                yield return null;
            }
        }
        //Rewind the ghost, at the same time erase the footsteps that the ghost passed
        IEnumerator PlayGhost()
        {
            CreateGhost();//Only instantiate the ghost when rewind started
            for (int i = (int)mySlider.value; i < ghostR.Count; i++)
            {
                if (ghostR[i] != null && isPlaying == true)
                {
                    ghost.transform.position = ghostR[i].LR;
                    ghost.transform.rotation = ghostR[i].LRR;
                    slideT2 = i;
                    mySlider.value = slideT2; //The slider will follow the lead
                    if (i == ghostR.Count - 1) { isPlaying = false; }
                }
                yield return null;
            }
        }
        //Leave a footprint after the assigned time
        IEnumerator LeaveFoot()
        {
            while (isR)
            {
                if (showFootprint)
                {
                    GameObject footPrints;
                    footPrints = Instantiate(footprint, transform.position, transform.rotation) as GameObject;
                    footPrints.transform.SetParent(footP);
                    footsObj.Add(footPrints); //Record each footprint as a gameobject in a list for destroy
                }
                //Record the position of each footprint in a list for storage and reading
                footPos.Add(ghostR.Count);
                yield return new WaitForSeconds(FootprintRate);
            }
        }

    }
}
