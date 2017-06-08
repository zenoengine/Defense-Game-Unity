using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour {

    public EnemyState mEnemyState;
    public Slider mSlider;

    void Start ()
    {
        mEnemyState = GetComponentInParent<EnemyState>();
        mSlider = GetComponentInChildren<Slider>();
    }
	
	void Update ()
    {
        if(mSlider == null || mEnemyState == null)
        {
            Debug.Log("EnemyHP Component need enemy state and slider");
            return;
        }

        mSlider.value = mEnemyState.mHP / mEnemyState.mMaxHP;	
	}
}
