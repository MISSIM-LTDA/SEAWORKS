using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

public class BoundSToolBar : EditorWindow
{
    int polygonsMax;
    int polygonsMin;
    GameObject Parent;
    GameObject TrisObj;
    Vector3 BoundLimit;
    int ax;

    [MenuItem("Tools/BoundsControlGenerator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BoundSToolBar));
    }
    public Transform body;

    public void OnGUI()
    {
        Parent = EditorGUILayout.ObjectField("Object to do a better performance", Parent, typeof(GameObject), true) as GameObject;
        BoundLimit = EditorGUILayout.Vector3Field("Set the limit of the Boundry", BoundLimit);
        //polygonsMax = EditorGUILayout.IntField("Set the max polygon count", polygonsMax);
        GUILayout.Space(10);


        GUILayout.Space(10);

        if (GUILayout.Button("Check"))
        {
            CheckChild();
            foreach (Transform child in Parent.GetComponentsInChildren<Transform>())
            {
                if (child.name != Parent.name)
                {
                    if (child.name == "SmallObjs")
                    {
                        CheckTris(child.gameObject);
                        foreach (Transform childs in child.GetComponentsInChildren<Transform>())
                        {
                            if (childs.name != Parent.name)
                            {
                                if (childs.name == "Less1000Tris")
                                {
                                    EqualObjs(childs.gameObject);

                                }
                                else if (childs.name == "1000HigherTris")
                                {
                                    EqualObjs(childs.gameObject);
                                }
                                else if (childs.name == "5000HigherTris")
                                {
                                    EqualObjs(childs.gameObject);
                                }
                            }

                        }
                    }
                    else if (child.name == "NormalObjs")
                    {
                        CheckTris(child.gameObject);

                        foreach (Transform childs in child.GetComponentsInChildren<Transform>())
                        {
                            if (childs.name != Parent.name)
                            {
                                if (childs.name == "Less1000Tris")
                                {
                                    EqualObjs(childs.gameObject);

                                }
                                else if (childs.name == "1000HigherTris")
                                {
                                    EqualObjs(childs.gameObject);
                                }
                                else if (childs.name == "5000HigherTris")
                                {
                                    EqualObjs(childs.gameObject);
                                }
                            }

                        }
                    }
                    else if (child.name == "BigObjs")
                    {
                        CheckTris(child.gameObject);

                        foreach (Transform childs in child.GetComponentsInChildren<Transform>())
                        {
                            if (childs.name != Parent.name)
                            {
                                if (childs.name == "Less1000Tris")
                                {
                                    EqualObjs(childs.gameObject);

                                }
                                else if (childs.name == "1000HigherTris")
                                {
                                    EqualObjs(childs.gameObject);
                                }
                                else if (childs.name == "5000HigherTris")
                                {
                                    EqualObjs(childs.gameObject);
                                }
                            }

                        }
                    }
                }

            }
        }


        //TrisObj = EditorGUILayout.ObjectField("Tris Obj to rearrenge", TrisObj, typeof(GameObject), true) as GameObject;

        //if (GUILayout.Button("Check Equals"))
        //{
        //    EqualObjs();
        //}

    }

    string count;
    void CheckTris(GameObject Parent)
    {
        GameObject obj1 = new GameObject("Less1000Tris", new Type[] { });
        Undo.RegisterCreatedObjectUndo(obj1, "Creating Obj");
        obj1.transform.SetParent(Parent.transform);


        GameObject obj2 = new GameObject("1000HigherTris", new Type[] { });
        Undo.RegisterCreatedObjectUndo(obj2, "Creating Obj");
        obj2.transform.SetParent(Parent.transform);

        GameObject obj3 = new GameObject("5000HigherTris", new Type[] { });
        Undo.RegisterCreatedObjectUndo(obj3, "Creating Obj");
        obj3.transform.SetParent(Parent.transform);


        foreach (Transform child in Parent.GetComponentsInChildren<Transform>())
        {
            if (child.GetComponent<MeshRenderer>())
            {
                if (child.GetComponent<MeshFilter>().mesh.triangles.Length / 3 < 1000)
                {
                    child.transform.SetParent(obj1.transform);
                }

                if ((child.GetComponent<MeshFilter>().mesh.triangles.Length / 3 < 5000) && (child.GetComponent<MeshFilter>().mesh.triangles.Length / 3 > 1000))
                {
                    child.transform.SetParent(obj2.transform);
                }

                if (child.GetComponent<MeshFilter>().mesh.triangles.Length / 3 > 5000)
                {
                    child.transform.SetParent(obj3.transform);
                }
            }
        }
    }

    public void EqualObjs(GameObject TrisObj)
    {
        foreach (Transform child in TrisObj.GetComponentsInChildren<Transform>())
        {
            GameObject obj1 = new GameObject("Object", new Type[] { });
            Undo.RegisterCreatedObjectUndo(obj1, "Creating Obj");
            obj1.transform.SetParent(TrisObj.transform);
            child.transform.SetParent(obj1.transform);

            foreach (Transform childs in TrisObj.GetComponentsInChildren<Transform>())
            {
                if ((child.GetComponent<MeshFilter>()) && (childs.GetComponent<MeshFilter>()))
                {
                    if (childs.name != child.name)
                    {
                        if ((child.name != "Object") || (childs.name != "Object"))
                        {
                            if (childs.childCount == 0)
                            {
                                if (child.GetComponent<MeshFilter>().mesh.bounds == childs.GetComponent<MeshFilter>().mesh.bounds)
                                {
                                    childs.transform.SetParent(obj1.transform);
                                }
                            }
                        }
                    }
                }
            }
            foreach (Transform childs in TrisObj.GetComponentsInChildren<Transform>())
            {
                if (childs.name == "Object")
                {
                    if (childs.childCount == 0)
                    {
                        DestroyImmediate(childs.gameObject);
                    }
                    else if (childs.childCount == 1)
                    {
                        foreach (Transform Child in childs.GetComponentsInChildren<Transform>())
                        {
                            Child.GetComponent<Transform>().SetParent(TrisObj.transform);
                        }
                        DestroyImmediate(childs.gameObject);
                    }

                }
            }
        }


    }

    public void CheckChild()
    {
        GameObject obj1 = new GameObject("SmallObjs", new Type[] { });
        Undo.RegisterCreatedObjectUndo(obj1, "Creating Obj");
        obj1.transform.SetParent(Parent.transform);


        GameObject obj2 = new GameObject("NormalObjs", new Type[] { });
        Undo.RegisterCreatedObjectUndo(obj2, "Creating Obj");
        obj2.transform.SetParent(Parent.transform);

        GameObject obj3 = new GameObject("BigObjs", new Type[] { });
        Undo.RegisterCreatedObjectUndo(obj3, "Creating Obj");
        obj3.transform.SetParent(Parent.transform);

        foreach (MeshRenderer child in Parent.GetComponentsInChildren<MeshRenderer>(true))
        {

            for (int x = 0; x < 3; x++)
            {
                if (child.gameObject.GetComponent<MeshFilter>().mesh.bounds.size[x] * child.gameObject.GetComponent<Transform>().localScale[x] < BoundLimit[x]) ax++;

            }

            if (ax == 0)
            {
                child.transform.SetParent(obj3.transform);
            }

            if (ax == 1)
            {
                child.transform.SetParent(obj2.transform);
            }

            if (ax >= 2)
            {
                child.transform.SetParent(obj1.transform);
            }



            ax = 0;
        }
    }
}
