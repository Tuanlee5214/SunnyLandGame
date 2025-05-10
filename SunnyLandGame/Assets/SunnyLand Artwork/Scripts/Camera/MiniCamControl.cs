using UnityEngine;
using UnityEngine.EventSystems;

public class MiniCamControl : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Camera miniCam;
    public Vector2 minLimits, maxLimits; // Giới hạn di chuyển
    private bool isDragging = false;
    private Vector3 lastMousePosition;


    private MiniCamFollow miniCamFollow;

    void Start()
    {
        miniCamFollow = miniCam.GetComponent<MiniCamFollow>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastMousePosition = eventData.position;
        // Dừng theo dõi nhân vật khi bắt đầu kéo Mini Cam
        if (miniCamFollow != null)
        {
            miniCamFollow.StartManualControl();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector3 delta = miniCam.ScreenToWorldPoint(lastMousePosition) - miniCam.ScreenToWorldPoint(eventData.position);
        Vector3 newPosition = miniCam.transform.position + delta;

        // Giới hạn di chuyển
        newPosition.x = Mathf.Clamp(newPosition.x, minLimits.x, maxLimits.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minLimits.y, maxLimits.y);

        miniCam.transform.position = newPosition;
        lastMousePosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
