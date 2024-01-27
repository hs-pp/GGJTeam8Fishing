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
        
        serializedObject.ApplyModifiedProperties();
    }
}
