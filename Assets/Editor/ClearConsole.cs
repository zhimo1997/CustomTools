using System.Reflection;
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

//注意是静态类
public static class ClearConsole
{
    [MenuItem("CustomTools/ClearConsole _c", false, 1)]
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
}
