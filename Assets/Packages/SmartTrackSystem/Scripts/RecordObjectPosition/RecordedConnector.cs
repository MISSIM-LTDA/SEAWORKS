using Obi;
using SmartTrackSystem;
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class RecordedConnector : RecordedObject
{
    [HideInInspector] public ObiRopeBase rope;
    [HideInInspector] public RecordedConnector otherConnector;
    public override void Update()
    {
        if (mouseOver && Input.GetMouseButtonDown(0))
        {
            SelectObject(rope.gameObject);
        }
    }

    #region Select
    public override void SelectObject(GameObject selectedObject)
    {
        base.SelectObject(selectedObject);

        PlaceOutlineOnMesh(rope.transform);
        PlaceOutlineOnMesh(otherConnector.transform);
    }
    public override IEnumerator Unselect()
    {
        unselecting = true;

        yield return new WaitUntil(() => Input.GetMouseButton(0) && !mouseOver && !otherConnector.mouseOver && !DetectButtonOver());

        foreach (Outline outline in outlines) { Destroy(outline); }

        button.image.color = Color.cyan;

        unselecting = false;
    }
    #endregion

    #region Save
    public override void GetObjectPosition(ref RecordedInfo<ObjectTransformToRecord> rec)
    {
        IFormatProvider formatProvider = CultureInfo.InvariantCulture.NumberFormat;

        if (transform.tag == "EFL_Parent") {
            Transform handle = transform.GetChild(0);
            Transform body = transform.GetChild(1);

            rec.RecordObjectStore.Add(new ObjectTransformToRecord
                (handle.gameObject.activeSelf,
                handle.localPosition.ToString(decimalPlaces, formatProvider),
                handle.localRotation.ToString(decimalPlaces, formatProvider)));
            rec.RecordObjectStore.Add(new ObjectTransformToRecord
                (body.gameObject.activeSelf,
                body.localPosition.ToString(decimalPlaces, formatProvider),
                body.localRotation.ToString(decimalPlaces, formatProvider)));
        }

        else {
            rec.RecordObjectStore.Add(new ObjectTransformToRecord
                (gameObject.activeSelf,
                transform.localPosition.ToString(decimalPlaces, formatProvider),
                transform.localRotation.ToString(decimalPlaces, formatProvider)));
        }
    }
    #endregion

    #region Load
    public override void LoadPositions(ref RecordedInfo<ObjectTransformToRecord> rec, bool makePhysics)
    {
        int i = 0;
        if (rec == record) {i = index;}

        if (transform.CompareTag("EFL_Parent")) {
            i *= 2;

            Transform handle = transform.GetChild(0);
            Rigidbody handleRigidbody = handle.GetComponent<Rigidbody>();

            Transform body = transform.GetChild(1);
            Rigidbody bodyRigidbody = body.GetComponent<Rigidbody>();

            handleRigidbody.isKinematic = true;
            bodyRigidbody.isKinematic = true;

            handle.gameObject.SetActive(rec.RecordObjectStore[i].e);

            SetLocalPositionAndRotation(handle,
                StringToVector3(rec.RecordObjectStore[i].p),
                StringToQuaternion(rec.RecordObjectStore[i].r));

            i++;

            body.gameObject.SetActive(rec.RecordObjectStore[i].e);

            SetLocalPositionAndRotation(body,
                StringToVector3(rec.RecordObjectStore[i].p),
                StringToQuaternion(rec.RecordObjectStore[i].r));

            if (makePhysics) {
                handle.GetComponent<GrabObject>().enabled = false;

                DestroyImmediate(handle.GetComponent<FixedJoint>());
                handleRigidbody.isKinematic = false;

                handle.gameObject.AddComponent<FixedJoint>();

                handle.GetComponent<GrabObject>().enabled = true;

                DestroyImmediate(body.GetComponent<HingeJoint>());
                bodyRigidbody.isKinematic = false;

                AddHingeJoint(body, handleRigidbody);
            }
        }

        else {
            gameObject.SetActive(rec.RecordObjectStore[i].e);

            SetLocalPositionAndRotation(transform,
                StringToVector3(rec.RecordObjectStore[i].p),
                StringToQuaternion(rec.RecordObjectStore[i].r));
        }
    }
    #endregion

    #region Support Functions
    private void AddHingeJoint(Transform hingeBody,Rigidbody connectedBody) 
    {
        HingeJoint hinge = hingeBody.gameObject.AddComponent<HingeJoint>();
        hinge.connectedBody = connectedBody;
        hinge.anchor = new Vector3(0.39f, -0.04f, -0.18f);
        hinge.axis = new Vector3(0, 0, 1);

        JointSpring hingeSpring = hinge.spring;
        hingeSpring.spring = 400;
        hinge.spring = hingeSpring;
        hinge.useSpring = true;
    }
    #endregion
}
