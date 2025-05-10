using UnityEngine;
using System.Collections;

public class MiniCamFollow : MonoBehaviour
{
    public Transform player; // Nhân vật chính
    public float smoothSpeed = 0.125f;
    private bool isManualControl = false; // Kiểm tra người chơi có đang thao tác không
    private float resetTime = 2f; // Thời gian reset về nhân vật
    private Vector3 offset;
    public Vector3 minValue, maxValue;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }
    void LateUpdate()   
    {
        if (!isManualControl && player != null)
        {
            Vector3 targetPosition = transform.position;
            targetPosition.x = player.position.x + offset.x;
            targetPosition.y = player.position.y + offset.y;

            targetPosition.x = Mathf.Clamp(targetPosition.x, minValue.x, maxValue.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minValue.y, maxValue.y);

            //Dời vị trí của camera theo vị trí của x thôi, giữ nguyên vị trí của y và z
            Vector3 smoothPosition = new Vector3(Mathf.Lerp(transform.position.x, targetPosition.x, smoothSpeed),
                                                 Mathf.Lerp(transform.position.y, targetPosition.y, smoothSpeed),
                                                 transform.position.z);

            //sau mỗi khung hình gán vị trí mới của camera sau khi di chuyển, nhớ là từng chút một
            transform.position = smoothPosition;
        }
    }

    public void StartManualControl()
    {
        isManualControl = true;
        StopAllCoroutines();
        StartCoroutine(ResetFollow());
    }

    IEnumerator ResetFollow()
    {
        yield return new WaitForSeconds(resetTime);
        isManualControl = false;
    }
}
