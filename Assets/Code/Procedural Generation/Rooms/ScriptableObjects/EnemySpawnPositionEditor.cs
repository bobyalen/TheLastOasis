using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EnemySpawnPosition))]
public class EnemySpawnPositionEditor : Editor
{
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sv)
    {
        EnemySpawnPosition esp = (EnemySpawnPosition)target;

        int posIndex = 0;
        List<Vector2> newTargetPos = new List<Vector2>();
        float max = -1;
        float min = 10000;
        if (esp.enemySpawns == null)
            return;
        foreach(EnemySpawn es in esp.enemySpawns)
        {
            //if(es.difficultyRange.y > max)
            //    max = es.difficultyRange.y;
            //if(es.difficultyRange.y < min)
            //    min = es.difficultyRange.y;
        }
        EditorGUI.BeginChangeCheck();
        foreach (EnemySpawn es in esp.enemySpawns)
        {
            Handles.Label(es.spawnPosition + Vector2.up * 0.5f, $"Pos {posIndex}");
            if(es.enemyToSpawn != null)
                Handles.Label(es.spawnPosition, es.enemyToSpawn.name);
            newTargetPos.Add(Handles.PositionHandle(es.spawnPosition, Quaternion.identity));
            posIndex++;
        }
        if(EditorGUI.EndChangeCheck())
        {
            for(int i = 0; i < esp.enemySpawns.Count; i++)
            {
                esp.enemySpawns[i].spawnPosition = new Vector2(Mathf.Round(newTargetPos[i].x * 2.0f) / 2.0f, Mathf.Round(newTargetPos[i].y * 2.0f) / 2.0f);
                
            }
            
        }
    }
}
