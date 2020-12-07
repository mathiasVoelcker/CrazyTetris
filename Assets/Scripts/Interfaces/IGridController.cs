using System;
using UnityEngine;

public interface IGridController
{
    void CheckBlocks();

    Transform GetBlock(int xPosition, int yPosition);

    void SetBlock(Transform block, int xPosition, int yPosition);

    bool GetIsHighlightingBlocks();
}
