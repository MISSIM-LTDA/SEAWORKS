using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace rj.ghost.runtime
{
    [Serializable]
    public class Save
    {
        //public List<Vector3> recordersJ = new List<Vector3>();
        //public List<Vector3> footsPPJ = new List<Vector3>();
        ////public List<Vector3> recordersRotationJ = new List<Vector3>();
        //public List<Quaternion> recordersRotationJ = new List<Quaternion>();
        public int ThisIsTheGhostData;
        public List<int> footPo;
        public List<ghostRecord> ghostRstore = new List<ghostRecord>();
    }
}
