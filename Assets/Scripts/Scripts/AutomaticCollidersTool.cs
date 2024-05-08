using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

public class AutomaticCollidersTool : EditorWindow
{

    GameObject Parent;
    GameObject ObiColliderParent;
    GameObject objects;
    Vector3 BoundLimit;
    int ax;

    [MenuItem("Tools/AutomaticCollidersTool")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AutomaticCollidersTool));
    }

    public void OnGUI()
    {
        //Object that will need a ObiCollider
        Parent = EditorGUILayout.ObjectField("Object", Parent, typeof(GameObject), true) as GameObject;
        //Object who holds the collider
        ObiColliderParent = EditorGUILayout.ObjectField("ObiCollider Parent", ObiColliderParent, typeof(GameObject), true) as GameObject;
        BoundLimit = EditorGUILayout.Vector3Field("Set the limit of the Boundry", BoundLimit);
        GUILayout.Space(5);


        if (GUILayout.Button("Check"))
        {
            CheckChild(); 
            checkOverllaping();
        }
    }

    public LayerMask m_LayerMask;
    public void CheckChild()
    {

        foreach (MeshRenderer child in Parent.GetComponentsInChildren<MeshRenderer>(true))
        {
            for (int x = 0; x < 3; x++)
            {
                if (child.bounds.size[x] >= BoundLimit[x])
                {
                    ax++;
                }
            }
            if (ax >= 2)
            {
                objects = new GameObject("Object", new Type[] { });
                Undo.RegisterCreatedObjectUndo(objects, "Create ObiCollider");

                objects.transform.SetParent(ObiColliderParent.transform);


                objects.gameObject.AddComponent<BoxCollider>();
                objects.gameObject.GetComponent<BoxCollider>().size = child.bounds.size;
                objects.gameObject.GetComponent<BoxCollider>().center = child.bounds.center - child.transform.position;

                objects.transform.position = child.transform.position;
            }

            ax = 0;
        }
    }

    public void checkOverllaping()
    {
        foreach (BoxCollider child in ObiColliderParent.GetComponentsInChildren<BoxCollider>(true))
        {
            ax = 0;
            Collider[] hitColliders = Physics.OverlapBox(child.gameObject.transform.position, child.transform.localScale / 2, Quaternion.identity);
            int i = 0;
            //check when there is a new collider coming into contact with the box
            while (i < hitColliders.Length)
            {
                //   Output all of the collider names
                for (int x = 0; x < 3; x++)
                {
                    if (hitColliders[i].GetComponent<BoxCollider>().size[x] < child.gameObject.GetComponent<BoxCollider>().size[x]) ax++;
                }
                if (ax == 3)
                {
                    hitColliders[i].gameObject.SetActive(false);
                }
                else
                {
                    hitColliders[i].gameObject.SetActive(true);
                }
                ax = 0;
                i++;

            }
        }
        foreach(BoxCollider child in ObiColliderParent.GetComponentsInChildren<BoxCollider>(true)){
            for (int x = 0; x < 3; x++)
            {
                ax = 0;
                if (child.gameObject.GetComponent<BoxCollider>().size[x] > 1)
                {
                    ax++;
                }
                if (ax == 3)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        foreach (Transform child in ObiColliderParent.GetComponentsInChildren<Transform>(true))
        {
            Debug.Log("Bora Tubaroes");
            if(child.gameObject.activeInHierarchy == false)
            {
                DestroyImmediate(child.gameObject);
            }
            
        }
    }
}   

