using UnityEngine;

public class BackgroundAligner : MonoBehaviour
{
    private void Awake()
    {
        AdjustDepth();
    }
    void AdjustDepth(Vector2 _originalScreenSize, Vector3 _originalDepth, Vector2 _adjustedScreenSize, Vector3 _adjustedDepth)
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;

        float originalAspectRatio = _originalScreenSize.x / _originalScreenSize.y;
        float adjustedAspectRatio = _adjustedScreenSize.x / _adjustedScreenSize.y;
        float aspectRatioDifference = (currentAspectRatio - originalAspectRatio) / (adjustedAspectRatio - originalAspectRatio);
        var adjustedDepth = aspectRatioDifference * _adjustedDepth + (1 - aspectRatioDifference) * _originalDepth;
        transform.localScale = adjustedDepth;
    }
    void AdjustDepth()
    {
        //Reference values, found iteratively, to always scale the background correctly, change here if the background size tend to change
        AdjustDepth(new Vector2(1920, 1080), Vector3.one, new Vector2(2400, 1080), new Vector3(1.17f,1,1));
    }
}