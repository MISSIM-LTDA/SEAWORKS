using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using rj.ghost.runtime;

namespace SmartTrackSystem 
{
    public abstract class RecordMovement : MonoBehaviour
    {
        public List<ghostRecord> grr = new List<ghostRecord>();
        public List<ghostRecord> ghostR = new List<ghostRecord>();

        protected bool isR = true;

        public GameObject ghost;

        protected SmartTrack m_playBackSystem;
        public SmartTrack playBackSystem
        {
            get { return m_playBackSystem; }
            set { m_playBackSystem = value; }
        }

        protected GameObject m_recordObject;
        public GameObject recordObject
        {
            get { return m_recordObject; }
            set { m_recordObject = value; }
        }

        private float m_recordRate = 0.0f;
        public float recordRate 
        {
            get { return m_recordRate; }
            set { m_recordRate = value; }
        }

        public string saveName;
        public abstract void Init();
        public abstract IEnumerator StartRecord();
        public abstract IEnumerator PlayGhost();
        Save createSave()
        {
            Save save = new Save();
            save.ghostRstore = ghostR;
            return save;
        }
        public void jsonSave()
        {
            Save save = createSave();
            string jsonString = JsonUtility.ToJson(save);

            //Create StreamWrite to create json file
            StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + saveName);
            sw.Write(jsonString);
            print("Write " + ghostR.Count);
            sw.Close();
            print("Use Json to save project successed");
        }
        public void jsonLoad()
        {
            if (File.Exists(Application.streamingAssetsPath + saveName))
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + saveName);
                string JsonString = sr.ReadToEnd();
                sr.Close();
                Save save = JsonUtility.FromJson<Save>(JsonString);
                ghostR = save.ghostRstore;
                print("Load " + ghostR.Count);
                print("Json Load successed");
            }
            else
            {
                print("Not found Json Save File");
            }
        }
    }
}
