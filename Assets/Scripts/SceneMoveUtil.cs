using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveUtil : MonoBehaviour {

    public void GoToTitle()
    {
        SoundManager.Instance.PlaySound("t_se_click", true);
        SceneManager.LoadScene("title");
    }

    public void GoToStageSelection()
    {
        SoundManager.Instance.PlaySound("t_se_click", true);
        SceneManager.LoadScene("stage_selection");
    }

    public void GoToStage(string stageName)
    {
        SoundManager.Instance.PlaySound("t_se_click", true);
        PlayerPrefs.SetString("STAGE_NAME", stageName);
        SceneManager.LoadScene("main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
