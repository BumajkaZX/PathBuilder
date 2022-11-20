#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathBuilder))]
public class PathBuilderEditor : Editor
{
    private PathBuilder pathBuilder = default;
    private void OnEnable()
    {
        pathBuilder = (PathBuilder)target;
        SceneView.duringSceneGui += pathBuilder.DuringScene;
    }
    private void OnDisable() => SceneView.duringSceneGui -= pathBuilder.DuringScene;

    public override void OnInspectorGUI()
    {
        PathBuilder script = (PathBuilder)target;
        SerializedProperty pointRadius = serializedObject.FindProperty("pointRadius");

        serializedObject.Update();

        EditorGUILayout.PropertyField(pointRadius);

        serializedObject.ApplyModifiedProperties();

        GUILayout.Space(20);
        if(GUILayout.Button("Clear list"))
        {
            script.ClearList();
        }
        GUILayout.Space(20);
        GUILayout.BeginVertical();
        EditorGUILayout.HelpBox($"Shift + \n V - Create point \n Q - Previous point \n E - Next point \n R - End point \n C - Remove point", MessageType.Info, true);
        GUILayout.EndVertical();
    }
}
#endif
