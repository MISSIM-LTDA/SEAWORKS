using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using Obi;
using rj.ghost.runtime;
using UnityEngine.UI;

public class GhostParticles : MonoBehaviour
{
    [SerializeField]
    public GameObject rope;
    public ObiRope obiRope;

    public GameObject ghostAttach;
    public GameObject[] ghostAttachments;

    public List<ghostRecord> ghostR = new List<ghostRecord>();
    public List<Coroutine> coroutines = new List<Coroutine>() { };

    public Slider mySlider;

    public bool isR;
    public bool playRecording;
    public bool stopRecording;
    public bool save;
    public bool load;

    public bool playGhost;
    public bool pauseGhost;

    public bool isPlaying;

    public int saveSlot = 0;
    public string saveName;

    private int slideT2;

    public int size;

    public float recordRate;
    public float playRate;

    public bool aux=true;
    public bool space;
    public int i=0;
    public bool cont = false;
    void Start()
    {
        rope = this.transform.gameObject;
        obiRope = rope.GetComponent<ObiRope>();

        size = obiRope.ropeBlueprint.activeParticleCount;

        //for (int i = 0; i < size; i++) 
        //{
        //    //MoveParticle moveParticle = gameObject.AddComponent<MoveParticle>();
        //    moveParticle.particle = i;
        //}
    }
    void Update()
    {
        if (playRecording) 
        {
            isR = true;
            StartCoroutine("StartRecord");
            playRecording = false;
        }

        if (stopRecording) 
        {
            StopCoroutine("StartRecord");
            isR = false;
            stopRecording = false;
        }

        if (save) 
        {
            jsonSave();
            save = false;
        }

        if (load)
        {
            jsonLoad();
            load = false;
        }

        if (playGhost) 
        {
            playGhost = false;

            if (aux) 
            {
                for (int k = 0; k < size; k++)
                    obiRope.solver.invMasses[obiRope.solverIndices[k]] = 0;

                for (int i = 0; i < size; i++)
                {
                    obiRope.solver.positions[obiRope.solverIndices[i]] = ghostR[i].LR;
                    obiRope.solver.orientations[obiRope.solverIndices[i]] = ghostR[i].LRR;
                }
            }

            cont = true;           
            InvokeRepeating("PlayGhost", 1f, playRate);
            isPlaying = true;
        }

      

        if (pauseGhost) 
        {
            pauseGhost = false;
            CancelInvoke("PlayGhost");
            isPlaying = false;
        }

        RefreshTimer();  
    }
    void RefreshTimer()
    {
        if (mySlider != null)
        {
            mySlider.maxValue = (ghostR.Count);//update the slider max value every frame
        }
    }
    IEnumerator StartRecord()
    {
        int i;

        while (isR)
        {
            for (i = 0; i < size; i++) 
            {
                ghostR.Add(new ghostRecord(obiRope.solver.positions[obiRope.solverIndices[i]], obiRope.solver.orientations[obiRope.solverIndices[i]]));
            }
            yield return new WaitForSeconds(recordRate);
        }
    }
    Save createSave()
    {
        Save save = new Save();
        save.ghostRstore = ghostR;
        return save;
    }
    private void chooseSlot()
    {
        switch (saveSlot)
        {
            case 0:
                saveName = "/SaveData/" + gameObject.name + "_JsonData1.txt";
                break;
        }
    }
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
    public void jsonLoad()
    {
        ClearRecordData();
        chooseSlot();
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
    private void ClearRecordData()
    {
        ghostR.Clear();
    }
    public  void PlayGhost()
    {
        if (i == ghostR.Count-size)
            CancelInvoke("PlayGhost");
        else
            i += size;
    }
}

