using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartTrackSystem
{
    [Serializable]
    public class ObjectTransformToRecord
    {
        public bool e = false;//enable

        public Vector3 p = Vector3.zero;//position
        public Quaternion r = Quaternion.identity;//rotation
        public ObjectTransformToRecord(bool enable, Vector3 position, Quaternion rotation)
        {
            this.e = enable;
            this.p = position;
            this.r = rotation;
        }
    }

    [Serializable]
    public class RopeTransformToRecord
    {
        public bool i = false;//initialIndex

        public bool e = false;//enable

        public Vector3 p = Vector3.zero;//position
        public Quaternion r = Quaternion.identity;//rotation

        public float iPM = 0.0f;//invPosMasses
        public float iRM = 0.0f;//invRotMasses

        public int pC = 0;//particleCount
        public float l = 0.0f;//lenght
        public RopeTransformToRecord(bool initialIndex, bool enable, Vector3 position, 
            Quaternion rotation, float invPosMasses, 
            float invRotMasses, int particleCount, float lenght)
        {
            i = initialIndex;

            e = enable;

            p = position;
            r = rotation;

            iPM = invPosMasses;
            iRM = invRotMasses;

            pC = particleCount;
            l = lenght;
        }
    }

    [Serializable]
    public class RecordedObjectInfo
    {
        public string Name;
        public List<ObjectTransformToRecord> RecordObjectStore = new List<ObjectTransformToRecord>();
        public List<RopeTransformToRecord> RecordRopeStore = new List<RopeTransformToRecord>();
        public RecordedObjectInfo(string m_name, List<ObjectTransformToRecord> m_recordObjectStore)
        {
            Name = m_name;
            RecordObjectStore = m_recordObjectStore;
        }

        public RecordedObjectInfo(string m_name, List<RopeTransformToRecord> m_recordRopeStore)
        {
            Name = m_name;
            RecordRopeStore = m_recordRopeStore;
        }
    }
}
