using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainCompass : MonoBehaviour {
	
	public Vector3 Destination;
	public bool ObjectInstead = true;
	public GameObject Target;
	
	public GameObject CompassArrows;
	public bool DrawDebugRay = false;
	public bool ShowDegrees = false;
	public Text DegreesObject;
	
	private float PrevRotation;
	private float NewRotation;
	private float CompassRotation;
	private Vector3 RelPos;
	private Vector2 RelPosXZ;
	private Vector2 RayDirXZ;
	private Vector3 CamDir;
	//private Vector3 Fix;
	private Vector3 normal;
	private Vector3 RelPosXYZ;
	private Vector3 RayDirXYZ;
	private Image CompassImage;
	private Image ArrowsImage;
	private bool Hided = false;
	private bool ErrorFounded = false;
	
	private Transform CompassParent;
	
	void Start() {
		//Fix = new Vector3(367, 209, 0);
		CompassParent = CompassArrows.transform.parent;
		CompassImage = CompassParent.gameObject.GetComponent<Image>();
		ArrowsImage = CompassArrows.GetComponent<Image>();
	}
	
	public void RotateCompass() {
		//CamDir = Camera.main.transform.forward.magnitude * Fix;
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		if (DrawDebugRay) {
			Debug.DrawRay(ray.origin, ray.direction*200, Color.black);
		}
		
		RelPos = Destination - Camera.main.transform.position;
		RelPosXZ = new Vector2(RelPos.x, RelPos.z);
		RayDirXZ = new Vector2(ray.direction.x, ray.direction.z);
		
		
		RelPosXYZ = new Vector3(RelPos.x, RelPos.z, 0);
		RayDirXYZ = new Vector3(ray.direction.x, ray.direction.z, 0);
		
		Vector3 normal = Vector3.Cross(RelPosXYZ, RayDirXYZ);
		
		if (normal.z < 0) {
			NewRotation = Vector2.Angle(RelPosXZ, RayDirXZ);
		} else {
			NewRotation = 360 - Vector2.Angle(RelPosXZ, RayDirXZ);
		}
		
		if (PrevRotation != NewRotation && ErrorFounded == false) {
			CompassRotation = Mathf.Abs(NewRotation) - Mathf.Abs(PrevRotation);
			PrevRotation = NewRotation;
			if (CompassArrows != null) {
				CompassArrows.transform.Rotate(Vector3.forward, CompassRotation);
			} else {
				ErrorFounded = true;
				Debug.Log("Object 'arrows' is not assigned");
			}
		}
		if (ShowDegrees == true && ErrorFounded == false) {
			if (DegreesObject != null) {
				DegreesObject.text = Mathf.Round(NewRotation).ToString();
			} else {
				ErrorFounded = true;
				Debug.Log("No object is selected to display the number of degrees");
			}
		}
	}
	
	void Update() {
		if (ObjectInstead && ErrorFounded == false) {
			if (Target == null) {
				ErrorFounded = true;
				Debug.Log("Object 'north' is not assigned");
			}
			Destination = Target.transform.position;
		}
		if (ErrorFounded == false) {
			RotateCompass();
		}
	}
	
	public void HideCompass() {
		if (Hided == false) {
			CompassParent.gameObject.SetActive(false);
			Hided = true;
		}
	}
	
	public void ShowCompass() {
		if (Hided == true) {
			CompassParent.gameObject.SetActive(true);
			CompassImage.color = Color.white;
			ArrowsImage.color = Color.white;
			Hided = false;
		}
	}
	
	public void SetNorthDir(Vector3 dir) {
		Destination = dir;
		ObjectInstead = false;
	}
	
	public void SetNorthObject(GameObject NorthObject) {
		Target = NorthObject;
		ObjectInstead = true;
	}
}
