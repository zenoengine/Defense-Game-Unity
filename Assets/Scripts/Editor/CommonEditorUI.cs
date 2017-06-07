using UnityEngine;
using UnityEditor;
using System.Collections;

public class CommonEditorUI : Editor
{

    public static void DrawSeparator()
    {
        DrawSeparator(Color.gray);
    }

    public static void DrawSeparator(Color color)
    {
        EditorGUILayout.Space();

        Texture2D tex = new Texture2D(1, 1);
        GUI.color = color;

        float y = GUILayoutUtility.GetLastRect().yMax;
        GUI.DrawTexture(new Rect(0.0f, y, Screen.width, 1.0f), tex);
        tex.hideFlags = HideFlags.DontSave;
        GUI.color = Color.white;

        EditorGUILayout.Space();
    }

    public static void RegisterUndo(string name, Object obj)
    {
        if (obj != null)
        {
            Undo.RecordObject(obj, name);
            EditorUtility.SetDirty(obj);
        }
    }
}
