using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Method))]
public class MethodEditor : Editor
{
    private SerializedProperty methodName;
    private SerializedProperty methodDescription;
    private SerializedProperty pointValue;

    private void OnEnable()
    {
        methodName = serializedObject.FindProperty("methodName");
        methodDescription = serializedObject.FindProperty("methodDescription");
        pointValue = serializedObject.FindProperty("pointValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Display other fields
        EditorGUILayout.PropertyField(methodName);
        EditorGUILayout.PropertyField(pointValue);

        // Dynamically adjust height based on text content
        float contentHeight = Mathf.Max(100, methodDescription.stringValue.Split('\n').Length * 18); // Estimate line height
        methodDescription.stringValue = EditorGUILayout.TextArea(methodDescription.stringValue, GUILayout.Height(contentHeight));

        serializedObject.ApplyModifiedProperties();
    }
}