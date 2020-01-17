using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using Microsoft.VisualBasic;

public static class LuaScriptCreater
{
    private static readonly string LuaScriptIconPath = "Assets/Tools/ScriptTemplates/lua_icon64.png";
    private static readonly string LuaScriptTemplatePath = "Assets/1-LuaTemplate/Editor/LuaTemplate.txt";

    [MenuItem("Assets/Create/Lua Script", false, 81)]
    private static void CreateLuaScript()
    {
        CreateLuaTemplate(".lua");
    }

    [MenuItem("Assets/Create/xLua Script", false, 81)]
    private static void CreateXLuaScript()
    {
        CreateLuaTemplate(".txt");
    }

    private static void CreateLuaTemplate(string ext){
        if (EditorApplication.isCompiling || EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "无法在游戏运行时或代码编译时创建lua脚本", "确定");
            return;
        }

        Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>(LuaScriptIconPath);
        string scriptDirPath = PathUtil.GetSelectionAssetDirPath();
        
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
                ScriptableObject.CreateInstance<CreateLuaScriptAction>(),
                scriptDirPath + "/NewLuaScript"+ext, icon,
                LuaScriptTemplatePath);
        
    }
}

internal class CreateLuaScriptAction : EndNameEditAction
{
    public override void Action(int instanceId, string pathName, string resourceFile)
    {

        string content = File.ReadAllText(PathUtil.GetDiskPath(resourceFile));
        string fileName = Path.GetFileNameWithoutExtension(pathName);
        content = content.Replace("#NAME#", fileName);

        string fullName = PathUtil.GetDiskPath(pathName);
        File.WriteAllText(fullName, content);
        AssetDatabase.ImportAsset(pathName);

        string ext=Path.GetExtension(pathName);

        if(ext==".txt"){
            string oldPathName=pathName;
            string newPathName=Path.ChangeExtension(pathName,".lua");
            newPathName+=".txt";

            if(File.Exists(PathUtil.GetDiskPath(oldPathName))){
                File.Move(PathUtil.GetDiskPath(oldPathName),PathUtil.GetDiskPath(newPathName));
                pathName=newPathName;
            }
        }

        Object obj = AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));

        ProjectWindowUtil.ShowCreatedAsset(obj);
        AssetDatabase.Refresh();
    }
}