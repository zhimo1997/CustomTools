using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;

public class CodeTemplateWindow : EditorWindow
{
    public string uiModuleMVCName;
    public string uiModuleDataName;
    public string uiModuleLogicName;
    public string uiModuleClassName;

    private static readonly string LuaTemplateScriptMPath = "Assets/Editor/CodeTemplate/LuaTemplateM.txt";
    private static readonly string LuaTemplateScriptVPath = "Assets/Editor/CodeTemplate/LuaTemplateV.txt";
    private static readonly string LuaTemplateScriptCPath = "Assets/Editor/CodeTemplate/LuaTemplateC.txt";
    private static readonly string LuaTemplateScriptDataPath = "Assets/Editor/CodeTemplate/LuaTemplateData.txt";
    private static readonly string LuaTemplateScriptLogicPath = "Assets/Editor/CodeTemplate/LuaTemplateLogic.txt";
    private static readonly string LuaTemplateScriptClassPath = "Assets/Editor/CodeTemplate/LuaTemplateClass.txt";

    private static readonly string LuaMVCPath = "Assets/Lua/UI";
    private static readonly string LuaDataPath = "Assets/Lua/Data";
    private static readonly string LuaLogicPath = "Assets/Lua/LogicScript";
    [NonSerialized]
    public static string uiModeleClassPath = "Assets/Lua/UI";

    // 修改相对应的文件
    // LuaGlobal内添加Logic
    private static readonly string LuaGlobalLogicWatchText = "设置logic";
    private static readonly string LuaGlobalLogicAddText = "    LuaGlobal.#LogicName# = require('LogicScript/#LogicName#')()";
    private static readonly string LuaGlobalPath = "Assets/Lua/Utils/LuaGlobal.lua";
    // DataManager内添加Data
    private static readonly string DataManagerWatchText = "添加Data";
    private static readonly string DataManagerAddText = "    InstanceData.#DataName# = require('Data.#DataName#')()";
    private static readonly string DataManagerPath = "Assets/Lua/Manager/DataManager.lua";

    [MenuItem("====Tools====/代码模板/模板创建")]
    public static void ShowWindow()
    {
        EditorWindow thisWindow = EditorWindow.GetWindow(typeof(CodeTemplateWindow));
        thisWindow.titleContent = new GUIContent("创建代码模板");
        thisWindow.position = new Rect(Screen.width / 2, Screen.height / 2, 600, 800);

    }

