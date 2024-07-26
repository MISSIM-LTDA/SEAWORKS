using SmartTrackSystem;
using System.Collections;
public interface IRecorded
{
    bool saving { get; set; }
    bool loading { get; set; }
    string folderPath { get; set; }
    int index { get; set; }
    string decimalPlaces { get; set; }

    #region Save
    void SaveNewPosition(string path);
    IEnumerator SaveNewPositionCoroutine(string path);
    void GetObjectPosition(ref RecordedInfo<ObjectTransformToRecord> rec);
    void SavePositions();
    #endregion

    #region Load
    void LoadNewPosition(string path);
    IEnumerator LoadNewPositionCoroutine(string path);
    void LoadPositions(ref RecordedInfo<ObjectTransformToRecord> rec, bool makePhysics);
    #endregion

    #region Support
    void SaveOrLoadSetup();
    IEnumerator ChooseDirectory();
    IEnumerator ChooseFile();
    string CreateDirectoryToSaveAll(string path);
    string FixFolderPath(string path);
    string FindCorrectFileOnFolder(string path);
    IEnumerator ReadPathFromFile();
    #endregion
}
