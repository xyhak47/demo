using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMemorySize : UnityEditor.Editor
{
    [UnityEditor.MenuItem("WebGLPak/Set Size")]
    public static void SetSize()
    {
        UnityEditor.PlayerSettings.WebGL.memorySize = 2 * 1024 * 1024 * 1000;
        /*UnityEditor.PlayerSettings.WebGL.memorySize = 10 * 1024 * 1024;*/
    }

    [UnityEditor.MenuItem("WebGLPak/Get Size")]
    public static void GetSize()
    {
        Debug.LogError(UnityEditor.PlayerSettings.WebGL.memorySize);
    }
}
