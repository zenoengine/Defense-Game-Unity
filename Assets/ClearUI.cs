using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearUI : MonoBehaviour {

    public GameObject [] mStarts;
    public Text mScoreValue;

    public GameRoot mGameRoot;

    void Start () {
        int loseLife = mGameRoot.mMaxLife - mGameRoot.mRemainedLife;

        //HACK : you should make good movement count,
        int loseMovement = mGameRoot.mMaxMovement/2 - mGameRoot.mRemainedMovement;

        int starCount = 1;
        if (loseLife < 3)
        {
            starCount++;
        }

        if(loseMovement < 3)
        {
            starCount++;
        }

        foreach(var startObj in mStarts)
        {
            startObj.SetActive(false);
        }

        
        for(int i= 0; i< starCount; i++)
        {
            if(i == mStarts.Length)
            {
                break;
            }
            mStarts[i].SetActive(true);
        }

        int scoreValue = 10000 - loseLife * 500 - loseMovement * 100;
        mScoreValue.text = scoreValue.ToString();
    }
	
	void Update () {
		
	}
}
