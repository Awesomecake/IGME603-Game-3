using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header ("Camera Settings")]
    public float mouseInfluenceStrength = 2f; // How much the mouse affects the camera offset
    public float maxMouseOffset = 3f; // Maximum distance the camera can move from the player


    [Header ("Components")]
    public CinemachineVirtualCamera virtualCamera;
    public Transform player;

    private Vector3 _defaultOffset;
    private CinemachineFramingTransposer framingTransposer;

    public static CameraManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _defaultOffset = framingTransposer.m_TrackedObjectOffset;
    }

    private void Update()
    {
        // Get the mouse position in screen space
        Vector3 mousePosition = Input.mousePosition;
        Vector3 viewportMousePosition = Camera.main.ScreenToViewportPoint(mousePosition);

        // Calculate the mouse offset
        Vector3 mouseOffset = new Vector3((viewportMousePosition.x - 0.5f) * 2f,(viewportMousePosition.y - 0.5f) * 2f,0);

        // Clamp and apply the offset
        mouseOffset = Vector3.ClampMagnitude(mouseOffset, maxMouseOffset);
        framingTransposer.m_TrackedObjectOffset = _defaultOffset + mouseOffset * mouseInfluenceStrength;

    }

    public void TrackPlayer(Transform newPlayer)
    {
        player = newPlayer;
        virtualCamera.Follow = player;
    }

    public void ChangeTarget(Transform target)
    {
        virtualCamera.Follow = target;
    }
}
