using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    Transform mTransform;
    Transform mMainCameraTransform;

    void Start()
    {
        mTransform = transform;
        mMainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        transform.LookAt(mTransform.position + mMainCameraTransform.rotation * Vector3.forward,
            mMainCameraTransform.rotation * Vector3.up);
    }

}
