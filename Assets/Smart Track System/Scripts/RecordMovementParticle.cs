using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rj.ghost.runtime;
using Obi;

namespace SmartTrackSystem
{
    public class RecordMovementParticle : RecordMovement
    {
        public ObiActor m_obiActor;
        public int size = 0;

        public int i = 0;
        public bool cont = false;
        private void Start()
        {
            Init();
        }
        public override void Init()
        {
            recordObject = gameObject;
            m_obiActor = gameObject.GetComponent<ObiActor>();

            if (m_obiActor.GetType() == typeof(ObiRope)) 
            {
                ObiRope rope;
                rope = recordObject.GetComponent<ObiRope>();
                size = rope.ropeBlueprint.activeParticleCount;
            }

            else if (m_obiActor.GetType() == typeof(ObiRod))
            {
                ObiRod rope;
                rope = recordObject.GetComponent<ObiRod>();
                size = rope.rodBlueprint.activeParticleCount;
            }

            for (int i = 0; i < size; i++)
            {
                MoveParticle moveParticle = gameObject.AddComponent<MoveParticle>();
                moveParticle.particle = i;
            }
        }
        public override IEnumerator StartRecord()
        {
            int i;

            while (isR)
            {
                for (i = 0; i < size; i++)
                {
                    ghostR.Add(new ghostRecord(m_obiActor.solver.positions[m_obiActor.solverIndices[i]], m_obiActor.solver.orientations[m_obiActor.solverIndices[i]]));
                }
                yield return new WaitForSeconds(recordRate);
            }
        }

        //Rewind the ghost, at the same time erase the footsteps that the ghost passed
        public override IEnumerator PlayGhost()
        {
            //CreateGhost();//Only instantiate the ghost when rewind started
            //for (int i = (int)mySlider.value; i < ghostR.Count; i++)
            //{
            //    if (ghostR[i] != null && isPlaying == true)
            //    {
            //        ghost.transform.position = ghostR[i].LR;
            //        ghost.transform.rotation = ghostR[i].LRR;
            //        slideT2 = i;
            //        mySlider.value = slideT2; //The slider will follow the lead
            //        if (i == ghostR.Count - 1) { isPlaying = false; }
            //    }
            yield return null;
            //}
        }
    }
}
