using System;
using UnityEngine;

public interface ISpawner
{
    void SummonNextPiece();

    GameObject GetGamePanel();

    void Run();

    Piece CreateNextPiece();
}
