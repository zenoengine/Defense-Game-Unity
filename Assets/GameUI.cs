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
        SoundManager.Instance.PlaySound("t_se_click", true);
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
        SoundManager.Instance.PlaySound("t_se_click", false);
    }

    public void OnClickToggleMuteBtn()
    {

    }

    public void OnClickBackBtn()
    {
        mConfirmUI.SetActive(true);
        SoundManager.Instance.PlaySound("t_se_click", true);

    }

    public void OnClickConfirmUIClosedBtn()
    {
        mConfirmUI.SetActive(false);
        SoundManager.Instance.PlaySound("t_se_click", false);
    }

    public void OnClear()
    {
        if(!mClearUI.activeSelf)
        {
            mClearUI.SetActive(true);
            SoundManager.Instance.PlaySound("t_se_clear", false);
            Time.timeScale = 0;
        }
    }

    public void OnGameOver()
    {
        if(!mGameOverUI.activeSelf)
        {
            mGameOverUI.SetActive(true);
            Time.timeScale = 0;
            SoundManager.Instance.PlaySound("r_se_star_explode", false);
        }
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene("main");
        SoundManager.Instance.PlaySound("t_se_click", false);
    }
}
