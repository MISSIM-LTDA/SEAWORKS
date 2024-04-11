using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectTransformToRecord
{
    public Vector3 Postion;
    public Quaternion Rotation;
    public ObjectTransformToRecord(Vector3 recorderPosition, Quaternion recorderRotations)
    {
        Postion = recorderPosition;
        Rotation = recorderRotations;
    }
}
