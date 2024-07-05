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
        public Transform startOfRope;
        public Transform endOfRope;
        public class Attachments
        {
            public Transform target = null;
            public ObiParticleGroup particleGroup = null;
            public ObiParticleAttachment.AttachmentType attachmentType = ObiParticleAttachment.AttachmentType.Static;
            public float compliance = 0.0f;
            public float breakThreshold = float.PositiveInfinity;
        }

        List<Attachments> destroyedAttachments = new List<Attachments>();

        #region Save Functions
        public override void GetObjectPosition()
        {
            bool initialIndex = true;

            int particleCount = rope.activeParticleCount;
            float lenght = 0.0f;

            if (rope.GetComponent<ObiRope>() != null) {
                lenght = rope.GetComponent<ObiRope>().restLength;
            }

            IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

            if (startOfRope != null) 
            {
                if (startOfRope.tag == "EFL_Parent")
                {
                    Transform start1 = startOfRope.GetChild(0);
                    Transform start2 = startOfRope.GetChild(1);

                    record.RecordRopeStore.Add(new RopeTransformToRecord
                        (initialIndex,start1.gameObject.activeSelf,
                        start1.localPosition.ToString(decimalPlaces, formatProvider), 
                        start1.localRotation.ToString(decimalPlaces, formatProvider)));
                    record.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,start2.gameObject.activeSelf,
                        start2.localPosition.ToString(decimalPlaces, formatProvider), 
                        start2.localRotation.ToString(decimalPlaces, formatProvider)));

                    initialIndex = false;
                }

                else
                {
                    record.RecordRopeStore.Add(new RopeTransformToRecord
                        (initialIndex,startOfRope.gameObject.activeSelf,
                        startOfRope.localPosition.ToString(decimalPlaces,formatProvider), 
                        startOfRope.localRotation.ToString(decimalPlaces,formatProvider)));

                    initialIndex = false;
                }
            }

            ObiSolver solver = rope.solver;

            for (int i = 0; i < particleCount; i++)
            {
                record.RecordRopeStore.Add(new RopeTransformToRecord
                (initialIndex,gameObject.activeSelf,
                rope.solver.positions[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                rope.solver.orientations[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                solver.invMasses[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                solver.invRotationalMasses[rope.solverIndices[i]].ToString(decimalPlaces,formatProvider),
                particleCount, lenght.ToString(decimalPlaces,formatProvider)));

                if (i == 0) { initialIndex = false; }
            }

            if(endOfRope != null) 
            {
                if (endOfRope.tag == "EFL_Parent")
                {
                    Transform end1 = endOfRope.transform.GetChild(0);
                    Transform end2 = endOfRope.transform.GetChild(1);

                    record.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,end1.gameObject.activeSelf, 
                        end1.localPosition.ToString(decimalPlaces,formatProvider),
                        end1.localRotation.ToString(decimalPlaces,formatProvider)));
                    record.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,end2.gameObject.activeSelf, 
                        end2.localPosition.ToString(decimalPlaces,formatProvider),
                        end2.localRotation.ToString(decimalPlaces,formatProvider)));
                }

                else
                {
                    record.RecordRopeStore.Add(new RopeTransformToRecord
                        (false,endOfRope.gameObject.activeSelf,
                        endOfRope.localPosition.ToString(decimalPlaces,formatProvider), 
                        endOfRope.localRotation.ToString(decimalPlaces,formatProvider)));
                }
            }
        }

        #endregion

        #region Load Functions
        protected override IEnumerator LoadNewPathCoroutine(string path)
        {
            if (path == null) { yield return StartCoroutine(ChooseFile()); }

            else { folderPath = FindCorrectFileOnFolder(path); }

            yield return StartCoroutine(ReadPathFromFile());

            if (record.Name != rope.sourceBlueprint.name)
            {
                Debug.Log("Tried to load a wrong file to this object");
                loading = false;
            }

            else
            {
                if (record.RecordObjectStore.Count == 0)
                {
                    Debug.Log("Problem Loading object position from File");
                    loading = false;
                }

                else { LoadPositions(true); }

                positionHelper.SelectedObject = null;
            }
        }
        public override void LoadPositions(bool makePhysic)
        {
            ObiParticleAttachment[] attach = rope.GetComponents<ObiParticleAttachment>();

            for (int i = 0; i < attach.Length; i++)
            {
                SaveAttach(attach[i]);
                Destroy(attach[i]);
            }

            float lenght = StringToFloat(record.RecordRopeStore[index].l);

            ObiRope obiRope = rope.GetComponent<ObiRope>();
            if (obiRope != null && obiRope.restLength != lenght){
                ExtendRope(obiRope,lenght);
            }

            int particleCount = record.RecordRopeStore[index].pC;

            if (startOfRope != null)
            {
                if (startOfRope.tag == "EFL_Parent")
                {
                    Transform start1 = startOfRope.GetChild(0);
                    Transform start2 = startOfRope.GetChild(1);

                    start1.GetComponent<Rigidbody>().isKinematic = true;
                    start2.GetComponent<Rigidbody>().isKinematic = true;

                    start1.gameObject.SetActive(record.RecordRopeStore[index].e);
                    SetLocalPositionAndRotation(start1,
                    StringToVector3(record.RecordRopeStore[index].p),
                    StringToQuaternion(record.RecordRopeStore[index].r));

                    start2.gameObject.SetActive(record.RecordRopeStore[index + 1].e);
                    SetLocalPositionAndRotation(start2,
                    StringToVector3(record.RecordRopeStore[index + 1].p),
                    StringToQuaternion(record.RecordRopeStore[index + 1].r));

                    if (makePhysic)
                    {
                        start1.GetComponent<Rigidbody>().isKinematic = false;
                        start2.GetComponent<Rigidbody>().isKinematic = false;
                    }

                    index += 2;
                }

                else
                {
                    startOfRope.gameObject.SetActive(record.RecordRopeStore[index].e);
                    SetLocalPositionAndRotation(startOfRope,
                    StringToVector3(record.RecordRopeStore[index].p),
                    StringToQuaternion(record.RecordRopeStore[index].r));

                    index++;
                }
            }

            rope.gameObject.SetActive(record.RecordRopeStore[index].e);
            for (int i = 0; i < particleCount; i++)
            {
                rope.solver.invMasses[rope.solverIndices[i]] = 0;

                rope.solver.positions[rope.solverIndices[i]] = 
                    StringToVector4(record.RecordRopeStore[index + i].p);
                rope.solver.orientations[rope.solverIndices[i]] = 
                   StringToQuaternion(record.RecordRopeStore[index + i].r);

                if (makePhysic)
                {
                    rope.solver.invMasses[rope.solverIndices[i]] =
                       StringToFloat(record.RecordRopeStore[index + i].iPM);
                    rope.solver.invRotationalMasses[rope.solverIndices[i]] =
                       StringToFloat(record.RecordRopeStore[index + i].iRM);
                }
            }

            index += particleCount;

            if (endOfRope != null)
            {
                if (endOfRope.tag == "EFL_Parent")
                {
                    Transform end1 = endOfRope.GetChild(0);
                    Transform end2 = endOfRope.GetChild(1);

                    end1.GetComponent<Rigidbody>().isKinematic = true;
                    end2.GetComponent<Rigidbody>().isKinematic = true;

                    end1.gameObject.SetActive(record.RecordRopeStore[index].e);
                    SetLocalPositionAndRotation(end1,
                    StringToVector3(record.RecordRopeStore[index].p),
                    StringToQuaternion(record.RecordRopeStore[index].r));

                    end2.gameObject.SetActive(record.RecordRopeStore[index + 1].e);
                    SetLocalPositionAndRotation(end2,
                    StringToVector3(record.RecordRopeStore[index + 1].p),
                    StringToQuaternion(record.RecordRopeStore[index + 1].r));

                    if (makePhysic)
                    {
                        end1.GetComponent<Rigidbody>().isKinematic = false;
                        end2.GetComponent<Rigidbody>().isKinematic = false;
                    }

                    index += 2;
                }

                else
                {
                    endOfRope.gameObject.SetActive(record.RecordRopeStore[index].e);
                    SetLocalPositionAndRotation(endOfRope,
                    StringToVector3(record.RecordRopeStore[index].p),
                    StringToQuaternion(record.RecordRopeStore[index].r));

                    index++;
                }
            }

            if (makePhysic) { ReturnAttachments(); }

            folderPath = null;
            loading = false;
        }

        #endregion

        #region Support Functions
        private void ExtendRope(ObiRope rope,float lenght)
        {
            ObiRopeCursor cursor = rope.GetComponent<ObiRopeCursor>();

            if (cursor != null){
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
        private void SaveAttach(ObiParticleAttachment attach)
        {
            Attachments saveAttach = new Attachments();

            saveAttach.target = attach.target;
            saveAttach.particleGroup = attach.particleGroup;
            saveAttach.attachmentType = attach.attachmentType;

            if (saveAttach.attachmentType == ObiParticleAttachment.AttachmentType.Dynamic)
            {
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
        protected override void SaveOrLoadSetup()
        {
            base.SaveOrLoadSetup();
            record.Name = rope.sourceBlueprint.name;
        }
        protected override string FindCorrectFileOnFolder(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            FileInfo[] fileInfos = info.GetFiles();

            foreach (FileInfo fileInfo in fileInfos)
            {
                StreamReader sr = new StreamReader(fileInfo.FullName);
                string jsonstring = sr.ReadToEnd();
                sr.Close();

                record = JsonUtility.FromJson<RecordedObjectInfo>(jsonstring);
                if (record.Name == rope.sourceBlueprint.name)
                {
                    path = fileInfo.FullName;
                    return path;
                }
            }

            return null;
        }

        #endregion
    }
}
