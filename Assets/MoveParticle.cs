using System.Collections;
using UnityEngine;
using Obi;

namespace SmartTrackSystem 
{
    public class MoveParticle : MonoBehaviour
    {
        private ObiRope obiRope;

        private int m_particle;
        private int position;

        private bool move;

        private float recordRate;
        void Start()
        {
            obiRope = gameObject.GetComponent<ObiRope>();
            recordRate = gameObject.GetComponent<RecordMovement>().recordRate / 2;
        }


        void Update()
        {
            position = gameObject.GetComponent<RecordMovementParticle>().i;
            move = gameObject.GetComponent<RecordMovementParticle>().cont;

            if (move)
                StartCoroutine(MoveToPosition());
        }

        public IEnumerator MoveToPosition()
        {
            var currentPos = obiRope.solver.positions[obiRope.solverIndices[m_particle]];
            var t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / recordRate;
                obiRope.solver.positions[obiRope.solverIndices[m_particle]] = Vector3.Lerp(currentPos, this.gameObject.GetComponent<GhostParticles>().ghostR[particle + position].LR, t);
                yield return null;
            }
        }
        public int particle
        {
            get { return m_particle; }
            set { m_particle = value; }
        }
    }
}

