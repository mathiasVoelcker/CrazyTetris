using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static readonly int LINES = 20;
    public static readonly int COLUMNS = 10;
    public static readonly int BOMB_RANGE = 1;

    public int blocksSequence { get; set; } = 1;

    [SerializeField]
    protected GameController GameController;
    
    [SerializeField]
    private AudioSource popSound;

    protected Transform[,] Grid = new Transform[LINES, COLUMNS];

    protected bool IsHighlightingBlocks = false;

    public virtual void CheckBlocks(bool isSequence = false)
    {
        var blocksToRemove = new List<Block>();
        List<Block> blocksToRemoveInLine;
        for (int l = 0; l < LINES; l++)
        {
            blocksToRemoveInLine = new List<Block>();
            for (int c = 0; c < COLUMNS; c++)
            {
                var block = Grid[l, c];
                if (block == null)
                    c = COLUMNS;
                else
                    blocksToRemoveInLine.Add(new Block(block, l, c));
            }
            if (blocksToRemoveInLine.Count == COLUMNS)
                blocksToRemove.AddRange(blocksToRemoveInLine);
        }

        if (blocksToRemove.Any())
            StartCoroutine(RemoveBlocks(blocksToRemove, false));
    }

    public void Reset()
    {
        for(int l = 0; l < LINES; l++)
        {
            for (int c = 0; c < COLUMNS; c++)
            {
                var block = Grid[l, c];
                if (block == null) continue;
                Destroy(block.gameObject);
                block.parent = null;
                Grid[l, c] = null;
            }
        }
        IsHighlightingBlocks = false;
        blocksSequence = 1;
    }

    protected IEnumerator RemoveBlocks(List<Block> blocksToScore, bool dropEveryBlock = true)
    {
        var blocksBombed = GetBlocksBombed(blocksToScore);

        var blocksToRemove = blocksToScore.Union(blocksBombed).ToList();

        StartCoroutine(HighlightBlocks(blocksToRemove));
        yield return new WaitForSeconds(1f);
        UnhighlightBlocks(blocksToRemove);
        IsHighlightingBlocks = false;

        GameController.AddScore(blocksToScore, blocksBombed, blocksSequence);
        blocksToRemove = blocksToRemove.OrderByDescending(x => x.Line).ToList();
        foreach (var block in blocksToRemove)
        {
            foreach (Transform child in block.BlockObject.gameObject.transform)
            {
                foreach (Transform grandChild in child)
                {
                    grandChild.parent = null;
                    Destroy(grandChild.gameObject);
                }
                child.parent = null;
                Destroy(child.gameObject);
            }
            Destroy(block.BlockObject.gameObject);
            Grid[block.Line, block.Column] = null;
            //DropBlocksAbove(block.Line, block.Column);
        }

        if (dropEveryBlock)
            DropBlocks();
        CheckBlocks(isSequence: true);
    }

    private List<Block> GetBlocksBombed(List<Block> blocksToRemove)
    {
        var newBlocksToRemote = new List<Block>();
        foreach (var block in blocksToRemove.Where(x => x.IsBombBlock()))
        {
            for (int i = -BOMB_RANGE; i <= BOMB_RANGE; i++)
            {
                for (int j = -BOMB_RANGE; j <= BOMB_RANGE; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var line = block.Line + i;
                    if (line < 0 || line >= LINES) continue;
                    var column = block.Column + j;
                    if (column < 0 || column > COLUMNS) continue;
                    if (Grid[line, column] == null) continue;
                    if (blocksToRemove.Any(x => x.Line == line && x.Column == column)) continue;
                    if (newBlocksToRemote.Any(x => x.Line == line && x.Column == column)) continue;
                    newBlocksToRemote.Add(new Block(Grid[line, column], line, column));
                }
            }
        }

        return newBlocksToRemote;
    }

    private void DropBlocksAbove(int line, int c, int fallLines = 1)
    {
        for (var l = line + 1; l < LINES; l++)
        {
            if (Grid[l, c] != null)
            {
                FallBlock(l, c);
            }
        }
    }

    void DropBlocks()
    {
        for (int c = 0; c < COLUMNS; c++)
        {
            for (int l = 1; l < LINES; l++)
            {
                if (Grid[l, c] != null && Grid[l - 1, c] == null)
                {
                    var fallingLine = l;
                    do
                    {
                        FallBlock(fallingLine, c);
                        fallingLine--;
                    } while (fallingLine > 0 && Grid[fallingLine - 1, c] == null);
                }
            }
        }
    }

    void FallUnattachedPieces()
    {
        for (var l = 1; l < LINES; l++)
        {
            for (var c = 0; c < COLUMNS; c++)
            {
                if (Grid[l, c] != null)
                {
                    if ((l + 1 == LINES || Grid[l + 1, c] == null)
                        && Grid[l - 1, c] == null
                        && (c + 1 == COLUMNS || Grid[l, c + 1] == null)
                        && (c == 0 || Grid[l, c - 1] == null))
                        FallBlock(l, c);

                }
            }
        }
    }

    private void FallBlock(int l, int c)
    {
        Grid[l, c].localPosition += new Vector3(0, -60, 0);
        Grid[l - 1, c] = Grid[l, c];
        Grid[l, c] = null;
    }

    private IEnumerator HighlightBlocks(List<Block> blockList)
    {
        var timeInterval = 1.0f / blockList.Count ;
        IsHighlightingBlocks = true;
        for (var i = 0; i < blockList.Count; i++)
        {
            var block = blockList[i];
            if (block.BlockObject.localScale.x == 0f)
            {
                blockList.RemoveAt(i);
                continue;
            }
            block.BlockObject.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            yield return new WaitForSeconds(timeInterval);
            if (GameController.IsSoundActive)
                popSound.PlayOneShot(popSound.clip);
            block.BlockObject.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    private void UnhighlightBlocks(List<Block> blockList)
    {
        foreach (var block in blockList)
        {
            block.BlockObject.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        IsHighlightingBlocks = false;
    }

    public Transform GetBlock(int xPosition, int yPosition)
    {
        return Grid[yPosition, xPosition];
    }

    public bool GetIsHighlightingBlocks()
    {
        return IsHighlightingBlocks;
    }

    public bool SetBlock(Transform block, int xPosition, int yPosition)
    {
        try
        {
            Grid[yPosition, xPosition] = block;
            return true;
        } catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    public void GameOver()
    {
        GameController.GameOver();
    }
}
