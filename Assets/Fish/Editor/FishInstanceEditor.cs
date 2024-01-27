using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FishInstance))]
public class FishInstanceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        SerializedProperty guidProperty = serializedObject.FindProperty("m_guid");
        GUI.enabled = false;
        EditorGUILayout.LabelField("GUID", guidProperty.stringValue);
        GUI.enabled = true;
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_fishName"));

        EditorGUI.BeginChangeCheck();
        SerializedProperty fishDefinitionProperty = serializedObject.FindProperty("m_fishDefinition");
        EditorGUILayout.PropertyField(fishDefinitionProperty);
        if (EditorGUI.EndChangeCheck())
        {
            FishInstance fish = target as FishInstance;
            
            // kill all children
            for (int i = 0; i < fish.transform.childCount; i++)
            {
                DestroyImmediate(fish.transform.GetChild(i).gameObject);
            }
            
            if (fishDefinitionProperty.objectReferenceValue != null)
            {
                FishDefinition fishDefinition = fishDefinitionProperty.objectReferenceValue as FishDefinition;
                if (fishDefinition.FishRender == null)
                {
                    Debug.LogError("FishDefinition has no FishRender!");
                    fishDefinitionProperty.objectReferenceValue = null;
                }
                else
                {
                    // create new child
                    GameObject child = Instantiate(fishDefinition.FishRender).gameObject;
                    child.hideFlags = HideFlags.NotEditable;
                    child.transform.SetParent(fish.transform);   
                }
            }
            
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
