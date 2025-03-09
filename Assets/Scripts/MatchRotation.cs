using UnityEngine;

public class MatchRotation : MonoBehaviour
{
    [SerializeField] private Transform following;
    private void LateUpdate()
    {
        transform.rotation = following.rotation;
    }
}