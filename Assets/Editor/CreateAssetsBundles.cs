using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetsBundles : MonoBehaviour
{
    [MenuItem("Assets/Create All Assets Bundles")]
    static void BuildAllAssetsBundle() 
    {
        string assetBundleDirectory = Application.streamingAssetsPath +  "/AssetsBundles/";
        Debug.Log(assetBundleDirectory);

        if (!Directory.Exists(assetBundleDirectory)) { Directory.CreateDirectory(assetBundleDirectory); }

        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        EditorUserBuildSettings.activeBuildTarget);

        AssetDatabase.Refresh();
    }
}
