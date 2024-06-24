using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartTrackSystem
{
    [Serializable]
    public class ObjectTransformToRecord
    {
        public bool e = false;//enable

        public string p;//position
        public string r;//rotation
        public ObjectTransformToRecord(bool enable, string position, string rotation)
        {
            e = enable;
            p = position;
            r = rotation;
        }
    }

    [Serializable]
    public class RopeTransformToRecord
    {
        public bool i = false;//initialIndex
        public bool e = false;//enable

        public string p;//position
        public string r;//rotation

        public string iPM;//invPosMasses
        public string iRM;//invRotMasses

        public int pC = 0;//particleCount
        public string l;//lenght
        public RopeTransformToRecord(bool initialIndex, bool enable, string position, string rotation)
        {
            i = initialIndex;

            e = enable;

            p = position;
            r = rotation;
        }
        public RopeTransformToRecord(bool initialIndex, bool enable, string position,
            string rotation, string invPosMasses,
            string invRotMasses, int particleCount, string lenght)
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
