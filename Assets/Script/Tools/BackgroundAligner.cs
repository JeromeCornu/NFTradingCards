using UnityEngine;

public class BackgroundAligner : MonoBehaviour
{
    private void Awake()
    {
        AdjustDepth();
    }
    void AdjustDepth(Vector2 _originalScreenSize, float _originalDepth, Vector2 _adjustedScreenSize, float _adjustedDepth)
    {
        float currentAspectRatio = (float)Screen.width / Screen.height;

        float originalAspectRatio = _originalScreenSize.x / _originalScreenSize.y;
        float adjustedAspectRatio = _adjustedScreenSize.x / _adjustedScreenSize.y;
        float aspectRatioDifference = (currentAspectRatio - originalAspectRatio) / (adjustedAspectRatio - originalAspectRatio);
        float adjustedDepth = aspectRatioDifference * _adjustedDepth + (1 - aspectRatioDifference) * _originalDepth;
        transform.position = new Vector3(transform.position.x, transform.position.y, adjustedDepth);
    }
    void AdjustDepth()
    {
        //Reference values, found iteratively, to always scale the background correctly, change here if the background size tend to change
        AdjustDepth(new Vector2(1920, 1080), 0f, new Vector2(2400, 1080), -2f);
    }
}