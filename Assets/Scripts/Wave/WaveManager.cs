using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Wave
{
    public GameObject enemyPrefab = null;
    public int spawnEnemyCount = 3;
    public float spawnEnemyTime = 1.0f;
}

public class WaveManager : MonoBehaviour {

    private GameObject mGameUI;

    public float mWaveDelayTime = 3.0f;

    public List<Wave> mWaveList = new List<Wave>();

    public int mCurrentWaveIndex = 0;
    bool mWaveProgress = false;
    
    void Update()
    {
        if (mWaveProgress == false)
        {
            if (mCurrentWaveIndex < mWaveList.Count)
            {
                Wave wave = mWaveList[mCurrentWaveIndex];
                StartCoroutine(WaveProcess(wave));
                ++mCurrentWaveIndex;
            }
        }
    }

    IEnumerator WaveProcess(Wave wave)
    {
        mWaveProgress = true;
        for (int i = 0; i < wave.spawnEnemyCount; ++i)
        {
            GameObject enemy = Instantiate(wave.enemyPrefab) as GameObject;

            //HACK : For catach when enemy instantiate in tower trigger
            enemy.GetComponent<Collider>().enabled = false;
            enemy.GetComponent<Collider>().enabled = true;
            yield return new WaitForSeconds((wave.spawnEnemyTime));
        }
        yield return new WaitForSeconds(mWaveDelayTime);
        mWaveProgress = false;
    }
}
