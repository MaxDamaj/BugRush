using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BugRush.System;

public class LevelWindow : EditorWindow {

    [MenuItem("Window/Level Editor")]

    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(LevelWindow));
    }

    public Vector2 scrollView;

    void OnGUI() {
        Level level = GlobalData.Instance.level;
        GUILayout.Label(level.title, EditorStyles.boldLabel);

        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        for (int i = 0; i < level.cells.Count; i += level.columns) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField((i/level.columns).ToString(), EditorStyles.boldLabel, GUILayout.Width(20));
            for (int j = 0; j < level.cells.Count / level.rows; j++) {
                EditorGUILayout.IntField(level.cells[i+j], GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }
}
