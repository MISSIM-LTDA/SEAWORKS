using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace SmartTrackSystem
{
   [RequireComponent(typeof(ObiActor))]
    public class RecordedRope : RecordedObject
    {
        public RecordedInfo<RopeTransformToRecord> recordRope = 
            new RecordedInfo<RopeTransformToRecord>("", new List<RopeTransformToRecord>() { });
        public RecordedInfo<RopeTransformToRecord> recordRopePosition = 
            new RecordedInfo<RopeTransformToRecord>("", new List<RopeTransformToRecord>() { });

        [SerializeField, HideInInspector] public ObiRopeBase rope;
        public class Attachments
        {
            public Transform target = null;
            public ObiParticleGroup particleGroup = null;
            public ObiParticleAttachment.AttachmentType attachmentType = ObiParticleAttachment.AttachmentType.Static;
            public float compliance = 0.0f;
            public float breakThreshold = float.PositiveInfinity;
        }

        private List<Attachments> destroyedAttachments = new List<Attachments>();

        #region Save Functions
        public void GetObjectPosition(ref RecordedInfo<RopeTransformToRecord> rec)
        {
            bool initialIndex = true;

            int particleCount = rope.activeParticleCount;
            float lenght = rope.restLength;

            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            ObiSolver solver = rope.solver;

            for (int i = 0; i < particleCount; i++) {

                rec.RecordObjectStore.Add(new RopeTransformToRecord
                (initialIndex, gameObject.activeSelf,
                rope.solver.positions[rope.solverIndices[i]].ToString(decimalPlaces, formatProvider),
                rope.solver.orientations[rope.solverIndices[i]].ToString(decimalPlaces, formatProvider),
                solver.invMasses[rope.solverIndices[i]].ToString(decimalPlaces, formatProvider),
                solver.invRotationalMasses[rope.solverIndices[i]].ToString(decimalPlaces, formatProvider),
                particleCount, lenght.ToString(decimalPlaces, formatProvider)));

                if (i == 0) { initialIndex = false; }
            }
        }
        #endregion

        #region Load Functions
        public void LoadPositions(ref RecordedInfo<RopeTransformToRecord> rec,bool makePhysic)
        {
            int j = 0;
            if (rec == recordRope) {j = index;}

            ObiParticleAttachment[] attach = rope.GetComponents<ObiParticleAttachment>();

            for (int i = 0; i < attach.Length; i++) {
                SaveAttach(attach[i]);
                Destroy(attach[i]);
            }

            float lenght = StringToFloat(rec.RecordObjectStore[j].l);

            if ((rope as ObiRope) && rope.restLength != lenght) {
                ExtendRope(rope as ObiRope,lenght);
            }

            int particleCount = rec.RecordObjectStore[j].pC;

            rope.gameObject.SetActive(rec.RecordObjectStore[j].e);
            for (int i = 0; i < particleCount; i++) {
                rope.solver.invMasses[rope.solverIndices[i]] = 0;

                rope.solver.positions[rope.solverIndices[i]] = 
                    StringToVector4(rec.RecordObjectStore[j + i].p);

                rope.solver.orientations[rope.solverIndices[i]] = 
                   StringToQuaternion(rec.RecordObjectStore[j + i].r);

                if (makePhysic) {
                    rope.solver.invMasses[rope.solverIndices[i]] =
                       StringToFloat(rec.RecordObjectStore[j + i].iPM);

                    rope.solver.invRotationalMasses[rope.solverIndices[i]] =
                       StringToFloat(rec.RecordObjectStore[j + i].iRM);
                }
            }

            if (makePhysic) { ReturnAttachments(); }

            folderPath = null;
            loading = false;
        }
        #endregion

        #region Support Functions
        private Vector4 StringToVector4(string rotation)
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            string[] axis = rotation.Split(",");

            float x = float.Parse(axis[0].Replace("(", "").Trim(), formatProvider);
            float y = float.Parse(axis[1].Trim(), formatProvider);
            float z = float.Parse(axis[2].Trim(), formatProvider);
            float w = float.Parse(axis[3].Replace(")", "").Trim(), formatProvider);

            return new Vector4(x, y, z, w);
        }
        private void ExtendRope(ObiRope rope,float lenght)
        {
            ObiRopeCursor cursor = rope.GetComponent<ObiRopeCursor>();

            if (cursor) {
                cursor.ChangeLength(lenght);
            }
        }
        private float StringToFloat(string lenght) 
        {
            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;
            NumberStyles style = NumberStyles.Float;
            float result;

            float.TryParse(lenght.Trim(), style, formatProvider,out result);

            return result;
        }
        private void SaveAttach(ObiParticleAttachment attach)
        {
            Attachments saveAttach = new Attachments();

            saveAttach.target = attach.target;
            saveAttach.particleGroup = attach.particleGroup;
            saveAttach.attachmentType = attach.attachmentType;

            if (saveAttach.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic) {
                saveAttach.compliance = attach.compliance;
                saveAttach.breakThreshold = attach.breakThreshold;
            }

            destroyedAttachments.Add(saveAttach);
        }
        public void ReturnAttachments()
        {
            foreach (Attachments attach in destroyedAttachments)
            {
                ObiParticleAttachment newAttach = rope.gameObject.AddComponent<ObiParticleAttachment>();

                newAttach.target = attach.target;
                newAttach.particleGroup = attach.particleGroup;
                newAttach.attachmentType = attach.attachmentType;

                if (newAttach.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic)
                {
                    newAttach.compliance = attach.compliance;
                    newAttach.breakThreshold = attach.breakThreshold;
                }
            }
        }
        public override void SaveOrLoadSetup()
        {
           base.SaveOrLoadSetup();
           recordRope.Name = rope.sourceBlueprint.name;
        }
        public override string FindCorrectFileOnFolder(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] fileInfos = info.GetFiles();

            foreach (FileInfo fileInfo in fileInfos)
            {
                StreamReader sr = new StreamReader(fileInfo.FullName);
                string jsonstring = sr.ReadToEnd();
                sr.Close();

                recordRope = JsonUtility.FromJson<RecordedInfo<RopeTransformToRecord>>(jsonstring);
                if (recordRope.Name == rope.sourceBlueprint.name)
                {
                    path = fileInfo.FullName;
                    return path;
                }
            }

            return null;
        }
        public override IEnumerator ReadPathFromFile()
        {
            if (File.Exists(folderPath))
            {
                StreamReader sr = new StreamReader(folderPath);
                string jsonstring = sr.ReadToEnd();
                sr.Close();

                recordRopePosition = JsonUtility.FromJson<RecordedInfo<RopeTransformToRecord>>(jsonstring);
            }

            yield return null;
        }
        #endregion
    }
}
