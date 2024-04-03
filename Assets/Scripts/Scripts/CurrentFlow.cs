using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentFlow : MonoBehaviour {   

        List<Rigidbody> rigidbodiesInWindzoneList = new List<Rigidbody>();
        Vector3 windDirection = Vector3.back;
        //Vector3 windDirection2 = Vector3.up;
    public float windStrength;

        private void OnTriggerEnter(Collider col)
        {
            Rigidbody objectRigid = col.gameObject.GetComponent<Rigidbody>();

            if (objectRigid != null)
                rigidbodiesInWindzoneList.Add(objectRigid);

        }

        private void OnTriggerExit(Collider col)
        {
            Rigidbody objectRigid = col.gameObject.GetComponent<Rigidbody>();

            if (objectRigid != null)
                rigidbodiesInWindzoneList.Remove(objectRigid);
        }

        private void FixedUpdate()
        {
            if (rigidbodiesInWindzoneList.Count > 0)
            {
                foreach (Rigidbody rigid in rigidbodiesInWindzoneList)
                {
                    rigid.AddForce(windDirection * windStrength);
                    //rigid.AddForce(windDirection2 * windStrength);
            }
            }
        }
    }
