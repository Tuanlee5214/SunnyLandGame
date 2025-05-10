using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;//Vị trí của nhân vật
    private Vector3 offset;//Vị trí của cam so với nhân vật 
    public float smoothSpeed;//Biến đo độ mượt

    public Vector3 minValue, maxValue;//Gía trị min max tọa độ x y của camera mà mình muốn cam nó ở vị trí đó
    void Start()
    {
        //Xác định bằng cách lấy vị trí hiện tại trừ đi vị trí nhân vật ra vị trí của 
        //cam so với nhân vật
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (target != null)
        {
            //Xác định vị trí mà camera sẽ di chuyển tới
            Vector3 targetPosition = transform.position;
            targetPosition.x = target.position.x + offset.x;
            targetPosition.y = target.position.y + offset.y;

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
}
