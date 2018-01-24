using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BugRush.System;
using System.IO;

public class LevelWindow : EditorWindow {

    [MenuItem("Window/Level Editor")]

    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(LevelWindow));
    }

    public Vector2 scrollView;
    public Level level;
    public string message;
    Vector2 gridSize;


    void OnGUI() {
        if (level == null) level = new Level(50, 50, "emptyLevel");
        if (!Directory.Exists(Application.persistentDataPath + "/Levels")) {
            Directory.CreateDirectory(Application.persistentDataPath + "/Levels");
        }

        //Sliders
        gridSize.x = Mathf.FloorToInt((EditorGUILayout.Slider("New level rows: ", gridSize.x, 50, 500))/50)*50;
        gridSize.y = Mathf.FloorToInt((EditorGUILayout.Slider("New level columns: ", gridSize.y, 50, 500)) / 50) * 50;

        //Buttons
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear level")) {
            level = new Level((int)gridSize.x, (int)gridSize.y, "emptyLevel");
            message = "load black level";
        }
        if (GUILayout.Button("Read level from GlobalData")) {
            level = GlobalData.Instance.level;
            message = "read level from GlobalData";
        }
        if (GUILayout.Button("Write level to GlobalData")) {
            GlobalData.Instance.level = level;
            message = "level written to GlobalData";
        }
        if (GUILayout.Button("Read sample level")) {
            level = JsonUtility.FromJson<Level>(Resources.Load<TextAsset>("sampleLevel").text);
            message = "load sampleLevel";
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        level.title = EditorGUILayout.TextField(level.title);
        if (GUILayout.Button("Load level")) {
            if (File.Exists(Application.persistentDataPath + "/Levels/" + level.title + ".json")) {
                level = JsonUtility.FromJson<Level>(File.ReadAllLines(Application.persistentDataPath + "/Levels/" + level.title + ".json")[0]);
                message = level.title + " loaded";
            } else {
                message = "file not exist";
            }
        }
        if (GUILayout.Button("Save level")) {
            if (level.title != "sampleLevel") {
                List<string> saveFile = new List<string>();
                saveFile.Add(JsonUtility.ToJson(level));
                File.WriteAllLines(Application.persistentDataPath + "/Levels/" + level.title + ".json", saveFile.ToArray());
                message = level.title + " saved";
            } else {
                message = "choose another name";
            }
        }
        EditorGUILayout.LabelField(message, EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        //Level grid
        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        for (int i = 0; i < level.cells.Count; i += level.columns) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField((i / level.columns).ToString(), EditorStyles.boldLabel, GUILayout.Width(30));
            for (int j = 0; j < level.cells.Count / level.rows; j++) {
                level.cells[i + j] = EditorGUILayout.IntField(level.cells[i + j], GUILayout.Width(20));
            }
            EditorGUILayout.LabelField((i / level.columns).ToString(), EditorStyles.boldLabel, GUILayout.Width(30));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();
    }

    bool CheckFileName(string name) {
        if (name == "sampleLevel") return false;
        if (name.Contains(".")) return false;
        if (name.Contains("\\")) return false;
        return true;
    }
}
