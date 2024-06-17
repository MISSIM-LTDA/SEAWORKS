using UnityEngine;
using UnityEditor;


public class BoundsNew : EditorWindow
{

    GameObject userObject;
    float userLimit;
    private float objectSizeX;
    private float objectSizeY;
    private float objectSizeZ;
    private GameObject smallObjects;
    private GameObject bigObjects;
    private GameObject noMeshObjects;


    //Item no menu Tools
    [MenuItem("Tools/BoundsCalculator")]

    //abrir Popup toolbar
    public static void ShowWindow()
    {
        GetWindow(typeof(BoundsNew));
    }



    //    //Conteudo do popup
    void OnGUI()
    {
        GUILayout.Space(10);

        //inserir objeto
        userObject = EditorGUILayout.ObjectField("Object", userObject, typeof(GameObject), true) as GameObject;
    
        //armazenar limite
        userLimit = EditorGUILayout.FloatField("Limit:", userLimit);
        GUILayout.Space(10);
        HandleCheck();

    }

    //    //Ao clicar em check
    private void HandleCheck()
    {

        //verifica se tem objeto
        if (GUILayout.Button("Check"))
        {
            if (userObject != null)
            {
                UnpackSelectedPrefab();
                CreateFolders();
                CheckBounds();

            }

            else
            {
                ShowNotification(new GUIContent("No object selected"));
            }
        }
    }



    //desempacota se for prefab
    private void UnpackSelectedPrefab()
    {

        if (userObject != null)
        {
            PrefabAssetType prefabType = PrefabUtility.GetPrefabAssetType(userObject);

            if (prefabType == PrefabAssetType.Regular || prefabType == PrefabAssetType.Model)
            {
                // Desempacote o prefab
                PrefabUtility.UnpackPrefabInstance(userObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
          
                ShowNotification(new GUIContent($"Prefab  {userObject.name} unpacked"));

            }

        }
        else
        {
            ShowNotification(new GUIContent(" unpacked ! "));

        }
    }
    
    

    private void CreateFolders()
    {
        smallObjects = new GameObject("Small - Useless Objects");
        smallObjects.transform.SetParent(userObject.transform);

        bigObjects = new GameObject("Big - usefull Objects");
        bigObjects.transform.SetParent(userObject.transform);

        //noMeshObjects = new GameObject("No mesh Objects");
        //noMeshObjects.transform.SetParent(userObject.transform);


        ShowNotification(new GUIContent("Successfully separated objects ! "));

    }


    private void CheckBounds()
    {
        
        foreach (MeshRenderer child in userObject.GetComponentsInChildren<MeshRenderer>())
        {
          

            //verifica cada bound size XYZ
            MeshFilter meshFilter = child.GetComponent<MeshFilter>();

            Mesh mesh = meshFilter.mesh;
            Bounds bounds = child.gameObject.GetComponent<MeshFilter>().mesh.bounds;

            objectSizeX = bounds.size.x;
            objectSizeY = bounds.size.y;
            objectSizeZ = bounds.size.z;

            MoveToFolders(child);

        }


    }


    private void MoveToFolders(MeshRenderer child )
    {
      

        //muda de "pasta"
        if (objectSizeX > userLimit || objectSizeY > userLimit || objectSizeZ > userLimit)
        {
           child.transform.SetParent(bigObjects.transform);
        }
    
      
        else
        {
            child.transform.SetParent(smallObjects.transform);
        }

    }
  
}


