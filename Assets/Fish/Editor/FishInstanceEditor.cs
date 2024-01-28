using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FishInstance))]
public class FishInstanceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_uniqueId"));
        if (GUILayout.Button("Validate", GUILayout.Width(100)))
        {
            FishInstance[] fishInstances = FindObjectsOfType<FishInstance>();
            HashSet<string> uniqueIds = new HashSet<string>();
            foreach (FishInstance fishInstance in fishInstances)
            {
                if (uniqueIds.Contains(fishInstance.UniqueId))
                {
                    Debug.LogError("Duplicate Unique ID: " + fishInstance.UniqueId);
                }
                else
                {
                    uniqueIds.Add(fishInstance.UniqueId);
                }
            }
            Debug.Log("Finished validating Unique IDs");
        }
        GUILayout.EndHorizontal();

        EditorGUI.BeginChangeCheck();
        SerializedProperty fishDefinitionProperty = serializedObject.FindProperty("m_fishDefinition");
        EditorGUILayout.PropertyField(fishDefinitionProperty);
        if (EditorGUI.EndChangeCheck())
        {
            (target as FishInstance).AutoSpawnDefinition(fishDefinitionProperty.objectReferenceValue as FishDefinition);
        }
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_fishName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_waypoints"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_idleDialogues"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_dialogueConfigRunaways"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_hasCaughtOtherFishDialogues"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_fishWasCaughtDialogues"));
        
        serializedObject.ApplyModifiedProperties();
    }

    protected void OnSceneGUI()
    {
        SerializedProperty waypointsSP = serializedObject.FindProperty("m_waypoints");
        
        for (int i = 0; i < waypointsSP.arraySize; i++)
        {
            SerializedProperty waypointSP = waypointsSP.GetArrayElementAtIndex(i);
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.FreeMoveHandle(waypointSP.vector3Value, .5f,
                new Vector3(0.01f, 0.01f, 0), Handles.SphereHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                waypointSP.vector3Value = new Vector3(newPos.x, newPos.y, 0);
                serializedObject.ApplyModifiedProperties();
            }
            
            Vector3 nextPos = i == waypointsSP.arraySize - 1 ? waypointsSP.GetArrayElementAtIndex(0).vector3Value : waypointsSP.GetArrayElementAtIndex(i + 1).vector3Value;
            Handles.DrawDottedLine(waypointSP.vector3Value, nextPos, 10);
            Handles.Label(new Vector3(waypointSP.vector3Value.x, waypointSP.vector3Value.y + 0.5f, 0), i.ToString(), EditorStyles.boldLabel);
        }
    }
}
