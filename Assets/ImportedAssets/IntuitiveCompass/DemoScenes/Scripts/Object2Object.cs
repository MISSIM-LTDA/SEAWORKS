using UnityEngine;
using System.Collections;

//UnitySpace.ru
//Script by Sagleft

public class Object2Object : MonoBehaviour {
	public GameObject Target;
	
	public bool Transform = true;
	public bool Evenly = false;
	public bool Resize = false;
	public bool EvenlyResize = false;
	public bool Activated = true;
	public bool VectorInsteadObject = false;
	public bool LocalVector = false;
	
	public Vector3 Vector;
	public Vector3 NewVector;
	
	public float HalfTime = 2.0f;
	public float ResizingTime = 2.5f;
	private float Correction2 = 1;
	
	private bool Yes = true;
	private bool No = false;
	private bool ToObject = true;
	private bool IncreaseSize = true;
	
	private Vector3 StartPos;
	private Vector3 EndPos;
	private Vector3 PathVector;
	private Vector3 VelocityVector;
	private Vector3 StartSize;
	private Vector3 Resizing;
	private Vector3 CurrentSize;
	
	private float NumStartSize;
	private float PathLength;
	private float StepLength;
	private float NumResizing;
	private float InstantaneousVelocity;
	private float SizeDat;
	private float Acceleration;
	private float Acceleration2;
	private float Param;
	private float Param2;
	private float AmplCorrection = 1.58f;
	private float ResizingStep;
	
	private int Steps;
	private int CurrentStep = 0;
	private int ResizingSteps;
	private int CurrentResizingStep = 0;
	
	public void FunctionOff () {
		Activated = No;
	}
	
	public void FunctionOn () {
		Activated = Yes;
	}
	
	void Start () {
		if (Transform == Yes) {
			if (VectorInsteadObject == Yes) {
				if (LocalVector == Yes) {
					EndPos = transform.position + Vector;
				} else {
					EndPos = Vector;
				}
			} else {
				EndPos = Target.transform.position;
			}
			StartPos = transform.position;
			PathVector = StartPos - EndPos;
			PathLength = PathVector.magnitude;
			InstantaneousVelocity = PathLength / HalfTime;
			StepLength = InstantaneousVelocity * Time.deltaTime;
			Steps = (int) (PathLength / StepLength);
			Param = Mathf.PI / Steps;
		}
		if (Resize == Yes) {
			StartSize = transform.localScale;
			NumStartSize = StartSize.magnitude;
			Resizing = NewVector - StartSize;
			NumResizing = Resizing.magnitude;
			SizeDat = NumResizing / ResizingTime;
			ResizingStep = SizeDat * Time.deltaTime;
			ResizingSteps = (int) (NumResizing / ResizingStep);
			Param2 = Mathf.PI / ResizingSteps;
		}
	}
	
	void Update () {
		if (Activated == Yes) {
			if (Transform == Yes) {
				if (ToObject == Yes) {
					if (CurrentStep <= Steps) {
						if (Evenly == No) {
							Acceleration = Mathf.Abs(AmplCorrection * Mathf.Sin(Param * CurrentStep));
						} else {
							Acceleration = 1;
						}
						transform.position -= PathVector.normalized * StepLength * Acceleration;
						CurrentStep += 1;
					} else {
						CurrentStep = 0;
						ToObject = No;
					}
				} else {
					if (CurrentStep <= Steps) {
						if (Evenly == No) {
							Acceleration = Mathf.Abs(AmplCorrection * Mathf.Sin(Param * CurrentStep));
						} else {
							Acceleration = 1;
						}
						transform.position += PathVector.normalized * StepLength * Acceleration;
						CurrentStep += 1;
					} else {
						CurrentStep = 0;
						ToObject = Yes;
					}
				}
			}
			if (Resize == Yes) {
				if (IncreaseSize == Yes) {
					if (CurrentResizingStep <= ResizingSteps) {
						if (EvenlyResize == No) {
							Acceleration2 = Mathf.Abs(Correction2 * Mathf.Sin(Param2 * CurrentResizingStep));
						} else {
							Acceleration2 = 1;
						}
						CurrentSize = Resizing.normalized * (NumStartSize + ResizingStep * CurrentResizingStep * Acceleration2);
						transform.localScale = CurrentSize;
						CurrentResizingStep += 1;
					} else {
						CurrentResizingStep = 0;
						IncreaseSize = No;
					}
				} else {
					if (CurrentResizingStep <= ResizingSteps) {
						if (EvenlyResize == No) {
							Acceleration2 = Mathf.Abs(Correction2 * Mathf.Sin(Param2 * CurrentResizingStep));
						} else {
							Acceleration2 = 1;
						}
						CurrentSize = Resizing.normalized * (NumStartSize + ResizingStep * CurrentResizingStep * Acceleration2);
						transform.localScale = CurrentSize;
						CurrentResizingStep += 1;
					} else {
						CurrentResizingStep = 0;
						IncreaseSize = Yes;
					}
				}
			}
		}
	}
}
