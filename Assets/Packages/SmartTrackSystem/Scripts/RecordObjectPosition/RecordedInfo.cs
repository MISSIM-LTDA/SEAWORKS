using System;
using System.Collections.Generic;

namespace SmartTrackSystem
{
    [Serializable]
    public class ObjectTransformToRecord
    {
        public bool e = false;//enable

        public string p;//position
        public string r;//rotation
        public ObjectTransformToRecord(bool enable, string position, string rotation) {
            e = enable;
            p = position;
            r = rotation;
        }
    }

    [Serializable]
    public class RopeTransformToRecord : ObjectTransformToRecord
    {
        public bool i = false;//initialIndex

        public string iPM;//invPosMasses
        public string iRM;//invRotMasses

        public int pC = 0;//particleCount
        public string l;//lenght
        public RopeTransformToRecord(bool initialIndex, bool enable, 
            string position, string rotation, 
            string invPosMasses,string invRotMasses, 
            int particleCount, string lenght) : base(enable,position,rotation)
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
    public class RecordedInfo<T> where T : ObjectTransformToRecord
    {
        public string Name;
        public List<T> RecordObjectStore = new List<T>();
        public RecordedInfo(string name, List<T> recordObjectStore)
        {
            Name = name;
            RecordObjectStore = recordObjectStore;
        }
    }
}
