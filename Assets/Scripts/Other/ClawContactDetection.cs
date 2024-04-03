using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawContactDetection : MonoBehaviour
{
    public bool Claw_Center_Collided;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("RightClaw") && !other.CompareTag("LeftClaw"))
            Claw_Center_Collided = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("RightClaw") && !other.CompareTag("LeftClaw"))
            Claw_Center_Collided = false;
    }
}
