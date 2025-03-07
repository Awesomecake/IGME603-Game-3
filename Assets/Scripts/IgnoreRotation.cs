using UnityEngine;

public class IgnoreRotation : MonoBehaviour
{
    [SerializeField] private bool flipOnFacingLeft;

    private void LateUpdate()
    {
        var needsFlip = flipOnFacingLeft && Mathf.Abs(Mathf.DeltaAngle(transform.parent.rotation.eulerAngles.z, 180f)) < 90f;

        transform.rotation = Quaternion.identity;
        
        var flippedX = Mathf.Abs(transform.localScale.x) * (needsFlip ? -1f : 1f);
        transform.localScale = transform.localScale.Copy(x: flippedX);
    }
}