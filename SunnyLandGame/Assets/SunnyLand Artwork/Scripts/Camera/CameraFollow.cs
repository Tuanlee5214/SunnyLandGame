using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;      // Đối tượng nhân vật mà camera sẽ theo dõi
    public Vector3 offset;        // Khoảng cách giữa camera và nhân vật
    public float smoothSpeed = 0.125f; // Tốc độ di chuyển mượt mà của camera

    void LateUpdate()
    {
        // Xác định vị trí mong muốn của camera dựa vào vị trí nhân vật và offset
        Vector3 desiredPosition = player.position + offset;
        // Sử dụng Lerp để camera di chuyển mượt mà
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Nếu muốn camera luôn nhìn về phía nhân vật
        transform.LookAt(player);
    }
}
