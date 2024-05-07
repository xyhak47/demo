using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SetQuailty : UnityEditor.Editor
{
    [UnityEditor.MenuItem("WebGLPak/Set Quailty")]
    public static void SetHighQuailty()
    {
        var index = QualitySettings.names.ToList().IndexOf("High");
        QualitySettings.SetQualityLevel(index);
    }

    [UnityEditor.MenuItem("WebGLPak/Get Quailty")]
    public static void GetHighQuailty()
    {
        Debug.LogError(QualitySettings.GetQualityLevel());
    }
}
