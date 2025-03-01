using UnityEngine;

public class LilBro : MonoBehaviour
{
    public enum State
    {
        Normal,
        Downed,
    }

    public State currentState = State.Normal;
}