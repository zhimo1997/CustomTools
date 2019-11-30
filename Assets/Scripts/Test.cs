using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //DebugHelper.Log("test white");
        DebugHelper.Log("first log");
        DebugHelper.Log("test red",Color.red);
        DebugHelper.Log("test green",Color.green);
        DebugHelper.Log("test blue",Color.blue);
        DebugHelper.Log("%d:custom color and format",new Color(1,0.5f,0.5f),4);
        
        //ConsoleProDebug.LogToFilter("ecp error","Error");
        //ConsoleProDebug.LogToFilter("console pro debug","xx");
        //ConsoleProDebug.LogToFilter("console pro debug2","xx2");
        //ConsoleProDebug.PrintMethodInfos();
    }

    private void Update()
    {
        //transform.Translate(1*Time.deltaTime,0,0);
        //ConsoleProDebug.LogToFilter(transform.position.x.ToString(), "watch");
        //ConsoleProDebug.Watch("cube pos",transform.position.x.ToString());
        //ConsoleProDebug.Watch("cube pos",transform.position.y.ToString());
    }
}
