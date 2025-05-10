using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform mainCam;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - mainCam.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = offset + mainCam.position;
    }
}
