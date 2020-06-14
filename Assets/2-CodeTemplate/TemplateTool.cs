using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


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
    public static string uiModeleClassPath = "Assets/Lua/UI";

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
            uiModeleClassPath = EditorUtility.OpenFolderPanel("选择文件夹", uiModeleClassPath, "");
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
    }

    private void CreateLogicModule(string name)
    {
        string dstDataPath = string.Format("{0}/{1}Logic.lua", LuaLogicPath, name);
        CreateTemplate(LuaTemplateScriptLogicPath, dstDataPath, name);
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
        FileStream fs = new FileStream(dstPath, FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
        sw.Write(content);
        sw.Close();
        fs.Close();
        AssetDatabase.Refresh();
    }
}
