using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(BuildingController))]
public class BuildingControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var level = target as BuildingController;

        if (GUILayout.Button("Generate"))
        {
            if (level.Pieces == null)
                level.Pieces = new List<BuildingPiece>();
            else
            {
                foreach (var buildingPiece in level.Pieces)
                {
                    if (buildingPiece == null) continue;
                    else
                        DestroyImmediate(buildingPiece.gameObject);
                }

                level.Pieces = new List<BuildingPiece>();
            }

            for (int i = 0; i < level.FloorCount; i++)
            {
                var levelPieceInstance = PrefabUtility.InstantiatePrefab(level.PiecePrefab, level.transform) as BuildingPiece;
                levelPieceInstance.transform.localPosition = Vector3.down * i * level.PieceHeight;
                if (i == 0)
                    levelPieceInstance.IsRoof = true;
                levelPieceInstance.SetMesh();
                level.Pieces.Add(levelPieceInstance);
            }

            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}