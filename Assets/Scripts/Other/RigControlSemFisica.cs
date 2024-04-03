using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class RigControlSemFisica : MonoBehaviour
{
    //------------------------------------------------------------------------------------------------------------------------
    #region Base Variables
    [Header("Base")]
    public GameObject Base;
    public Vector3 Base_Axis = new Vector3(0,1,0);

    public float Base_LargestAngleLimit = 40;
    public float Base_SmallestAngleLimit = -90;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Shoulder Variables
    [Header("Shoulder")]
    public GameObject Shoulder;
    public Vector3 Shoulder_Axis = new Vector3(1,0,0);

    public float Shoulder_LargestAngleLimit = 70;
    public float Shoulder_SmallestAngleLimit = -30;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Forearm Variables
    [Header("Forearm")]
    public GameObject ForeArm;
    public Vector3 ForeArm_Axis = new Vector3(1,0,0);

    public float ForeArm_LargestAngleLimit  = 90;
    public float ForeArm_SmallestAngleLimit = -90;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Yaw Variables
    [Header("Yaw")]
    public GameObject Yaw;
    public Vector3 Yaw_Axis = new Vector3(1,0,0);

    public float Yaw_LargestAngleLimit   =  70;
    public float Yaw_SmallestAngleLimit  = -90;
    #endregion

    //------------------------------------------------------------------------------------------------------------------------
    #region Pitch Variables;
    [Header("Pitch")]
    public GameObject Pitch;
    public Vector3 Pitch_Axis = new Vector3(0,1,0);

    public float Pitch_LargestAngleLimit  =   70;
    public float Pitch_SmallestAngleLimit = -100;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Arduino
    [Header("Arduino")]
    public ReadArduino ReadArduino;
    int[] RecievedData;
    public int BatchSize = 30;
    int VectorIndex = 0;
    public int[] Data;
    float Cur_ControllerValue;
    float Last_ControllerValue;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Debug
    [Header("Debug")]
    public float AngularSpeed = 1;
    public bool BlockMovements  = false;
    public bool ReadFromArduino = true;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Mode Information
    [Header("Extras")]
    public KeyCode Next_Mode = KeyCode.O;
    public KeyCode Prev_Mode = KeyCode.I;

    int ModeIndex;
    public enum MovementMode
    {
        Base,
        Shoulder,
        Forearm,
        Yaw,
        Pitch
    }
    Array modes = Enum.GetValues(typeof(MovementMode));
    public MovementMode Cur_Mode;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    #region Target Angles
    [Header("Target Angles")]
    public float Base_TargetAngle       =  0;
    public float Shoulder_TargetAngle   =  0;
    public float Forearm_TargetAngle    = 90;
    public float Yaw_TargetAngle        =-90;
    public float Pitch_TargetAngle      =  0;
    #endregion
    //------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        Data = new int[BatchSize];
    }
    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }
    public void GoToNormalAngles()
    {
        Base_TargetAngle     =   0;
        Shoulder_TargetAngle =   0;
        Forearm_TargetAngle  =  90;
        Yaw_TargetAngle      = -90;
        Pitch_TargetAngle    =   0;
    }
    private void GoToAngle(GameObject obj, float objAngle, float targetAngle, float largestAngleLimit, float smallestAngleLimit, Vector3 RotationAxis)
    {
        if(targetAngle > largestAngleLimit)
            targetAngle = largestAngleLimit;
        else if (targetAngle < smallestAngleLimit)
            targetAngle = smallestAngleLimit;

        objAngle = WrapAngle(objAngle);

        int dir;
        if (objAngle > targetAngle)
            dir = -1;
        else
            dir = 1;

        if (targetAngle - 0.5 <= objAngle && objAngle <= targetAngle + 0.5)
            return;
        
        Vector3 Axis = RotationAxis * dir;
        obj.transform.Rotate(Axis, AngularSpeed);
    }
    private float Moving_mean(int recievedData)
    {
        Data[VectorIndex] = recievedData;

        VectorIndex += 1;
        if (VectorIndex == BatchSize)
            VectorIndex = 0;

        int sum = 0;
        for (int i = 0; i < Data.Length; i++)
            sum += Data[i];

        return sum / Data.Length;
    }
    void CheckMovementMode()
    {
        if (Input.GetKeyDown(Next_Mode))
            ModeIndex++;
        else if (Input.GetKeyDown(Prev_Mode))
            ModeIndex--;

        if (ModeIndex > 4)
            ModeIndex = 0;
        else if (ModeIndex < 0)
            ModeIndex = 4;

        Cur_Mode = (MovementMode)modes.GetValue(ModeIndex);
    }
    private void Update()
    {
        CheckMovementMode();
        if (ReadFromArduino)
        {
            RecievedData = ReadArduino.datas;
            Cur_ControllerValue = Moving_mean(RecievedData[1]);

            float ValueDisplamecent = Last_ControllerValue - Cur_ControllerValue;
            float AngleDisplacement = -ValueDisplamecent * 360 / 1024;
            if (Cur_Mode == MovementMode.Base)
            {
                Base_TargetAngle += AngleDisplacement;
                Base_TargetAngle = Math.Clamp(Base_TargetAngle, Base_SmallestAngleLimit, Base_LargestAngleLimit);
            }   
            else if (Cur_Mode == MovementMode.Shoulder)
            {
                Shoulder_TargetAngle += AngleDisplacement;
                Shoulder_TargetAngle = Math.Clamp(Shoulder_TargetAngle, Shoulder_SmallestAngleLimit, Shoulder_LargestAngleLimit);
            }
            else if (Cur_Mode == MovementMode.Forearm)
            {
                Forearm_TargetAngle += AngleDisplacement;
                Forearm_TargetAngle = Math.Clamp(Forearm_TargetAngle, ForeArm_SmallestAngleLimit, ForeArm_LargestAngleLimit);
            }   
            else if (Cur_Mode == MovementMode.Yaw)
            {
                Yaw_TargetAngle += AngleDisplacement;
                Yaw_TargetAngle = Math.Clamp(Yaw_TargetAngle, Yaw_SmallestAngleLimit, Yaw_LargestAngleLimit);
            }   
            else if (Cur_Mode == MovementMode.Pitch)
            {
                Pitch_TargetAngle += AngleDisplacement;
                Pitch_TargetAngle = Math.Clamp(Pitch_TargetAngle, Pitch_SmallestAngleLimit, Pitch_LargestAngleLimit);
            }
                

            Last_ControllerValue = Cur_ControllerValue;
        }

        if (BlockMovements)
            return;

        GoToAngle(Base,       Base.transform.localEulerAngles.y,      Base_TargetAngle,       Base_LargestAngleLimit,       Base_SmallestAngleLimit,        Base_Axis);
        GoToAngle(Shoulder,   Shoulder.transform.localEulerAngles.x,  Shoulder_TargetAngle,   Shoulder_LargestAngleLimit,   Shoulder_SmallestAngleLimit,    Shoulder_Axis);
        GoToAngle(ForeArm,    ForeArm.transform.localEulerAngles.x,   Forearm_TargetAngle,    ForeArm_LargestAngleLimit,    ForeArm_SmallestAngleLimit,     ForeArm_Axis);
        GoToAngle(Yaw,        Yaw.transform.localEulerAngles.x,       Yaw_TargetAngle,        Yaw_LargestAngleLimit,        Yaw_SmallestAngleLimit,         Yaw_Axis);
        GoToAngle(Pitch,      Pitch.transform.localEulerAngles.y,     Pitch_TargetAngle,      Pitch_LargestAngleLimit,      Pitch_SmallestAngleLimit,       Pitch_Axis);
    }
}