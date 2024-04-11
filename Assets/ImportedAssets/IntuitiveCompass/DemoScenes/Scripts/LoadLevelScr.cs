using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevelScr : MonoBehaviour {
	
	public int Levels;
	
	private int CurrentLevel = 0;
	
	void Awake() {
        DontDestroyOnLoad(this);
    }
	
	public void GotoPrevLevel() {
		if (CurrentLevel > 0) {
			CurrentLevel -= 1;
			SceneManager.LoadScene(CurrentLevel);
		}
	}
	
	public void GotoNextLevel() {
		if (CurrentLevel < Levels) {
			CurrentLevel += 1;
			SceneManager.LoadScene(CurrentLevel);
		}
	}
}
