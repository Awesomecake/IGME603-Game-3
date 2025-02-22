using UnityEngine;

public class Grow : MonoBehaviour
{
    public void ChangeSize(bool isBig)
    {
        transform.localScale *= isBig ? 2f : 0.5f;
    }
}