using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace rj.ghost.runtime{


[Serializable]
public class ghostRecord
{
    public Vector3 LR;
    public Quaternion LRR;
    public ghostRecord(Vector3 recorders, Quaternion recorderRotations)
    {
        LR = recorders;
        LRR = recorderRotations;
    }

}
}