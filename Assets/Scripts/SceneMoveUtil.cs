using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveUtil : MonoBehaviour {

    public void GoToTitle()
    {
        SceneManager.LoadScene("title");

    }

    public void GoToStageSelection()
    {
        SceneManager.LoadScene("stage_selection");
    }

    public void GoToStage(string stageName)
    {
        PlayerPrefs.SetString("STAGE_NAME", stageName);
        SceneManager.LoadScene("main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
