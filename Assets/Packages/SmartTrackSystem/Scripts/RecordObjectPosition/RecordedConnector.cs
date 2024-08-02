using Obi;
using SmartTrackSystem;
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

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
                (body.gameObject,
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

        if (transform.tag == "EFL_Parent") {

            if(i%2 != 0) { i++; }

            Transform handle = transform.GetChild(0);
            Transform body = transform.GetChild(1);

            handle.GetComponent<Rigidbody>().isKinematic = true;
            body.GetComponent<Rigidbody>().isKinematic = true;

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
                handle.GetComponent<Rigidbody>().isKinematic = false;
                body.GetComponent<Rigidbody>().isKinematic = false;
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
}
