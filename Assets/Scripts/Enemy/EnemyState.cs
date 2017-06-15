using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour {
    public ParticleSystem mMachineGunDamageEffect = null;
    public ParticleSystem mCannonDamageEffect = null;

    public float mDamageEffectHeight = 1.0f;
    public float mDamageEffectRadiusOffset = 0.11f;
    public float mHP = 100;
    public float mMaxHP = 100;

    Dictionary<DamageType, ParticleSystem> mDamageEffectMap = new Dictionary<DamageType, ParticleSystem>();

    void Start()
    {
        mDamageEffectMap.Add(DamageType.CANNON, mCannonDamageEffect);
        mDamageEffectMap.Add(DamageType.MACHINE_GUN, mMachineGunDamageEffect);
    }

    public void Damage(Transform towerTransform, float fireActiveTime, float damage, DamageType damageType)
    {
        StartCoroutine(DamageEffectProcess(towerTransform, fireActiveTime, damage, damageType));
    }

    IEnumerator DamageEffectProcess(Transform towerTransform, float fireActiveTime, float damage, DamageType damageType)
    {
        ParticleSystem damageEffect = null;
        mDamageEffectMap.TryGetValue(damageType, out damageEffect);

        if(damageEffect)
        {
            damageEffect.Play();
        }

        float deltaTime = 0.0f;
        while ((deltaTime < fireActiveTime) && (towerTransform) != null)
        {
            deltaTime += Time.deltaTime;
            Vector3 dir = towerTransform.position - transform.position;
            dir.y = 0.0f;
            dir.Normalize();
            if (damageEffect)
            {
                damageEffect.transform.position = transform.position + (Vector3.up * mDamageEffectHeight) + (mDamageEffectRadiusOffset * dir);
            }
            yield return new WaitForEndOfFrame();
        }

        if(damageEffect)
        {
            damageEffect.Stop();
        }

        mHP = mHP - damage;
        if(mHP < 0)
        {
            Destroy(gameObject);
        }

        yield return null;
    }

    void OnDestroy()
    {
        GameObject waveManagerObj = GameObject.Find("WaveManager");
        WaveManager waveManager = waveManagerObj.GetComponent<WaveManager>();
        waveManager.RemoveEnemy(gameObject);
    }
}
