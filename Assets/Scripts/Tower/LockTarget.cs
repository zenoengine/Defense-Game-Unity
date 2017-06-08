using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTarget : MonoBehaviour {
    public float rotationSpeed = 3.0f;
    
    public void RotateToTarget(Transform target)
    {
        if (target == null)
        {
            return;
        }

        Vector3 dir = target.position - transform.position;
        dir.y = 0.0f;
        dir.Normalize();

        Quaternion from = transform.rotation;
        Quaternion to = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(from, to, rotationSpeed * Time.deltaTime);
    }
}
