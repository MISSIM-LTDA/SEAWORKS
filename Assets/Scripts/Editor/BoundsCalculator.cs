//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditorInternal;
//using System;
//using System.Linq;

//public class BoundsCalculator : EditorWindow
//{

//    public GameObject userObject;
//    float userLimit;


//    //Item no menu Tools
//    [MenuItem("Tools/BoundsCalculator")]

//    //abrir Popup toolbar
//    public static void ShowWindow()
//    {
//        GetWindow(typeof(BoundsCalculator));
//    }

//    //Conteudo do popup
//    void OnGUI()
//    {
//        GUILayout.Space(10);

//        //inserir objeto
//        userObject = EditorGUILayout.ObjectField("Object", userObject, typeof(GameObject), true) as GameObject;

//        //armazenar limite
//        userLimit = EditorGUILayout.FloatField("Limit:", userLimit);
//        GUILayout.Space(10);
//         HandleCheck();

//    }






//    //Ao clicar em check
//    private void HandleCheck()
//    {
  
//        //verifica se tem objeto
//        if (GUILayout.Button("Check"))
//        {
//            if (userObject == null)
//            {
//                ShowNotification(new GUIContent("No object selected"));
//            }

//            else
//            {
//                UnpackSelectedPrefab();
//                //CreateFolders();
//                //CheckBounds();
//                //CompareWithLimit();
//            }
//        }
//    }

//    //desempacota se for prefab
//    private void UnpackSelectedPrefab()
//    {

//        if (userObject != null)
//        {
//            PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(userObject);

//            if (prefabType == PrefabAssetType.Regular || prefabType == PrefabAssetType.Model)
//            {
//                // Desempacote o prefab
//                PrefabUtility.UnpackPrefabInstance(userObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
//                Debug.Log($"Prefab desempacotado: {userObject.name}");
//            }

//            else if (userObject == null)
//            {
//                Debug.Log("nao era prefab");
//            }

//        }

//    }



    
//    }
  








//    //insere na respectiva pasta


     

