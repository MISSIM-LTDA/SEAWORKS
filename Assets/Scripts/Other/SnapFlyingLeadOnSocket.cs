using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapFlyingLeadOnSocket : MonoBehaviour
{
    GameObject flyingLead;
    Transform socket;

    [SerializeField] bool drawSphere;

    [SerializeField] float radius;
    Vector3 center;
    [SerializeField] Vector3 centerOffset;
    [SerializeField] Vector3 positionOffset;

    bool efl;
    bool hfl;
    private void Start()
    {
        flyingLead = gameObject;

        if (flyingLead.tag == "EFL_Parent") { efl = true; }
        else if (flyingLead.tag == "HFL_Parent") { hfl = true; }

        center = transform.position + centerOffset;
        socket = CheckCollision(center);

        #if UNITY_EDITOR
                if (socket != null) { DestroyImmediate(socket.gameObject); }
        #endif

        if (socket != null) { Destroy(socket.gameObject); }
    }
    void Update()
    {
        #if UNITY_EDITOR
            flyingLead = gameObject;

            if (flyingLead.tag == "EFL_Parent") { efl = true; }
            else if (flyingLead.tag == "HFL_Parent") { hfl = true; }

            center = transform.position + centerOffset;
            socket = CheckCollision(center);

            if (socket != null) { Snap(); }
        #endif
    }

    private void Snap() 
    {
        transform.position = socket.position + positionOffset;
    }

    private Transform CheckCollision(Vector3 center)
    {
        Transform socket = null;

        Collider[] colliders = Physics.OverlapSphere(center, radius);

        foreach (Collider coll in colliders) 
        {
            if(efl && coll.transform.tag == "EFL_socket") { socket = coll.transform; }
            else if (hfl && coll.transform.tag == "HFL_socket") { socket = coll.transform; }
        }

        return socket;
    }
    private void OnDrawGizmos(){if (drawSphere) { Gizmos.DrawSphere(center, radius);}}
}
