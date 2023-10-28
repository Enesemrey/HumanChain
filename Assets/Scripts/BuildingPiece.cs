using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPiece : MonoBehaviour
{
    public bool IsRoof;
    public bool IsWindowed;
    public GameObject[] PieceTypes;
    public GameObject[] Enemies;
    public bool LeftEnemy;
    public bool MiddleEnemy;
    public bool RightEnemy;

    void OnValidate()
    {
        SetMesh();
    }

    public void SetMesh()
    {
        if (PieceTypes.Length < 4) return;
        var index = 0;
        if (IsWindowed) index += 1;
        if (IsRoof) index += 2;
        for (var i = 0; i < PieceTypes.Length; i++)
        {
            var pieceType = PieceTypes[i];
            pieceType.SetActive(i == index);
        }
        if(Enemies.Length < 3) return;
        Enemies[0].SetActive(IsWindowed && LeftEnemy);
        Enemies[1].SetActive(IsWindowed && MiddleEnemy);
        Enemies[2].SetActive(IsWindowed && RightEnemy);
    }
}
