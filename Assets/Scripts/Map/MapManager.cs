using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public int mMinWidth = 3;
    public int mMinHeight = 3;
    public int mMaxWidth = 20;
    public int mMaxHeight = 20;

    public GameObject mBaseTilePrefab;
    public int mCurrentWidth = 8;
    public int mCurrentHeight = 11;

    public TILESTYLE mEditTileStyle = TILESTYLE.NORMAL;
    
//    public GameObject[,] mTiles;
    public List<GameObject> mTiles;

    public List<GameObject> mStoredTile;

    public List<Transform> mPathList = new List<Transform>();
    
    public string mFileName = "MapData";

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

    static MapManager mInstance = null;

    public static MapManager instance
    {
        get
        {
            return mInstance;
        }
    }
    
    void Awake()
    {
        mInstance = this;
        LoadMapDataFromFile();
    }

    public Transform[] GetPathArry()
    {
        return mPathList.ToArray();
    }

    public void LoadMapDataFromFile()
    {
        RemoveAllTiles();
        string filePath = "MapData/" + mFileName;
        TextAsset asset = (TextAsset)Resources.Load(filePath, typeof(TextAsset));

        if (asset == null)
        {
            Debug.Log("Can not open File on Asset/" + filePath + ".txt");
            return;
        }
        
        TextReader textReader = new StringReader(asset.text);
        
        string text = textReader.ReadLine();
        string widthText = text.Substring(text.IndexOf(' ') + 1);
        mCurrentWidth = int.Parse(widthText);

        text = textReader.ReadLine();
        string heightText = text.Substring(text.IndexOf(' ') + 1);
        mCurrentHeight = int.Parse(heightText);

        //mTiles = new GameObject[mCurrentWidth, mCurrentHeight];
        mTiles = new List<GameObject>(mCurrentWidth * mCurrentHeight + 1);

        for (int row = 0; row < mCurrentWidth; ++row)
        {
            for (int col = 0; col < mCurrentHeight; ++col)
            {
                text = textReader.ReadLine();
                string[] infos = text.Split('\t');

                GameObject obj = Instantiate(mBaseTilePrefab) as GameObject;
                obj.name = row + "_" + col;
                obj.transform.parent = transform;

                obj.transform.localPosition = GetVector3FromString(infos[0]);
                obj.transform.eulerAngles = GetVector3FromString(infos[1]);

                TileInfomation tileInfomation = obj.GetComponent<TileInfomation>();
                tileInfomation.currentTileStyle = (TILESTYLE)(int.Parse(infos[2]));
                tileInfomation.UpdateMaterial();

                //mTiles[i, j] = obj;
                mStoredTile.Add(obj);
                mTiles.Add(obj);
            }
        }
        
        text = textReader.ReadLine();
        string pathCountString = text.Substring(text.IndexOf(' ') + 1);
        int pathCount = int.Parse(pathCountString);

        for (int i = 0; i < pathCount; ++i)
        {
            text = textReader.ReadLine();
            string[] tiles = text.Split('\t');
            int x = int.Parse(tiles[0]);
            int y = int.Parse(tiles[1]);

            mPathList.Add(mTiles[x * mCurrentHeight + y].transform);
        }

    }//End of LoadMap DataFromFile

    public void RemoveAllTiles()
    {
        List<GameObject> gameObjectList = new List<GameObject>();

        foreach (Transform child in transform)
        {
            gameObjectList.Add(child.gameObject);
        }

        for (int i = 0; i < gameObjectList.Count; ++i)
        {
            if (Application.isPlaying == true)
            {
                Destroy(gameObjectList[i]);
            }
            else
            {
                DestroyImmediate(gameObjectList[i]);
            }
        }

        mPathList.Clear();
        gameObjectList.Clear();
        gameObjectList = null;
        mTiles.Clear();
        //mTiles = null;

    }
}
