#define HandleBoth
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
#if HandleBoth
            return Input.touchCount > 0 ? Input.GetTouch(0).position : Vector3.zero + Input.mousePosition;
#endif
#if UNITY_ANDROID
            return Input.touchCount > 0 ? Input.GetTouch(0).position : Vector3.zero;
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
#if HandleBoth
        return (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Began) || Input.GetMouseButtonDown(0);
#endif
#if UNITY_ANDROID
        return Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Began;
#else
        return Input.GetMouseButtonDown(0);
#endif
    }
    private static bool CheckSecondaryInput()
    {
#if HandleBoth
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(1);
#endif
#if UNITY_ANDROID
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#else
        Input.return GetMouseButtonDown(1);
#endif
    }

}
