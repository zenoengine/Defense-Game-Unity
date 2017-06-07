using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(MapManager))]
public class MapManagerInspector : Editor
{
    static Dictionary<char, TILESTYLE> mShortCutTileMap = new Dictionary<char, TILESTYLE>();

    public override void OnInspectorGUI()
    {
        MapManager mapManager = target as MapManager;

        DrawMinMaxValues(mapManager);
        DrawBaseTileObject(mapManager);
        DrawCurrentMinMax(mapManager);
        DrawGenerateButton(mapManager);

        CommonEditorUI.DrawSeparator(Color.cyan);
        DrawSaveLoadButton(mapManager);

        CommonEditorUI.DrawSeparator(Color.cyan);
        DrawPathData(mapManager);

        CommonEditorUI.DrawSeparator(Color.cyan);
        ShowTiles(mapManager);
    }

    void Init()
    {
        if(mShortCutTileMap.Count == 0)
        {
            mShortCutTileMap.Add('3', TILESTYLE.NORMAL);
            mShortCutTileMap.Add('4', TILESTYLE.STRAIGHT);
            mShortCutTileMap.Add('5', TILESTYLE.CORNER);
            mShortCutTileMap.Add('6', TILESTYLE.START);
        }
    }

    void DrawSaveLoadButton(MapManager mapManager)
    {
        string fileName = EditorGUILayout.TextField("Map Data File Name", mapManager.mFileName);
        if (fileName != mapManager.mFileName)
        {
            CommonEditorUI.RegisterUndo("Map File Name", mapManager);
            mapManager.mFileName = fileName;
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save Map Data"))
        {
            string title = "Save Map File";
            string msg = "Do tou want to save map data?";
            if (EditorUtility.DisplayDialog(title, msg, "yes", "no"))
            {
                string dataPath = Application.dataPath;
                string fullPath = dataPath + "/Resources/MapData/" + fileName + ".txt";
                Debug.Log(fullPath);
                FileStream fs = new FileStream(fullPath, FileMode.Create);
                TextWriter textWriter = new StreamWriter(fs);
                int width = mapManager.mCurrentWidth;
                int height = mapManager.mCurrentHeight;
                textWriter.WriteLine("width " + width);
                textWriter.WriteLine("height " + height);

                for (int row = 0; row < width; ++row)
                {
                    for (int col = 0; col < height; ++col)
                    {
                        Transform tile = mapManager.mTiles[row*mapManager.mCurrentWidth + col].transform;
                        textWriter.Write(tile.position + "\t");
                        textWriter.Write(tile.eulerAngles + "\t");

                        TileInfomation tileInfomation = tile.GetComponent<TileInfomation>();
                        textWriter.Write((int)tileInfomation.currentTileStyle + "\t");
                        textWriter.Write("\n");
                    }
                }

                int pathCount = mapManager.mPathList.Count;
                textWriter.WriteLine("PathCount " + pathCount);
                for (int i = 0; i < pathCount; ++i)
                {
                    string[] tileIndex = mapManager.mPathList[i].name.Split('_');
                    textWriter.Write(tileIndex[0] + "\t");
                    textWriter.Write(tileIndex[1]);
                    textWriter.Write("\n");
                }
                textWriter.Close();
            }
        }
        if (GUILayout.Button("Load Map Data"))
        {
            mapManager.LoadMapDataFromFile();
        }
        EditorGUILayout.EndHorizontal();
    }

    void DrawPathData(MapManager mapManager)
    {
        if (GUILayout.Button("Remove All Path Data"))
        {
            mapManager.mPathList.Clear();
        }

        string pathLabel = "Path List :" + mapManager.mPathList.Count;
        EditorGUILayout.LabelField(pathLabel);
        for (int i = 0; i < mapManager.mPathList.Count; ++i)
        {
            string pathName = "Path Index :" + i;
            Transform pathTransform = mapManager.mPathList[i];
            EditorGUILayout.ObjectField(pathName, pathTransform, typeof(GameObject), false);
        }
    }

    public void DrawPathList(MapManager mapManager)
    {
        for (int i = 0; i < mapManager.mPathList.Count; ++i)
        {
            Transform obj = mapManager.mPathList[i];
            TileInfomation tileinfomation = obj.GetComponent<TileInfomation>();
            if (tileinfomation.currentTileStyle == TILESTYLE.START)
            {
                Handles.color = Color.green;
            }
            else if (tileinfomation.currentTileStyle == TILESTYLE.END)
            {
                Handles.color = Color.red;
            }

            Handles.SphereCap(i, obj.position + Vector3.up * 0.5f, obj.rotation, 0.5f);
            Handles.color = Color.white;
        }

        if (mapManager.mPathList.Count > 1)
        {
            Handles.color = Color.magenta;

            for (int i = 0; i < mapManager.mPathList.Count - 1; ++i)
            {
                Transform obj1 = mapManager.mPathList[i];
                Transform obj2 = mapManager.mPathList[i + 1];
                Vector3 pos1 = obj1.position + Vector3.up * 0.5f;
                Vector3 pos2 = obj2.position + Vector3.up * 0.5f;

                Handles.DrawLine(pos1, pos2);
            }
            Handles.color = Color.white;
        }
    }

    public void OnSceneGUI()
    {
        if (Application.isPlaying)
        {
            return;
        }

        MapManager mapManager = target as MapManager;
        Handles.BeginGUI();

        if (GUI.Button(new Rect(10, 10, 100, 30), "Normal"))
        {
            mapManager.mEditTileStyle = TILESTYLE.NORMAL;
        }

        if (GUI.Button(new Rect(10, 50, 100, 30), "Straight"))
        {
            mapManager.mEditTileStyle = TILESTYLE.STRAIGHT;
        }

        if (GUI.Button(new Rect(10, 90, 100, 30), "Corner"))
        {
            mapManager.mEditTileStyle = TILESTYLE.CORNER;
        }

        if (GUI.Button(new Rect(10, 130, 100, 30), "Start"))
        {
            mapManager.mEditTileStyle = TILESTYLE.START;
        }

        if (GUI.Button(new Rect(10, 170, 100, 30), "End"))
        {
            mapManager.mEditTileStyle = TILESTYLE.END;
        }

        GUI.color = Color.green;
        GUI.Label(new Rect(120, 10, 500, 30), "Edit Mode : " + mapManager.mEditTileStyle);
        GUI.color = Color.white;

        Handles.EndGUI();

        int ControllID = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(ControllID);

        Event e = Event.current;

        if (e.isKey)
        {
            TILESTYLE nextTileStyle = TILESTYLE.END;
            if(mShortCutTileMap.TryGetValue(e.character, out nextTileStyle))
            {
                mapManager.mEditTileStyle = nextTileStyle;
            }
        }

        if (e.type == EventType.mouseDown || e.type == EventType.mouseDrag)
        {
            if (e.alt)
            {
                return;
            }

            Vector2 mousePosition = Event.current.mousePosition;

            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;
            bool result = Physics.Raycast(ray, out hit, 1000.0f);

            if (result)
            {
                GameObject tileObj = hit.transform.gameObject;
                TileInfomation tileInfomation = tileObj.GetComponent<TileInfomation>();
                if (tileInfomation == null)
                {
                    return;
                }

                if (e.button == 0)// left click button
                {
                    if (e.shift)
                    {
                        if (mapManager.mPathList.Contains(hit.transform) == false)
                        {
                            mapManager.mPathList.Add(hit.transform);
                        }
                    }
                    else
                    {
                        tileInfomation.currentTileStyle = mapManager.mEditTileStyle;
                        tileInfomation.UpdateMaterial();
                    }
                }
                else if (e.button == 1) //right click button
                {
                    if (e.shift)
                    {
                        if (mapManager.mPathList.Contains(hit.transform) == true)
                        {
                            mapManager.mPathList.Remove(hit.transform);
                        }
                    }
                    else
                    {
                        if (e.type == EventType.mouseDown)
                        {
                            hit.transform.localEulerAngles += new Vector3(0.0f, 90.0f, 0.0f);
                        }
                    }
                }
            }
        }

        DrawPathList(mapManager);
    }

    public void CreateTiles(MapManager mapManager)
    {
        int width = mapManager.mCurrentWidth;
        int height = mapManager.mCurrentHeight;

        mapManager.mTiles = new List<GameObject>(width * height+1);

        for (int row = 0; row < width; ++row)
        {
            for (int col = 0; col < height; col++)
            {
                GameObject obj = Instantiate(mapManager.mBaseTilePrefab) as GameObject;
                obj.transform.parent = mapManager.transform;
                obj.transform.localPosition = new Vector3(row, 0.0f, col);
                obj.name = row + "_" + col;
                mapManager.mTiles.Add(obj);

                TileInfomation tileInfo = mapManager.mTiles[row * mapManager.mCurrentWidth + col].GetComponent<TileInfomation>();
                tileInfo.UpdateMaterial();
            }
        }
    }

    public void ShowTiles(MapManager mapManager)
    {
        if (mapManager.mTiles == null)
        {
            return;
        }

        int width = mapManager.mCurrentWidth;
        int height = mapManager.mCurrentHeight;

        EditorGUILayout.LabelField("Tiles Width : " + width);
        EditorGUILayout.LabelField("Tiles Height : " + height);

        for (int row = 0; row < width; ++row)
        {
            for (int col = 0; col < height; ++col)
            {
                string text = string.Format("Tile ({0}),{1})", row, col);
                EditorGUILayout.ObjectField(text, mapManager.mTiles[row * mapManager.mCurrentWidth + col], typeof(GameObject), true);
            }
        }
    }


    void DrawBaseTileObject(MapManager mapManager)
    {
        CommonEditorUI.DrawSeparator();
        GUI.backgroundColor = (mapManager.mBaseTilePrefab != null) ? Color.green : Color.red;

        GameObject baseTilePrefab = (GameObject)EditorGUILayout.ObjectField("Base Tield", mapManager.mBaseTilePrefab,
                                                                            typeof(GameObject), false);

        if (baseTilePrefab != mapManager.mBaseTilePrefab)
        {
            CommonEditorUI.RegisterUndo("Map Base Tile", mapManager);
            mapManager.mBaseTilePrefab = baseTilePrefab;
        }

        GUI.backgroundColor = Color.white;
    }

    void DrawGenerateButton(MapManager mapManager)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Map Data"))
        {
            Debug.Log("Generate Map Data ======= ");
            CreateTiles(mapManager);
        }
        if (GUILayout.Button("Remove Map DAta"))
        {
            Debug.LogWarning("========== Remove Map Data");
            mapManager.RemoveAllTiles();
        }

        EditorGUILayout.EndHorizontal();
    }

