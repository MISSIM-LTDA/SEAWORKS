using System.Collections;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.UI;
using Mono.Cecil;

namespace RenderHeads.Media.AVProMovieCapture
{
    public class RecordControl : MonoBehaviour
	{
        private CaptureFromTexture movieCapture;

		private Image recordingImage;
		private Sprite recOn;
        private Sprite recOff;
        private void Start()
        {
            FileBrowser.SetFilters(false, new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
            FileBrowser.SetDefaultFilter(".txt");
            FileBrowser.AddQuickLink("Users", "C:\\Users", null);

			movieCapture = transform.parent.GetComponentInChildren<CaptureFromTexture>();

			recordingImage = GetComponentsInChildren<Image>()[GetComponentsInChildren<Image>().Length-1];

			recOn = Resources.Load<Sprite>("recOn");
			recOff = Resources.Load<Sprite>("recOff");

            Button[] buttons = GetComponentsInChildren<Button>();

			buttons[0].onClick.AddListener(StartRecording);
            buttons[1].onClick.AddListener(StopRecording);
            buttons[2].onClick.AddListener(ResumeRecording);
            buttons[3].onClick.AddListener(() => StartCoroutine(ChooseDirectory()));
		}
        private void Update()
        {
            if(movieCapture && movieCapture.IsCapturing()) {
				recordingImage.sprite = recOn;
			}

			else { recordingImage.sprite = recOff;}
        }
        public void StartRecording()
        {
			if(movieCapture.OutputFolderPath != null 
				&& movieCapture.OutputFolderPath != "") {
                movieCapture.StartCapture();

				if (movieCapture.CompletedFileWritingAction == null){
                    movieCapture.NamedPipePath = movieCapture.OutputFolderPath;
					movieCapture.CompletedFileWritingAction += OnCompleteFinalFileWriting;
				}
            }

            else{ Debug.Log("No save path chosen");}	
		}
		public void StopRecording()
		{
			if (movieCapture && movieCapture.IsCapturing()){
                movieCapture.GetComponent<CaptureGUI>().StopCapture();
            }

            else if(!movieCapture){
				Debug.Log("No CaptureFromTexture found");
			}

			else { 
				Debug.Log("No recoding being made"); 
			}
		}
		public void ResumeRecording()
		{
			if (movieCapture){
                movieCapture.ResumeCapture();
			}

			else{
				Debug.Log("No CaptureFromTexture found");
			}
		}
		void OnCompleteFinalFileWriting(FileWritingHandler handler)
		{
			Debug.Log(handler.Path);
			Debug.Log("Recording finished! The file is at: " + handler.Path);
		}
		IEnumerator ChooseDirectory()
		{
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Folders, false, null, "Record", "Get Folder Path", "Save");

			if (FileBrowser.Success){
                movieCapture.OutputFolderPath = FileBrowser.Result[0]; ;
			}
		}
	}
}
