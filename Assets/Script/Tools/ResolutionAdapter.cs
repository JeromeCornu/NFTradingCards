using System.Collections;
using UnityEngine;

public class ResolutionAdapter : MonoBehaviour
{
    [SerializeField]
    private Transform _backgroundToScale;
    private void Awake()
    {
        Debug.Log(Screen.resolutions.Length + " total supported resolutions, list : " + string.Join(',', Screen.resolutions));
        /*var last = Screen.resolutions[Screen.resolutions.Length - 1];
        Screen.SetResolution(last.width, last.height, true);
        yield return new WaitForSeconds(3f);*/
        Screen.SetResolution(1920, 1080, true);
        AdjustDepth();
    }
    void AdjustDepth(Vector2 _originalScreenSize, Vector2 _originalScale, Vector2 _adjustedScreenSize, Vector2 _adjustedScale)
    {
        //We use max because sometimes this get called before or after landscape mode has been forced that inverts things
        var x = LerpRelative(_originalScreenSize.x, _adjustedScreenSize.x, _originalScale.x, _adjustedScale.x, Mathf.Max(Screen.width,Screen.height));
        //var y = LerpRelative(_originalScreenSize.y, _adjustedScreenSize.y, _originalScale.y, _originalScale.y, Screen.height);
        _backgroundToScale.localScale = new Vector3(x, 1, 1);
        Debug.Log(Mathf.Max(Screen.width, Screen.height) + " adjusted to : " + x);
    }
    /// <summary>
    /// Inverses lerp tToRelativize beetween v1 and v2, and lerp with this obtained value beetween a and b
    /// Given, two points and their corresponding values, we can calculate a new value on the line drawn beetween the first two couples
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="tToRelativize"></param>
    /// <returns></returns>
    private float LerpRelative(float v1, float v2, float a, float b, float tToRelativize)
    {
        tToRelativize = (tToRelativize - v1) / (v2 - v1);
        //If is nan, meands v1=v2, we take the halfway point, but it especially means we provided same values for v1 and v2 which doesn't mean a thing
        if (float.IsNaN(tToRelativize))
            tToRelativize = .5f;
        return tToRelativize * b + (1 - tToRelativize) * a;

    }
    void AdjustDepth()
    {
        //Reference values, found iteratively, to always scale the background correctly, change here if the background size tend to change
        AdjustDepth(new Vector2(1920, 1080), Vector3.one, new Vector2(2400, 1080), new Vector3(1.17f, 1, 1));
        //1.17f,1,1
    }
}