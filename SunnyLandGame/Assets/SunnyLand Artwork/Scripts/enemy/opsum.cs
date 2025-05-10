using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opsum : MonoBehaviour
{
    public GameObject fx;
    private float tocdo = 2.8f;
    private float distance = 4f;
    private Vector3 startPos;
    private bool movingLeft = true;
    void Start()
    {
        startPos = transform.position;

    }

    void Update()
    {
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;
        if (movingLeft)
        {
            transform.Translate(Vector2.left * tocdo * Time.deltaTime);
            if (transform.position.x < leftBound)
            {
                movingLeft = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.right * tocdo * Time.deltaTime);
            if (transform.position.x > rightBound)
            {
                movingLeft = true;
                Flip();
            }
        }

    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
