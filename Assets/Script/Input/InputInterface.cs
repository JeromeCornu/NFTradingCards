using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputInterface
{
    internal static Vector3 mousePosition
    {
        get
        {
#if UNITY_ANDROID
#if UNITY_EDITOR
            return Input.touchCount > 0 ? Input.GetTouch(0).position : Vector3.zero + Input.mousePosition;
#else
            return Input.touchCount > 0 ? Input.GetTouch(0).position : Vector3.zero;
#endif
#else
            return Input.mousePosition;
#endif
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputIndex">0 is main, 1 is secondary, 2 is tertiary (not implemented)...</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal static bool GetInputDown(int inputIndex)
    {
        if (inputIndex == 0)
            return CheckMainInput();
        else if (inputIndex == 1)
            return CheckSecondaryInput();
        else
            throw new NotImplementedException("Input interface doesn't handle " + inputIndex + " index of  click currently");
    }


    private static bool CheckMainInput()
    {
#if UNITY_ANDROID
#if UNITY_EDITOR
        return FingerRemains() || Input.GetMouseButtonDown(0);
#else
        return FingerRemains();
#endif
#else
        return Input.GetMouseButtonDown(0);
#endif
    }

    private static bool FingerRemains()
    {
        if (Input.touchCount > 0)
        {
            var phase = Input.GetTouch(0).phase;
            //Every valid phase excpet for began
            return phase == TouchPhase.Stationary || phase == TouchPhase.Moved;
        }
        else
            return false;
    }

    private static bool CheckSecondaryInput()
    {
#if UNITY_ANDROID
#if UNITY_EDITOR
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(1);
#else
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#endif
#else
        Input.return GetMouseButtonDown(1);
#endif
    }

}
