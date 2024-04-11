using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rj.ghost.runtime;
using Obi;

namespace SmartTrackSystem 
{
    public class RecordMovementObject : RecordMovement
    {
        public GameObject m_prefab;
        public bool isReplayObject = false;

        private void Start()
        {
            Init();
        }

        public override void Init()
        {
            recordObject = gameObject;
            m_prefab = Resources.Load<GameObject>("Ghosts/" + m_recordObject.name + "_ghost");
        }

        //Record current position data
        public override IEnumerator StartRecord()
        {
            while (isR)
            {
                ghostR.Add(new ghostRecord(transform.position, transform.rotation));
                yield return new WaitForSeconds(recordRate);
            }
        }

        //Instantiate the replay object 
        public void CreateReplayObject()
        {
            if (!isReplayObject && ghostR[0] != null)//Make sure the ghost only instantiate once
            {
                ghost = Instantiate(m_prefab, transform.position, transform.rotation) as GameObject;
                isReplayObject = true;
            }

            if (ghostR[0] != null)
                ghost.SetActive(true);
        }

        //Rewind the ghost, at the same time erase the footsteps that the ghost passed
        public override IEnumerator PlayGhost()
        {
            for (int i = (int)m_playBackSystem.sliderValue; i < ghostR.Count; i++)
            {
                if (ghostR[i] != null && m_playBackSystem.IsPlaying == true)
                {
                    ghost.transform.position = ghostR[i].LR;
                    ghost.transform.rotation = ghostR[i].LRR;
                }
                yield return null;
            }
        }
    }
}

