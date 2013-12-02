using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

[CustomEditor(typeof(PlayerController))]
public class PCScriptEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerController myTarget = (PlayerController)target;

        EditorGUILayout.HelpBox("Character stats", MessageType.Info);
        EditorGUILayout.LabelField("Stat", "Cur. Value");
        foreach (var item in Enum.GetValues(typeof(StatName)))
        {
            BaseStat stat = myTarget.GetBaseStat((StatName)item);
            EditorGUILayout.LabelField(((StatName)item).ToString(), stat.CurValue.ToString());
        }
    }
}