    void DrawCurrentMinMax(MapManager mapManager)
    {
        int width = EditorGUILayout.IntField("Current Map Width", mapManager.mCurrentWidth);

        if (width != mapManager.mCurrentWidth)
        {
            CommonEditorUI.RegisterUndo("CurrentMap Width", mapManager);
            mapManager.mCurrentWidth = width;
        }

        int height = EditorGUILayout.IntField("Current Map Height", mapManager.mCurrentHeight);
        if (height != mapManager.mCurrentHeight)
        {
            CommonEditorUI.RegisterUndo("CurrentMap Height", mapManager);
            mapManager.mCurrentHeight = height;
        }

    }

    void DrawMinMaxValues(MapManager mapManager)
    {
        CommonEditorUI.DrawSeparator();

        if (mapManager.mMinWidth <= 20 && mapManager.mMinWidth >= 3)
        {
            GUI.backgroundColor = Color.green;
        }
        else
        {
            GUI.backgroundColor = Color.red;
        }

        int minWidth = EditorGUILayout.IntField("Min Width", mapManager.mMinWidth);
        if (minWidth != mapManager.mMinWidth)
        {
            CommonEditorUI.RegisterUndo("Map Min Width", mapManager);
            mapManager.mMinWidth = minWidth;
        }

        CommonEditorUI.DrawSeparator();


        if (mapManager.mMinHeight <= 20 && mapManager.mMinHeight >= 3)
        {
            GUI.backgroundColor = Color.green;
        }
        else
        {
            GUI.backgroundColor = Color.red;
        }

        int minHeight = EditorGUILayout.IntField("Min Height", mapManager.mMinHeight);
        if (minHeight != mapManager.mMinHeight)
        {
            CommonEditorUI.RegisterUndo("Map Min Height", mapManager);
            mapManager.mMinHeight = minHeight;
        }

        CommonEditorUI.DrawSeparator();
        if (mapManager.mMaxWidth <= 20 && mapManager.mMaxWidth >= 3)
        {
            GUI.backgroundColor = Color.green;
        }
        else
        {
            GUI.backgroundColor = Color.red;
        }

        int maxWidth = EditorGUILayout.IntField("max Width", mapManager.mMaxWidth);
        if (maxWidth != mapManager.mMinHeight)
        {
            CommonEditorUI.RegisterUndo("Map Max Width", mapManager);
            mapManager.mMaxWidth = maxWidth;
        }

        CommonEditorUI.DrawSeparator();

        if (mapManager.mMaxHeight <= 20 && mapManager.mMaxHeight >= 3)
        {
            GUI.backgroundColor = Color.green;
        }
        else
        {
            GUI.backgroundColor = Color.red;
        }

        int maxHeight = EditorGUILayout.IntField("max maxHeight", mapManager.mMaxHeight);
        if (maxHeight != mapManager.mMaxHeight)
        {
            CommonEditorUI.RegisterUndo("Map maxHeight", mapManager);
            mapManager.mMaxWidth = maxWidth;
        }
    }

}
