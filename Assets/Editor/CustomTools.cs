using System.Reflection;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

//注意是静态类
public static class CustomTools
{
    [MenuItem("Custom Tools/ClearConsole _c", false, 1)]
    public static void ClearTheConsole()
    {

        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.SceneView));
        //Debug.Log(assembly.FullName);
        
        System.Type type = assembly.GetType("UnityEditor.LogEntries");
        //Debug.Log(type.Name);
        MethodInfo method = type.GetMethod("Clear");
        method.Invoke(null, null);

        //System.Type[] types = assembly.GetTypes();
        //foreach (Type t in types) {
        //    Debug.Log(t.FullName);
        //}

        //System.Reflection.MethodInfo[] mehthods = type.GetMethods();
        //foreach (MethodInfo m in mehthods) {
        //    Debug.Log(m.Name);
        //}
    }

    [MenuItem("Custom Tools/CustomConsole", false, 1)]
    public static void ShowConsole2()
    {

        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.SceneView));
        //Debug.Log(assembly.FullName);

        System.Type type = assembly.GetType("UnityEditor.ConsoleWindow");
        //Debug.Log(type.Name);
        //MethodInfo method = type.GetMethod("Clear");
        //method.Invoke(null, null);

        //System.Type[] types = assembly.GetTypes();
        //foreach (Type t in types) {
        //    Debug.Log(t.FullName);
        //}

        System.Reflection.MethodInfo[] methods = type.GetMethods();
        foreach (MethodInfo m in methods)
        {
            //Debug.Log(m.Name);
        }
        MethodInfo method = type.GetMethod("ShowConsoleWindow");
        object[] args = new object[1]{true};
        method.Invoke(null,args);
        
        Debug.Log(args[0]);

        System.Type LogEntryType = assembly.GetType("UnityEditor.LogEntries");
        System.Reflection.MethodInfo[] logMethods = LogEntryType.GetMethods();
        foreach (MethodInfo m in logMethods)
        {
            Debug.Log(m.Name);
        }
        MethodInfo filterMethod = LogEntryType.GetMethod("SetFilteringText"); 
        //Debug.Log(filterMethod.Name);
        args[0] = "console";
        filterMethod.Invoke(null,new object[] {"console" });

    }

    [MenuItem("Custom Tools/Choose LogColor")]
    public static void ChooseLogColor() {
        //EditorWindow.GetWindow<>();
    }
}
