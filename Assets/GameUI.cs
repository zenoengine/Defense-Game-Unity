using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    public GameObject mConfirmUI;
    public GameObject mInGameUI;
    public GameObject mPauseUI;
    public GameObject mClearUI;
    public GameObject mGameOverUI;

    public GameRoot mGameRoot;
    public WaveManager mWaveManager;
    public Text mMovementText;
    public Text mLifeText;
    public Text mWaveInfoText;
    
	// Update is called once per frame
	void Update () {
        mMovementText.text =  mGameRoot.mRemainedMovement.ToString();
        mLifeText.text = mGameRoot.mRemainedLife.ToString();
        mWaveInfoText.text = "Wave " + mWaveManager.mCurrentWaveIndex.ToString();
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
        mPauseUI.SetActive(true);
        mInGameUI.SetActive(false);
    }

    public void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public void OnClickResume()
    {
        Time.timeScale = 1;
        mPauseUI.SetActive(false);
        mInGameUI.SetActive(true);
    }

    public void OnClickToggleMuteBtn()
    {

    }

    public void OnClickBackBtn()
    {
        mConfirmUI.SetActive(true);
    }

    public void OnClickConfirmUIClosedBtn()
    {
        mConfirmUI.SetActive(false);
    }

    public void OnClear()
    {
        mClearUI.SetActive(true);
    }

    public void OnGameOver()
    {
        mGameOverUI.SetActive(true);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene("main");
    }
}
