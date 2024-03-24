using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private GameObject cameraAnchor;
    private Vector3 cameraAngles;
    private Vector3 cameraOffset; 
    private Vector3 initialAngles; 
    private Vector3 initialOffset;

    void Start()
    {
        initialAngles = cameraAngles = transform.eulerAngles;
        initialOffset = cameraOffset = transform.position - cameraAnchor.transform.position;
        
    }

    void Update()
    {
        cameraAngles.y += Input.GetAxis("Mouse X");
        cameraAngles.x -= Input.GetAxis("Mouse Y");
        if (Input.GetKeyUp(KeyCode.V))
        {
            cameraOffset = (cameraOffset == Vector3.zero) ? initialOffset : Vector3.zero;
        }
    }

    private void LateUpdate()
    {
        transform.position = cameraAnchor.transform.position + Quaternion.Euler(0, cameraAngles.y - initialAngles.y, 0) * cameraOffset;
        transform.eulerAngles = cameraAngles;
    }
}
