using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugHelper
{
    public static void Log(object message)
    {
        string msg = message.ToString();
        Debug.Log(msg);
    }
    public static void Log(object msg,params object[] args) {
        Log(_format(msg, args));
    }
    public static void Log(object message, Color color)
    {
        color = color == null ? Color.white : color;
        string colHtmlString = ColorUtility.ToHtmlStringRGB(color);
        string msg = message.ToString();
        string colorTagStart = "<color=#{0}>";
        string colorTagEnd = "</color>";
        msg = string.Format(colorTagStart,colHtmlString)+msg+colorTagEnd;
        Debug.Log(msg);
    }
    public static void Log(object msg, Color color, params object[] args) {
        Log(_format(msg,args), color);
    }
    private static string _format(object msg, params object[] args)
    {
        string fmt = msg as string;
        if (args.Length == 0 || string.IsNullOrEmpty(fmt))
        {
            return msg.ToString();
        }
        else
        {
            return string.Format(fmt, args);
        }
    }
}
