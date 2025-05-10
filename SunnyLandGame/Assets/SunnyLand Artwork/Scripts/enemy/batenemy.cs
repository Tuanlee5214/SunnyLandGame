using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class batenemy : MonoBehaviour
{
    public GameObject fx;
    private float tocdo = 3.2f;
    private float distance = 4f;
    private Vector3 startPos;
    private bool movingRight = true;
    void Start()
    {
        startPos = transform.position;
        
    }

    void Update()
    {
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;
        if(movingRight)
        {
            transform.Translate(Vector2.right * tocdo * Time.deltaTime);
            if (transform.position.x > rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * tocdo * Time.deltaTime);
            if(transform.position.x < leftBound)
            {
                movingRight = true;
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
