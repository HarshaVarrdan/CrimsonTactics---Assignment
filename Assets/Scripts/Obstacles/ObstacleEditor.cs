using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleManager))]
public class ObstacleEditor : Editor
{
    private bool[] grid = new bool[100];
    private ObstacleData obstacleData;

    private void OnEnable()
    {
        ObstacleManager manager = (ObstacleManager)target;
        obstacleData = manager.obstacleData;
        grid = obstacleData.obstacleGrid;
    }

    // This function creates Toggleable buttons in inspector panel for us to customize
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        for (int y = 0; y < 10; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++)
            {
                int index = y * 10 + x;
                EditorGUILayout.LabelField($"({x},{y})", GUILayout.Width(30));
                grid[index] = EditorGUILayout.Toggle(grid[index]);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Obstacles"))
        {
            SaveObstacles();
        }
    }

    // this saves the data to the obstacleData.
    private void SaveObstacles()
    {
        ObstacleManager manager = (ObstacleManager)target;
        manager.obstacleData.obstacleGrid = grid;
        EditorUtility.SetDirty(manager.obstacleData);
    }
}
