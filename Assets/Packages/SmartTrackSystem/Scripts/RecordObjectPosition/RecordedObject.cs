using Obi;
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
    public class RecordedObject : MonoBehaviour
    {
        [SerializeField, HideInInspector] public SmartTrack smartTrack;

        [SerializeField, HideInInspector] public ObiActor rope;
        [SerializeField, HideInInspector] public RecordedObject otherConnector;

        private bool connectedToRope;

        protected Camera mainCamera;
        protected OutlineEffect outEffect;
        protected EventSystem eventSystem;

        [SerializeField, HideInInspector] public Button button;

        protected List<Outline> outlines = new List<Outline>();

        private bool mouseOver;
        public bool MouseOver { 
            get { return mouseOver;}
            set { mouseOver = value; } 
        }

        protected bool blinking = false;
        protected bool unselecting = false;

        protected bool saving;
        protected bool loading;

        protected string folderPath;

        public RecordedObjectInfo record = new RecordedObjectInfo("", new List<ObjectTransformToRecord>() { });

        public RecordedObjectInfo recordPosition = new RecordedObjectInfo("", new List<ObjectTransformToRecord>() { });

        public int index;

        public string decimalPlaces;

        public Transform hitBody;
        protected virtual void Start()
        {
            if (rope != null) { connectedToRope = true; }

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            outEffect = mainCamera.GetComponent<OutlineEffect>();

            eventSystem = FindObjectOfType<EventSystem>();

            SmartTrack smartTrack = GameObject.FindObjectOfType<SmartTrack>();
            decimalPlaces = "F" + smartTrack.decimalPlaces;
        }
        public void Update()
        {
            if (mouseOver && Input.GetMouseButtonDown(0)) { 
                SelectObject(); 
            }
        }

        #region Select Functions
        //-------------------------------- Select Functions -----------------------------------------------//
        public void SelectObject()
        {
            if (connectedToRope)
            {
                smartTrack.SelectedObject = rope.gameObject;
            }
            else
            {
                smartTrack.SelectedObject = gameObject;
            }

            if (blinking || unselecting) { return; }

            blinking = true;

            eventSystem.SetSelectedGameObject(button.gameObject);

            if (connectedToRope)
            {
                PlaceOutlineOnMesh(rope.transform);
                PlaceOutlineOnMesh(otherConnector.transform);
            }

            PlaceOutlineOnMesh(transform);

            SnapRopeButtonsPanel();
            StartCoroutine(BlinkColor(outEffect, outlines, 0, 10, button));

            if (!unselecting) { StartCoroutine(Unselect()); }
        }
        private void PlaceOutlineOnMesh(Transform meshTransform)
        {
            foreach (MeshRenderer meshRenderer in meshTransform.GetComponentsInChildren<MeshRenderer>())
            {
                Outline outline = meshRenderer.gameObject.GetComponent<Outline>();

                if (outline == null && meshRenderer.enabled) { outline = meshRenderer.gameObject.AddComponent<Outline>(); }

                int i = 0;
                for (; i < outlines.Count; i++) { if (outline == outlines[i]) { break; } }
                if (i == outlines.Count) { outlines.Add(outline); }
            }
        }
        private void SnapRopeButtonsPanel()
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
        private IEnumerator BlinkColor(OutlineEffect outEffect, List<Outline> outlines, int color, int repeating, Button m_button)
        {
            Color c = GetColor(outEffect, color);
            Color buttonColor = m_button.gameObject.GetComponent<Image>().color;

            if (c.a == 1) { c.a = 0; }
            else { c.a = 1; }

            if (buttonColor.a == 1) { buttonColor.a = 0; }
            else { buttonColor.a = 1; }

            m_button.image.color = buttonColor;
            ChangeColor(outEffect, color, c);

            yield return new WaitForSeconds(0.1f);

            repeating--;

            if (repeating == 0)
            {
                blinking = false;
                m_button.image.color = new Color(0.8f, 0.8f, 0.8f, 1);
            }

            else { StartCoroutine(BlinkColor(outEffect, outlines, 0, repeating, m_button)); }
        }
        private Color GetColor(OutlineEffect outEffect, int color)
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
        private void ChangeColor(OutlineEffect outEffect, int color, Color newColor)
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
        private IEnumerator Unselect()
        {
            unselecting = true;

            if (connectedToRope)
            {
                yield return new WaitUntil(() => Input.GetMouseButton(0) && !mouseOver && !otherConnector.mouseOver && !DetectButtonOver());
            }
            else
            {
                yield return new WaitUntil(() => Input.GetMouseButton(0) && !mouseOver && !DetectButtonOver());
            }

            foreach (Outline outline in outlines) { Destroy(outline); }

            button.image.color = Color.cyan;

            unselecting = false;
        }
        private bool DetectButtonOver()
        {
            return eventSystem.currentSelectedGameObject == button.gameObject;
        }
        #endregion

        #region Save Functions
        //-------------------------------- Save Functions -----------------------------------------------//
        public void SaveNewPosition(string path)
        {
            if (smartTrack.IsReplaying) { 
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
        protected IEnumerator SaveNewPositionCoroutine(string path)
        {
            if (path == null) { yield return StartCoroutine(ChooseDirectory()); }

            else
            {
                path = CreateDirectoryToSaveAll(path);
                folderPath = FixFolderPath(path);
            }

            GetObjectPosition(ref recordPosition);

            if (recordPosition.RecordObjectStore.Count == 0)
            {
                Debug.Log("Problem Getting object position");
                saving = false;
            }

            else { SavePositions(); }
        }
        public virtual void GetObjectPosition(ref RecordedObjectInfo rec)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            if (transform.tag == "EFL_Parent")
            {
                Transform eflConnector1 = transform.GetChild(0);
                Transform eflConnector2 = transform.GetChild(1);

                rec.RecordObjectStore.Add(new ObjectTransformToRecord
                    (eflConnector1.gameObject.activeSelf, 
                    eflConnector1.localPosition.ToString(decimalPlaces, formatProvider),
                    eflConnector1.localRotation.ToString(decimalPlaces, formatProvider)));
                rec.RecordObjectStore.Add(new ObjectTransformToRecord
                    (eflConnector2.gameObject,
                    eflConnector2.localPosition.ToString(decimalPlaces, formatProvider),
                    eflConnector2.localRotation.ToString(decimalPlaces, formatProvider)));
            }

            else
            {
                rec.RecordObjectStore.Add(new ObjectTransformToRecord
                    (gameObject.activeSelf,
                    transform.localPosition.ToString(decimalPlaces, formatProvider),
                    transform.localRotation.ToString(decimalPlaces, formatProvider)));
            }
        }
        protected void SavePositions()
        {
            string jsonString = JsonUtility.ToJson(recordPosition);
            File.WriteAllText(folderPath, jsonString);

            folderPath = null;
            saving = false;
        }

        #endregion

        #region Load Functions
        //-------------------------------- Load Functions -----------------------------------------------//
        public void LoadNewPosition(string path)
        {
            SaveOrLoadSetup();

            if (!loading)
            {
                StartCoroutine(LoadNewPositionCoroutine(path));
                loading = true;
            }

            else { Debug.Log(record.Name + " is already loading this object position"); }
        }
        protected virtual IEnumerator LoadNewPositionCoroutine(string path)
        {
            if (path == null) { yield return StartCoroutine(ChooseFile()); }

            else { folderPath = FindCorrectFileOnFolder(path); }

            yield return StartCoroutine(ReadPathFromFile());

            if (recordPosition.Name != gameObject.name)
            {
                Debug.Log("Tried to load a wrong file to this object");
                loading = false;
            }

            else
            {
                if (recordPosition.RecordObjectStore.Count == 0)
                {
                    Debug.Log("Problem Loading object position from File");
                    loading = false;
                }

                else { LoadPositions(ref recordPosition,true); }
            }
        }
        public virtual void LoadPositions(ref RecordedObjectInfo rec,bool makePhysics)
        {
            int i = 0;

            if (rec == record) {
                Debug.Log("btuh");
                i = index;
                index++;
            }

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
        protected virtual void SaveOrLoadSetup()
        {
            saving = false;
            loading = false;
            folderPath = null;

            recordPosition.RecordObjectStore.Clear();
        }
        protected string CreateDirectoryToSaveAll(string path)
        {
            DateTime myTime = DateTime.Now;
            string time = myTime.ToString("HH:mm:ss");
            time = time.Replace(":", "");

            string newFolder = Path.Combine(path, "AllObjects" + time);

            if (!File.Exists(newFolder)) { Directory.CreateDirectory(newFolder); }

            return newFolder;
        }
        protected virtual string FindCorrectFileOnFolder(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] fileInfos = info.GetFiles();

            foreach (FileInfo fileInfo in fileInfos)
            {
                StreamReader sr = new StreamReader(fileInfo.FullName);
                string jsonstring = sr.ReadToEnd();
                sr.Close();

                record = JsonUtility.FromJson<RecordedObjectInfo>(jsonstring);
                if (record.Name == gameObject.name)
                {
                    path = fileInfo.FullName;
                    return path;
                }
            }

            return null;
        }
        protected string FixFolderPath(string path)
        {
            path = path + @"\" + record.Name + ".txt";
            Debug.Log(path);
            return path;
        }
        protected IEnumerator ReadPathFromFile()
        {
            if (File.Exists(folderPath))
            {
                StreamReader sr = new StreamReader(folderPath);
                string jsonstring = sr.ReadToEnd();
                sr.Close();

                recordPosition = JsonUtility.FromJson<RecordedObjectInfo>(jsonstring);
            }

            yield return null;
        }
        protected IEnumerator ChooseDirectory()
        {
            yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, null, record.Name, "Save Files and Folders", "Save");

            if (FileBrowser.Success)
            {
                folderPath = FileBrowser.Result[0];
            }
        }
        protected IEnumerator ChooseFile()
        {
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Files and Folders", "Load");

            if (FileBrowser.Success)
            {
                folderPath = FileBrowser.Result[0];
            }
        }
        #endregion
    }
}
