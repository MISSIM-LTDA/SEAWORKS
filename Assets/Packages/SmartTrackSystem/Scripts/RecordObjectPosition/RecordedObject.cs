using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SmartTrackSystem
{
    public class RecordedObject : MonoBehaviour, ISelectable, IRecorded
    {
        //ISelectable
        public Camera mainCamera { get; set; }
        public OutlineEffect outEffect { get; set; }
        public EventSystem eventSystem { get; set; }
        public Button button { get; set; }
        public List<Outline> outlines { get; set; }
        public bool mouseOver { get; set; }
        public bool blinking { get; set; }
        public bool unselecting { get; set; }

        //IRecorded
        public bool saving { get; set; }
        public bool loading { get; set; }
        public string folderPath { get; set; }
        public int index { get; set; }
        public string decimalPlaces { get; set; }

        public RecordedInfo<ObjectTransformToRecord> record = 
            new RecordedInfo<ObjectTransformToRecord>("", new List<ObjectTransformToRecord>() { });
        public RecordedInfo<ObjectTransformToRecord> recordPosition = 
            new RecordedInfo<ObjectTransformToRecord>("", new List<ObjectTransformToRecord>() { });

        protected void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            outEffect = mainCamera.GetComponent<OutlineEffect>();

            eventSystem = FindObjectOfType<EventSystem>();

            decimalPlaces = "F" + SmartTrack.smartTrack.decimalPlaces;
        }
        public virtual void Update()
        {
            if (mouseOver && Input.GetMouseButtonDown(0)) { 
                SelectObject(gameObject); 
            }
        }

        #region Select
        public virtual void SelectObject(GameObject selectedObject)
        {
            SmartTrack.smartTrack.SelectedObject = selectedObject;
            
            if (blinking || unselecting) { return; }

            blinking = true;

            eventSystem.SetSelectedGameObject(button.gameObject);

            PlaceOutlineOnMesh(transform);

            SnapRopeButtonsPanel();
            StartCoroutine(BlinkColor(outEffect, outlines, 0, 10));

            if (!unselecting) { StartCoroutine(Unselect()); }
        }
        public void PlaceOutlineOnMesh(Transform meshTransform)
        {
            foreach (MeshRenderer meshRenderer in meshTransform.GetComponentsInChildren<MeshRenderer>())
            {
                Outline outline = meshRenderer.gameObject.GetComponent<Outline>();

                if (!outline && meshRenderer.enabled) { outline = meshRenderer.gameObject.AddComponent<Outline>(); }

                int i = 0;
                for (; i < outlines.Count; i++) { if (outline == outlines[i]) { break; } }
                if (i == outlines.Count) { outlines.Add(outline); }
            }
        }
        public void SnapRopeButtonsPanel()
        {
            Transform contentPanel = button.transform.parent;
            ScrollRect scrollRect = contentPanel.transform.parent.parent.GetComponent<ScrollRect>();

            Canvas.ForceUpdateCanvases();

            Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
            Vector2 childLocalPosition = button.transform.localPosition;
            Vector2 snapPosition = new Vector2(
                0 - (viewportLocalPosition.x + childLocalPosition.x),
                0 - (viewportLocalPosition.y + childLocalPosition.y)
            );

            contentPanel.transform.localPosition = snapPosition;
        }
        public IEnumerator BlinkColor(OutlineEffect outEffect, List<Outline> outlines, int color, int repeating)
        {
            Color c = GetColor(outEffect, color);
            Color buttonColor = button.gameObject.GetComponent<Image>().color;

            if (c.a == 1) { c.a = 0; }
            else { c.a = 1; }

            if (buttonColor.a == 1) { buttonColor.a = 0; }
            else { buttonColor.a = 1; }

            button.image.color = buttonColor;
            ChangeColor(outEffect, color, c);

            yield return new WaitForSeconds(0.1f);

            repeating--;

            if (repeating == 0)
            {
                blinking = false;
                button.image.color = new Color(0.8f, 0.8f, 0.8f, 1);
            }

            else { StartCoroutine(BlinkColor(outEffect, outlines, 0, repeating)); }
        }
        public Color GetColor(OutlineEffect outEffect, int color)
        {
            Color m_Color = Color.clear;

            switch (color)
            {
                case 0:
                    m_Color = outEffect.lineColor0;
                    break;
                case 1:
                    m_Color = outEffect.lineColor1;
                    break;
                case 2:
                    m_Color = outEffect.lineColor2;
                    break;
                case 3:
                    m_Color = outEffect.lineColor3;
                    break;
                case 4:
                    m_Color = outEffect.lineColor4;
                    break;
                default:
                    break;

            }

            return m_Color;
        }
        public void ChangeColor(OutlineEffect outEffect, int color, Color newColor)
        {
            switch (color)
            {
                case 0:
                    outEffect.lineColor0 = newColor;
                    break;
                case 1:
                    outEffect.lineColor1 = newColor;
                    break;
                case 2:
                    outEffect.lineColor2 = newColor;
                    break;
                case 3:
                    outEffect.lineColor3 = newColor;
                    break;
                case 4:
                    outEffect.lineColor4 = newColor;
                    break;
                default:
                    break;
            }

            outEffect.UpdateMaterialsPublicProperties();
        }
        public virtual IEnumerator Unselect()
        {
            unselecting = true;

            yield return new WaitUntil(() => Input.GetMouseButton(0) && !mouseOver && !DetectButtonOver());
            
            foreach (Outline outline in outlines) { Destroy(outline); }

            button.image.color = Color.cyan;

            unselecting = false;
        }
        public bool DetectButtonOver()
        {
            return eventSystem.currentSelectedGameObject == button.gameObject;
        }
        #endregion

        #region Save
        public void SaveNewPosition(string path)
        {
            if (SmartTrack.smartTrack.IsReplaying) { 
                Debug.Log("Cant't save a object position while SmartTrack is replaying"); 
                return; 
            }

            SaveOrLoadSetup();

            if (!saving)
            {
                StartCoroutine(SaveNewPositionCoroutine(path));
                saving = true;
            }

            else { Debug.Log(record.Name + " is already saving this object position"); }
        }
        public IEnumerator SaveNewPositionCoroutine(string path)
        {
            if (string.IsNullOrEmpty(path)) { 
                yield return StartCoroutine(ChooseDirectory()); 
            }

            else {
                path = CreateDirectoryToSaveAll(path);
                folderPath = FixFolderPath(path);
            }

            GetObjectPosition(ref recordPosition);

            if (recordPosition.RecordObjectStore.Count == 0) {
                Debug.Log("Problem Getting object position");
                saving = false;
            }

            else { SavePositions(); }
        }
        public virtual void GetObjectPosition(ref RecordedInfo<ObjectTransformToRecord> rec)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            rec.RecordObjectStore.Add(new ObjectTransformToRecord
                (gameObject.activeSelf,
                transform.localPosition.ToString(decimalPlaces, formatProvider),
                transform.localRotation.ToString(decimalPlaces, formatProvider)));
        }
        public void SavePositions()
        {
            string jsonString = JsonUtility.ToJson(recordPosition);
            File.WriteAllText(folderPath, jsonString);

            folderPath = null;
            saving = false;
        }

        #endregion

        #region Load Functions
        public void LoadNewPosition(string path)
        {
            SaveOrLoadSetup();

            if (!loading) {
                StartCoroutine(LoadNewPositionCoroutine(path));
                loading = true;
            }

            else { Debug.Log(record.Name + " is already loading this object position"); }
        }
        public virtual IEnumerator LoadNewPositionCoroutine(string path)
        {
            if (string.IsNullOrEmpty(path)) { yield return StartCoroutine(ChooseFile()); }

            else { folderPath = FindCorrectFileOnFolder(path); }

            yield return StartCoroutine(ReadPathFromFile());

            if (recordPosition.Name != gameObject.name) {
                Debug.Log("Tried to load a wrong file to this object");
                loading = false;
            }

            else{
                if (recordPosition.RecordObjectStore.Count == 0) {
                    Debug.Log("Problem Loading object position from File");
                    loading = false;
                }

                else { LoadPositions(ref recordPosition,true); }

                SmartTrack.smartTrack.SelectedObject = null;
            }
        }
        public virtual void LoadPositions(ref RecordedInfo<ObjectTransformToRecord> rec,bool makePhysics)
        {
            int i = 0;
            if (rec == record) {i = index;}

            gameObject.SetActive((rec.RecordObjectStore[i].e));

            SetLocalPositionAndRotation(transform,
                StringToVector3(rec.RecordObjectStore[i].p),
                StringToQuaternion(rec.RecordObjectStore[i].r));


            folderPath = null;
            loading = false;
        }

        #endregion

        #region Support Functions
        protected void SetLocalPositionAndRotation(Transform obj, 
            Vector3 position, Quaternion rotation)
        {
            obj.localPosition = position;
            obj.localRotation = rotation;
        }
        protected Vector3 StringToVector3(string position) 
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            string[] axis = position.Split(",");
        
            float x = float.Parse(axis[0].Replace("(", "").Trim(), formatProvider);
            float y = float.Parse(axis[1].Trim(), formatProvider);
            float z = float.Parse(axis[2].Replace(")", "").Trim(), formatProvider);

            return new Vector3(x, y, z);
        }
        protected Quaternion StringToQuaternion(string rotation) 
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            string[] axis = rotation.Split(",");

            float x = float.Parse(axis[0].Replace("(", "").Trim(), formatProvider);
            float y = float.Parse(axis[1].Trim(), formatProvider);
            float z = float.Parse(axis[2].Trim(), formatProvider);
            float w = float.Parse(axis[3].Replace(")", "").Trim(), formatProvider);

            return new Quaternion(x, y, z, w);
        }
        public virtual void SaveOrLoadSetup()
        {
            saving = false;
            loading = false;
            folderPath = null;

            recordPosition.RecordObjectStore.Clear();
        }
        public string CreateDirectoryToSaveAll(string path)
        {
            DateTime myTime = DateTime.Now;
            string time = myTime.ToString("HH:mm:ss");
            time = time.Replace(":", "");

            string newFolder = Path.Combine(path, "AllObjects" + time);

            if (!File.Exists(newFolder)) { Directory.CreateDirectory(newFolder); }

            return newFolder;
        }
        public virtual string FindCorrectFileOnFolder(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] fileInfos = info.GetFiles();

            foreach (FileInfo fileInfo in fileInfos){
                StreamReader sr = new StreamReader(fileInfo.FullName);
                string jsonstring = sr.ReadToEnd();
                sr.Close();

                record = JsonUtility.FromJson<RecordedInfo<ObjectTransformToRecord>>(jsonstring);
                if (record.Name == gameObject.name) {
                    path = fileInfo.FullName;
                    return path;
                }
            }

            return null;
        }
        public string FixFolderPath(string path)
        {
            path = path + @"\" + record.Name + ".txt";
            return path;
        }
        public virtual IEnumerator ReadPathFromFile()
        {
            if (File.Exists(folderPath)) {
                StreamReader sr = new StreamReader(folderPath);
                string jsonstring = sr.ReadToEnd();
                sr.Close();

                recordPosition = JsonUtility.FromJson<RecordedInfo<ObjectTransformToRecord>>(jsonstring);
            }

            yield return null;
        }
        public IEnumerator ChooseDirectory()
        {
            yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, record.Name, "Save Files and Folders", "Save");

            if (FileBrowser.Success) {
                folderPath = FileBrowser.Result[0];
            }
        }
        public IEnumerator ChooseFile()
        {
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Files and Folders", "Load");

            if (FileBrowser.Success){
                folderPath = FileBrowser.Result[0];
            }
        }
        #endregion
    }
}
