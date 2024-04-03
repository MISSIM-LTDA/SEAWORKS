using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawCollisionDetection : MonoBehaviour
{
    public GameObject other;
    public bool colided;

    private void OnCollisionEnter(Collision collision)
    {
        colided = collision.gameObject == other;
    }
    private void OnCollisionExit(Collision collision)
    {
        colided = !collision.gameObject == other;
    }
}
