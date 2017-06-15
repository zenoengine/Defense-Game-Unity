using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour {
    public ParticleSystem mDamageEffect = null;
    public float mDamageEffectHeight = 1.0f;
    public float mDamageEffectRadiusOffset = 0.11f;
    public float mHP = 100;
    public float mMaxHP = 100;

    public void Damage(Transform towerTransform, float fireActiveTime, float damage)
    {
        if (mDamageEffect == null)
        {
            return;
        }

        StartCoroutine(DamageEffectProcess(towerTransform, fireActiveTime, damage));
    }

    IEnumerator DamageEffectProcess(Transform towerTransform, float fireActiveTime, float damage)
    {
        mDamageEffect.Play();
        float deltaTime = 0.0f;
        while ((deltaTime < fireActiveTime) && (towerTransform) != null)
        {
            deltaTime += Time.deltaTime;
            Vector3 dir = towerTransform.position - transform.position;
            dir.y = 0.0f;
            dir.Normalize();
            mDamageEffect.transform.position = transform.position + (Vector3.up * mDamageEffectHeight) + (mDamageEffectRadiusOffset * dir);
            yield return new WaitForEndOfFrame();
        }

        mDamageEffect.Stop();
        mHP = mHP - damage;
        if(mHP < 0)
        {
            Destroy(gameObject);
        }

        yield return null;
    }
}
