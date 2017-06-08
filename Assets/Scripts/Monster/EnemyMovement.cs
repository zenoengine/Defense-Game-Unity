using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public Transform[] mPath = null;
    public float mMoveSpeed = 1.0f;
    public float mRotationSpeed = 10.0f;
    public Animation mAnimation = null;
    public float mWalkAnimationSpeed = 1.0f;

    int mCurrentPathIndex = 0;
    Transform mNexDestination = null;

    void Start()
    {
        mPath = MapManager.instance.GetPathArry();

        FindNextDesination();

        if (mAnimation)
        {
            mAnimation["Walk"].speed = mWalkAnimationSpeed;
            mAnimation.Play("Walk");
        }
    }

    void FindNextDesination()
    {
        if (mCurrentPathIndex < mPath.Length)
        {
            mNexDestination = mPath[mCurrentPathIndex];

            if (mCurrentPathIndex == 0)
            {
                transform.position = mNexDestination.position;
            }

            ++mCurrentPathIndex;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        if (mNexDestination == null)
        {
            return;
        }

        Vector3 targetPos = mNexDestination.position;
        Vector3 myPos = transform.position;
        myPos.y = targetPos.y = 0.0f;

        float distance = (targetPos - myPos).magnitude;

        if (distance < 0.1f)
        {
            FindNextDesination();
        }
        else
        {
            Vector3 dir = mNexDestination.position - transform.position;
            dir.y = 0.0f;
            dir.Normalize();
            transform.position += (dir * mMoveSpeed) * Time.deltaTime;

            Quaternion from = transform.rotation;
            Quaternion to = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(from, to, mRotationSpeed * Time.deltaTime);
        }
    }
}