    private void Awake()
    {
        uiModeleClassPath = TemplatePathUtil.GetDiskPath(uiModeleClassPath);
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("创建MVC模板，输入UI模块名");
        uiModuleMVCName = EditorGUILayout.TextField(uiModuleMVCName);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("创建") && uiModuleMVCName != null)
        {
            CreateMVCModule(uiModuleMVCName);
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("创建Data模板，输入模块名");
        uiModuleDataName = EditorGUILayout.TextField(uiModuleDataName);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("创建") && uiModuleDataName != null)
        {
            CreateDataModule(uiModuleDataName);
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("创建Logic模板，输入模块名");
        uiModuleLogicName = EditorGUILayout.TextField(uiModuleLogicName);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("创建") && uiModuleLogicName != null)
        {
            CreateLogicModule(uiModuleLogicName);
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("创建类模板");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("指定生成文件夹的位置");
        EditorGUILayout.TextField(uiModeleClassPath);
        if (GUILayout.Button("选择"))
        {
            string selectPath = EditorUtility.OpenFolderPanel("选择文件夹", uiModeleClassPath, "");
            Debug.Log(selectPath);
            if (selectPath != null && selectPath != "") {
                uiModeleClassPath = selectPath;
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("输入模块名");
        uiModuleClassName = EditorGUILayout.TextField(uiModuleClassName);
        if (GUILayout.Button("创建") && uiModuleClassName != null && uiModeleClassPath != null)
        {
            CreateClassModule(uiModuleClassName);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
    }

    private static void Validate() {
        if (EditorApplication.isCompiling || EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("警告", "无法在游戏运行时或代码编译时创建lua脚本", "确定");
            return;
        }
    }

    private static void CreateMVCModule(string name) {
        // 创建文件夹
        string guid = AssetDatabase.CreateFolder(LuaMVCPath, name);
        string parentPath = AssetDatabase.GUIDToAssetPath(guid);
        string dstModelPath = string.Format("{0}/{1}Model.lua", parentPath, name);
        CreateTemplate(LuaTemplateScriptMPath, dstModelPath, name);

        string dstViewPath = string.Format("{0}/{1}View.lua", parentPath, name);
        CreateTemplate(LuaTemplateScriptVPath, dstViewPath, name);

        string dstCtrlPath = string.Format("{0}/{1}Ctrl.lua", parentPath, name);
        CreateTemplate(LuaTemplateScriptCPath, dstCtrlPath, name);
    }

    private void CreateDataModule(string name)
    {
        string dstDataPath = string.Format("{0}/{1}Data.lua", LuaDataPath, name);
        CreateTemplate(LuaTemplateScriptDataPath, dstDataPath, name);
        AddCodeToFile(DataManagerPath, DataManagerWatchText, DataManagerAddText, "#DataName#", name + "Data");
    }

    private void CreateLogicModule(string name)
    {
        string dstDataPath = string.Format("{0}/{1}Logic.lua", LuaLogicPath, name);
        CreateTemplate(LuaTemplateScriptLogicPath, dstDataPath, name);
        AddCodeToFile(LuaGlobalPath, LuaGlobalLogicWatchText, LuaGlobalLogicAddText, "#LogicName#", name + "Logic");
    }

    private void AddCodeToFile(string filePath, string watchText, string codeAddText, string replaceText, string name){
        int index = 0;
        string text = ReadAsset(filePath);
        string[] lineInFile = Regex.Split(text, "\n");
        for (int i = 0; i < lineInFile.Length; i++)
        {
            if (Regex.IsMatch(lineInFile[i], watchText)){
                index = i + 1;
                break;
            }
        }
        string newtext = "";
        for (int i = 0; i < lineInFile.Length + 1; i++)
        {
            if (i < index)
                newtext += lineInFile[i] + "\n";
            if (i == index)
            {
                string fileText = codeAddText;
                fileText = Regex.Replace(fileText, replaceText, name);

                newtext += fileText + "\n";
            }
            if (i > index)
                newtext += lineInFile[i - 1] + "\n";
        }
        if (!string.IsNullOrEmpty(newtext))
        {
            WriteAsset(filePath, newtext, false);
        }
    }

    public static void CreateClassModule(string name)
    {
        string dstDataPath = string.Format("{0}/{1}Class.lua", uiModeleClassPath, name);
        CreateTemplate(LuaTemplateScriptClassPath, dstDataPath, name);
    }

    private static void CreateTemplate(string templateFile, string dstPath, string moduleName) {
        Validate();
        string content = File.ReadAllText(TemplatePathUtil.GetDiskPath(templateFile));
        content = content.Replace("#NAME#", moduleName);
        File.WriteAllText(dstPath, content, new System.Text.UTF8Encoding(false));
        AssetDatabase.Refresh();
    }
    static string ReadAsset(string resPath)
    {
        string text = "";
        if (File.Exists(resPath))
        {
            StreamReader streamReader = new StreamReader(resPath);
            text = streamReader.ReadToEnd();
            streamReader.Close();
        }
        return text;
    }
    static void WriteAsset(string desPath, string text, bool encoderShouldEmitUTF8Identifier = true)
    {
        bool throwOnInvalidBytes = false;
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        StreamWriter streamWriter = new StreamWriter(desPath, append, encoding);
        streamWriter.Write(text);
        streamWriter.Close();
        AssetDatabase.ImportAsset(desPath);
    }
}
