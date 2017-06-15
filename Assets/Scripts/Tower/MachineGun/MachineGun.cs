using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour, ITower {
    public GameObject mCurrentTarget = null;
	LockTarget mLockTarget = null;

    float mDamage = 10;
	float mDeltaTime = 0.0f;
	public float fireStateUpdateTime = 0.6f;
	public float fireActiveTime = 0.5f;
    public bool mIsGrounded = true;
    

    GunAnimation[] mGunAnimationsFlashs = null;

	// Use this for initialization
	void Start () 
	{
        mLockTarget = GetComponentInChildren<LockTarget>();

		mGunAnimationsFlashs = GetComponentsInChildren<GunAnimation>();

		foreach (GunAnimation muzzleFlash in mGunAnimationsFlashs) 
		{
			muzzleFlash.InitMuzzleFlash();
		}
	}
	
	// Update is called once per frame
	void Update () {
        if(!mIsGrounded)
        {
            return;
        }

        if (mCurrentTarget == null) 
		{
            GetComponent<Collider>().enabled = false;
            GetComponent<Collider>().enabled = true;

            return;
		}

		mLockTarget.RotateToTarget (mCurrentTarget.transform);

		mDeltaTime += Time.deltaTime;
		if (mDeltaTime > fireStateUpdateTime) 
		{
			mDeltaTime += Time.deltaTime;

			if(mDeltaTime > fireStateUpdateTime)
			{
				mDeltaTime = -fireActiveTime;
				FireProcess();
			}
		}
	}

	void FireProcess()
	{
		if (mCurrentTarget == null) 
		{
            return ;		
		}

		foreach (GunAnimation muzzleFlash in mGunAnimationsFlashs) 
		{
			muzzleFlash.ActiveMuzzleFlash(fireActiveTime);
		}

        EnemyState enemyState = mCurrentTarget.GetComponent<EnemyState>();
        if (enemyState != null)
        {
            enemyState.Damage(transform, fireActiveTime, mDamage, DamageType.MACHINE_GUN);
            SoundManager.Instance.PlaySound("machine_gun_shot", true);
        }
    }

	void OnTriggerEnter( Collider other)
	{
        if(other.gameObject.tag != "Enemy")
        {
            return;
        }

		if (mCurrentTarget == null) 
		{
			mCurrentTarget = other.gameObject;	
		}
	}

	void OnTriggerExit( Collider other )
	{
		if ( mCurrentTarget == other.gameObject ) 
		{
			mCurrentTarget = null;
		}
	}

    public TowerType GetTowerType()
    {
        return TowerType.MACHINE_GUN;
    }

    public bool IsGrounded()
    {
        return mIsGrounded;
    }

    public void SetGrounded(bool value)
    {
        mIsGrounded = value;
    }
}
