#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DataTableMaker
{
    [MenuItem("Assets/BY/Create binary File form CSV (text file)", false, 1)]
    private static void CreateBinaryFile()
    {
        foreach(Object obj in Selection.objects)
        {
            TextAsset csvFile = (TextAsset)obj;
            string tableName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(csvFile));

            ScriptableObject scriptableObject = ScriptableObject.CreateInstance(tableName);
            if (scriptableObject == null)
                return;

            AssetDatabase.CreateAsset(scriptableObject, "Assets/Resources/Configs/" + tableName + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            ConfigDatabase configDatabase = (ConfigDatabase)scriptableObject;
            configDatabase.CreateBinaryFile(csvFile);
            EditorUtility.SetDirty(configDatabase);
        }
    }
}
#endif
