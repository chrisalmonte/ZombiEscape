using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceOrientation : MonoBehaviour
{
    private Transform _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main.transform; 
    }
    private void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.rotation * Vector3.forward, _mainCamera.rotation * Vector3.up);
    }
}
