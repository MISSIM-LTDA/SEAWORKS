using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartTrackSystem
{
    [Serializable]
    public class ObjectTransformToRecord
    {
        public bool initialIndex = false;

        public bool enable = false;
        public Vector3 postion = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;

        public float invPosMasses = 0.0f;
        public float invRotMasses = 0.0f;

        public int particleCount = 0;
        public float lenght = 0.0f;

        public ObjectTransformToRecord(bool enable, Vector3 position, Quaternion rotation)
        {
            this.enable = enable;
            this.postion = position;
            this.rotation = rotation;
        }

        public ObjectTransformToRecord(bool initialIndex, bool enable, Vector3 position, Quaternion rotation, int particleCount, float lenght)
        {
            this.initialIndex = initialIndex;

            this.enable = enable;
            this.postion = position;
            this.rotation = rotation;

            this.particleCount = particleCount;
            this.lenght = lenght;
        }

        public ObjectTransformToRecord(bool initialIndex, bool enable, Vector3 position, Quaternion rotation, float invPosMasses, float invRotMasses, int particleCount, float lenght)
        {
            this.initialIndex = initialIndex;

            this.enable = enable;
            this.postion = position;
            this.rotation = rotation;

            this.invPosMasses = invPosMasses;
            this.invRotMasses = invRotMasses;

            this.particleCount = particleCount;
            this.lenght = lenght;
        }
    }

    [Serializable]
    public class RecordedObjectInfo
    {
        public string Name;
        public List<ObjectTransformToRecord> RecordObjectStore = new List<ObjectTransformToRecord>();
        public RecordedObjectInfo(string m_name, List<ObjectTransformToRecord> m_recordObjectStore)
        {
            Name = m_name;
            RecordObjectStore = m_recordObjectStore;
        }
    }
}
