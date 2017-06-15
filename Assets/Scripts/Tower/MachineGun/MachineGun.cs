﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour {
    public GameObject mCurrentTarget = null;
	LockTarget mLockTarget = null;

	float deltaTime = 0.0f;
	public float fireStateUpdateTime = 0.6f;
	public float fireActiveTime = 0.5f;
    public bool IsGrounded = true;
    

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
        if(!IsGrounded)
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

		deltaTime += Time.deltaTime;
		if (deltaTime > fireStateUpdateTime) 
		{
			deltaTime += Time.deltaTime;

			if(deltaTime > fireStateUpdateTime)
			{
				deltaTime = -fireActiveTime;
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
            enemyState.Damage(transform, fireActiveTime);
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
}
