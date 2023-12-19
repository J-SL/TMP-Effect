using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CharacterOperate))]
public class CharacterOperateEditor : Editor
{
    private SerializedProperty AnimateColor;

    private SerializedProperty isRefreshColorProperty;
    private SerializedProperty refreshColorTimeProperty;
    private SerializedProperty transparencyProperty;
    private SerializedProperty isSolidColorProperty;

    private SerializedProperty AnimateJitter;
    private SerializedProperty isJitterProperty;
    private SerializedProperty isCustomJitterProperty;
    private SerializedProperty customXProperty;
    private SerializedProperty customYProperty;
    private SerializedProperty rotateExtentProperty;

    private SerializedProperty AnimateWave;
    private SerializedProperty isWaveProperty;
    private SerializedProperty heightProperty;



    private void OnEnable()
    {
        AnimateColor = serializedObject.FindProperty("AnimateColor");

        isRefreshColorProperty = serializedObject.FindProperty("isRefreshColor");
        refreshColorTimeProperty = serializedObject.FindProperty("refreshColorTime");
        transparencyProperty = serializedObject.FindProperty("transparency");
        isSolidColorProperty = serializedObject.FindProperty("isSolidColor");
        //
        AnimateJitter = serializedObject.FindProperty("AnimateJitter");

        isJitterProperty = serializedObject.FindProperty("isJitter");
        isCustomJitterProperty = serializedObject.FindProperty("isCustomJitter");
        customXProperty = serializedObject.FindProperty("customX");
        customYProperty = serializedObject.FindProperty("customY");
        rotateExtentProperty = serializedObject.FindProperty("rotateExtent");
        //
        AnimateWave=serializedObject.FindProperty("AnimateWave");
        isWaveProperty = serializedObject.FindProperty("isWave");
        heightProperty = serializedObject.FindProperty("height");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        AnimateColor.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(AnimateColor.boolValue, "AnimateColor");
        EditorGUILayout.EndFoldoutHeaderGroup();

        if (AnimateColor.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(isRefreshColorProperty);
            if (isRefreshColorProperty.boolValue)
            {
                EditorGUILayout.PropertyField(refreshColorTimeProperty);
                EditorGUILayout.PropertyField(transparencyProperty);
                EditorGUILayout.PropertyField(isSolidColorProperty);
            }
            EditorGUI.indentLevel--;
        }


        AnimateJitter.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(AnimateJitter.boolValue, "AnimateJitter");
        EditorGUILayout.EndFoldoutHeaderGroup();

        if (AnimateJitter.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(isJitterProperty);
            if (isJitterProperty.boolValue)
            {
                EditorGUILayout.PropertyField(isCustomJitterProperty);
                if (isCustomJitterProperty.boolValue)
                {
                    EditorGUILayout.PropertyField(customXProperty);
                    EditorGUILayout.PropertyField(customYProperty);
                }
                EditorGUILayout.PropertyField(rotateExtentProperty);
            }
            EditorGUI.indentLevel--;
        }

        AnimateWave.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(AnimateWave.boolValue, "AnimateWave");
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (AnimateWave.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(isWaveProperty);
            if (isWaveProperty.boolValue)
            {
                EditorGUILayout.PropertyField(heightProperty);
            }
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}