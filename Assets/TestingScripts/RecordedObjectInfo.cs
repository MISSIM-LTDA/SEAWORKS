using System;
using System.Collections.Generic;

[Serializable]
public class RecordedObjectInfo
{
    public string Name;
    public int ThisIsRecordedObjectData;
    public List<ObjectTransformToRecord> RecordObjectStore = new List<ObjectTransformToRecord>();
    public RecordedObjectInfo(string m_name,int m_thisIsRecordedObjectData, List<ObjectTransformToRecord> m_recordObjectStore) 
    {
        Name = m_name;
        ThisIsRecordedObjectData = m_thisIsRecordedObjectData;
        RecordObjectStore = m_recordObjectStore;
    }
}
