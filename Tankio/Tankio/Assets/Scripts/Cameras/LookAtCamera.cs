using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cameraTransform;

    void Awake()
    {
        InitializationAwake();
    }

    void LateUpdate()
    {
        UpdateDirection();
    }

    void UpdateDirection()
    {
        transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward, cameraTransform.rotation * Vector3.up);
    }

    void InitializationAwake()
    {
        cameraTransform = Camera.main.transform;
    }
}