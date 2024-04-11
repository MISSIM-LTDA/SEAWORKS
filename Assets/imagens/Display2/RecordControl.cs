using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.UI;
using System.Drawing;
using System.IO;

namespace RenderHeads.Media.AVProMovieCapture
{
    public class RecordControl : MonoBehaviour
	{
		string folderPath;
        [SerializeField] CaptureFromTexture _movieCapture = null;
        public GameObject CaptureGUI;

		public Button bttStartRecord;
		public Button bttStopRecord;
		public Button bttPauseRecord;
		public Button bttResumeRecord;
		public Button bttChoosePath;


		private void Start()
        {
			bttStartRecord.onClick.AddListener(StartC);
			bttStopRecord.onClick.AddListener(StopCapture);
			bttPauseRecord.onClick.AddListener(PauseCapture);
			bttResumeRecord.onClick.AddListener(ResumeCapture);
			bttChoosePath.onClick.AddListener(ChoosePath);
		}

        public void StartC()
        {
			if(folderPath != null) 
			{
				_movieCapture.StartCapture();
				if (_movieCapture.CompletedFileWritingAction == null)
				{
					_movieCapture.NamedPipePath = folderPath;
					_movieCapture.CompletedFileWritingAction += OnCompleteFinalFileWriting;
				}
            }
            else
            {
				Debug.Log("No Path acquired");
            }
			
		}

		public void StopCapture()
		{
			if (_movieCapture != null)
            {
				_movieCapture.GetComponent<CaptureGUI>().StopCapture();
            }
            else
            {
				Debug.Log("No CaptureFromTexture found");
            }

		}

		public void ResumeCapture()
		{
			if (_movieCapture != null)
			{
				_movieCapture.ResumeCapture();
			}

			else
			{
				Debug.Log("No CaptureFromTexture found");
			}
		}

		public void PauseCapture()
		{
			if (_movieCapture != null)
			{
				_movieCapture.PauseCapture();
			}
			else
			{
				Debug.Log("No CaptureFromTexture found");
			}
		}

		public void ChoosePath()
		{
			FileBrowser.SetFilters(false, new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
			FileBrowser.SetDefaultFilter(".txt");
			FileBrowser.AddQuickLink("Users", "C:\\Users", null);
			StartCoroutine(ChooseDirectory());
			
		}




		void OnCompleteFinalFileWriting(FileWritingHandler handler)
		{
			Debug.Log(handler.Path);
			Debug.Log("Recording finished! The file is at: " + handler.Path);
		}

		IEnumerator ChooseDirectory()
		{
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Folders, false, null, "Record", "Get Folder Path", "Save");

			if (FileBrowser.Success)
			{
				folderPath = FileBrowser.Result[0];
				_movieCapture.OutputFolderPath = folderPath;
				Debug.Log(FileBrowser.Result[0]);
			}
		}

	}
}
