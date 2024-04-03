using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace rj.ghost.editor
{
    public class GoalScript : MonoBehaviour
    {
        public GameObject obj;
        public GameObject[] SliderConsole;
        public GameObject Win;
        public GameObject Ins1;
        public GameObject Ins2;
        public GhostRecorder[] gr;
        void Start()
        {
            Invoke("StartRR", 0.5f);

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "FootBall")
            {
                Win.SetActive(true);
                for (int i = 0; i < gr.Length; i++)
                {
                    gr[i].StopRecording();
                }
                Invoke("hidWin", 3f);

            }
        }
        void StartRR()
        {
            for (int i = 0; i < gr.Length; i++)
            {
                gr[i].StartRecording();
            }
        }
        void hidWin()
        {
            for (int i = 0; i < SliderConsole.Length; i++)
            {
                SliderConsole[i].SetActive(true);
            }
            obj.transform.position = new Vector3(50, 0, 50);
            Win.SetActive(false);
            Ins1.SetActive(false);
            Ins2.SetActive(true);
        }
    }
}
