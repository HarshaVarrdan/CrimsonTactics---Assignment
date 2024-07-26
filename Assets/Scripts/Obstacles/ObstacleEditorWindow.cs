using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObstacleEditorWindow : EditorWindow
{
    private ObstacleData obstacleData;
    private bool[] obstacleGrid;

    [MenuItem("Tools/Obstacle Editor")]
    public static void ShowWindow()
    {
        GetWindow<ObstacleEditorWindow>("Obstacle Editor");
    }

    private void OnEnable()
    {
        // Load the ObstacleData ScriptableObject
        obstacleData = AssetDatabase.LoadAssetAtPath<ObstacleData>("Assets/ObstacleData.asset");

        if (obstacleData != null)
        {
            obstacleGrid = obstacleData.obstacleGrid;
        }
        else
        {
            obstacleGrid = new bool[100];
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Obstacle Editor Tool", EditorStyles.boldLabel);

        if (obstacleData == null)
        {
            EditorGUILayout.HelpBox("Please create and assign an ObstacleData ScriptableObject", MessageType.Warning);

            if (GUILayout.Button("Create ObstacleData"))
            {
                CreateObstacleData();
            }
            return;
        }

        // Display the 10x10 grid of toggleable buttons
        for (int y = 0; y < 10; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                GUILayout.Label($"({x},{y})", GUILayout.Width(35));
                obstacleGrid[index] = GUILayout.Toggle(obstacleGrid[index], "", GUILayout.Width(20), GUILayout.Height(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Obstacles"))
        {
            SaveObstacles();
        }
    }

    private void CreateObstacleData()
    {
        ObstacleData newObstacleData = ScriptableObject.CreateInstance<ObstacleData>();
        AssetDatabase.CreateAsset(newObstacleData, "Assets/ObstacleData.asset");
        AssetDatabase.SaveAssets();
        obstacleData = newObstacleData;
        obstacleGrid = obstacleData.obstacleGrid;
    }

    private void SaveObstacles()
    {
        if (obstacleData != null)
        {
            obstacleData.obstacleGrid = obstacleGrid;
            EditorUtility.SetDirty(obstacleData);
            AssetDatabase.SaveAssets();
            Debug.Log("Obstacles Saved!");
        }
    }
}
