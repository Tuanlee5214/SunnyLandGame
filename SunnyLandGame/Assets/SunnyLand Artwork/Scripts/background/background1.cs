using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background1 : MonoBehaviour
{
    public Transform miniCam;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - miniCam.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset + miniCam.position;
    }
}
