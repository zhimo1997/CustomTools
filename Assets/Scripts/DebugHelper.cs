using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugHelper
{
    public static void Log(object message,Color color) {
        string colHtmlString = ColorUtility.ToHtmlStringRGB(color);
        string msg = message.ToString();
        string colorTagStart = "<color=#{0}>";
        string colorTagEnd = "</color>";
        msg = string.Format(colorTagStart,colHtmlString)+msg+colorTagEnd;
        Debug.Log(colHtmlString);
        Debug.Log(msg);
    }
}
