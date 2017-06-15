using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameRoot : MonoBehaviour {
    string mFileName;

    public int mRemainedMovement = 0;
    public int mMaxMovement = 0;
    public int mRemainedLife = 0;
    public int mMaxLife = 0;
    public int mTowerCount = 0;

    public WaveManager mWaveManager;
    public GameObject GameUIObject;

    public GameObject [] towerPrefabs;

	void Start () {
        mFileName = PlayerPrefs.GetString("STAGE_NAME", "DefaultLevel");
        LoadLevelDataFromFile();
    }

    void Update()
    {
        if(mWaveManager.IsWaveFinish() && mWaveManager.IsZeroEnemy())
        {
            GameUIObject.SendMessage("OnClear");
        }

        if(IsGameOver())
        {
            GameUIObject.SendMessage("OnGameOver");
        }
    }

    bool IsGameOver()
    {
        return mRemainedLife == 0;
    }

    public void LoadLevelDataFromFile()
    {
        string filePath = "LevelData/" + mFileName;
        TextAsset asset = (TextAsset)Resources.Load(filePath, typeof(TextAsset));

        if (asset == null)
        {
            Debug.Log("Can not open File on Asset/" + filePath + ".txt");
            return;
        }

        TextReader textReader = new StringReader(asset.text);

        string text = textReader.ReadLine();
        string movementText = text.Substring(text.IndexOf(' ') + 1);
        mMaxMovement = int.Parse(movementText);
        mRemainedMovement = mMaxMovement;

        text = textReader.ReadLine();
        string lifeText = text.Substring(text.IndexOf(' ') + 1);
        mMaxLife = int.Parse(lifeText);
        mRemainedLife = mMaxLife;

        text = textReader.ReadLine();
        string towerCountText = text.Substring(text.IndexOf(' ') + 1);
        mTowerCount = int.Parse(towerCountText);

        for(int idx = 0; idx < mTowerCount; idx++)
        {
            text = textReader.ReadLine();
            string[] infos = text.Split('\t');

            Vector3 position = GetVector3FromString(infos[0]);
            int towerType = int.Parse(infos[1]);

            if(towerPrefabs.Length > towerType)
            {
                Instantiate(towerPrefabs[towerType], position, Quaternion.identity);
            }
        }
    }

    public Vector3 GetVector3FromString(string text)
    {
        string newText = text.Replace('(', ' ');
        newText = newText.Replace(')', ' ');

        string[] elements = newText.Split(',');
        float x = float.Parse(elements[0]);
        float y = float.Parse(elements[1]);
        float z = float.Parse(elements[2]);

        return new Vector3(x, y, z);
    }

    public void ReduceRemainedMovement()
    {
        mRemainedMovement--;
    }

    public void ReduceRemainedLife()
    {
        mRemainedLife--;
    }

}
