using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingClaw : MonoBehaviour
{
    public bool Claws_collided;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("LeftClaw"))
            Claws_collided = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("LeftClaw"))
            Claws_collided = false;
    }
}
