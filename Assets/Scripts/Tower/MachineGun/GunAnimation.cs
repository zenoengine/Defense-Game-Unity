using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimation : MonoBehaviour {
    public float mMinScale = 0.5f;
    public float mMaxScale = 1.5f;
    
    public void InitMuzzleFlash()
    {
        gameObject.SetActive(false);
    }

    public void ActiveMuzzleFlash(float activeTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(ActiveGunFlashProcess(activeTime));
    }

    IEnumerator ActiveGunFlashProcess(float activeTime)
    {
        float deltaTime = 0.0f;

        while (deltaTime < activeTime)
        {
            deltaTime += Time.deltaTime;
            UpdateScaleAndRotation();
            yield return new WaitForEndOfFrame();
        }

        InitMuzzleFlash();
    }

    void UpdateScaleAndRotation()
    {
        float sacleValue = Random.Range(mMinScale, mMaxScale);
        transform.localScale = Vector3.one * sacleValue;

        Vector3 localEulerAngle = transform.localEulerAngles;
        localEulerAngle.z = Random.Range(0.1f, 90.0f);
        transform.localEulerAngles = localEulerAngle;
    }
}
