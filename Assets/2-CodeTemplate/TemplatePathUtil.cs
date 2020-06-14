using UnityEngine;
using UnityEditor;
using System;
using System.IO;
public static class TemplatePathUtil
{
    public static string GetAssetPath(string dirPath)
    {
        dirPath = dirPath.Replace("\\", "/");
        if (dirPath.StartsWith(Application.dataPath))
        {
            return "Assets" + dirPath.Replace(Application.dataPath, "");
        }
        return string.Empty;
    }

    public static string GetDiskPath(string assetPath)
    {
        if (string.IsNullOrEmpty(assetPath))
        {
            return string.Empty;
        }
        assetPath = assetPath.Replace("\\", "/");
        if (!assetPath.StartsWith("Assets"))
        {
            return string.Empty;
        }
        return Application.dataPath + assetPath.Substring(assetPath.IndexOf("Assets") + 6);
    }

    public static string GetSelectionAssetDirPath()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            if(obj == null)
            {
                continue;
            }
            path = AssetDatabase.GetAssetPath(obj);
            if(Path.HasExtension(path))
            {
                path = Path.GetDirectoryName(path);
            }
            break;
        }
        return path;
    }
}