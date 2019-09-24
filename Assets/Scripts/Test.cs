using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DebugHelper.Log("test red",Color.red);
        DebugHelper.Log("test green",Color.green);
    }
}
