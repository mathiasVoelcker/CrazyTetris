using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorTetrisGameController : GameController
{
    [SerializeField]
    private GameObject PieceSizePanel;

    [SerializeField]
    private GameObject ScorePanel;

    [SerializeField]
    private GameObject PieceSizeText;

    [SerializeField]
    private GameObject SizeBlockText; 

    [SerializeField]
    private Animator BlockSizeAnimator;


    public int pieceSize { get; set; }

    private int _scoreToNextLevel;

    private int _nextScoreAddition;

    private int _currentLevel;


    private ColorTetrisSpawnerController _colorTetrisSpawnerController;

    public override void StartGame()
    {
        PieceSizeText.gameObject.GetComponent<Text>().text = "3";
        pieceSize = 3;
        _currentLevel = 1;
        _scoreToNextLevel = 50;
        _nextScoreAddition = 75;
        PieceSizePanel.SetActive(true);
        ScorePanel.SetActive(true);
        base.StartGame();
    }

    public override void ResetGame()
    {
        PieceSizePanel.SetActive(false);
        ScorePanel.SetActive(false);
        ColorTetrisSpawnerController.ActiveColors.Clear();
        ColorTetrisSpawnerController.ActiveColors.AddRange(ColorTetrisSpawnerController.StartColors);
        ColorTetrisSpawnerController.BombProb = 0f;
        base.ResetGame();
    }

    void Start()
    {
        _colorTetrisSpawnerController = (ColorTetrisSpawnerController)SpawnerController;
    }

    public override void AddScore(List<Block> blocksToRemove, int seqMultiplier)
    {
        if (_currentLevel > 1)
        {
            var blocksByColor = blocksToRemove.GroupBy(x => x.BlockObject.GetColor());
            foreach (var piece in blocksByColor)
            {
                if (piece.Count() > pieceSize)
                {
                    var blocks = piece.ToList();
                    while(blocks.Count >= pieceSize)
                    {
                        AddNewPiece(blocks.Take(pieceSize).ToList());
                        blocks.RemoveRange(0, pieceSize);
                    }
                }
                else
                {
                    AddNewPiece(piece.ToList());
                }
            }
        }
        
        var pieceCount = blocksToRemove.Count / pieceSize;
        
        var specialPieces = blocksToRemove.Where(x => Math.Round(x.BlockObject.GetColor().r, 2) == 0.32).ToList();
        var specialPieceCount = specialPieces.Count / pieceSize;
        var scoreToAdd = (pieceCount + specialPieceCount) * (Math.Pow(2, pieceSize) * seqMultiplier);
        var newScore = ScoreController.AddScore((int)scoreToAdd);
        if (newScore >= _scoreToNextLevel)
        {
            _currentLevel = LevelController.AddLevel();
            _scoreToNextLevel += _nextScoreAddition;
            _nextScoreAddition = (int)(_nextScoreAddition * 1.5);
            if (_currentLevel == 2)
            {
                StartCoroutine(RaisePieceSize());
                PieceSizeText.gameObject.GetComponent<Text>().text = "" + pieceSize;
            }
            if (_currentLevel == 5)
            {
                StartCoroutine(RaisePieceSize());
                PieceSizeText.gameObject.GetComponent<Text>().text = "" + pieceSize;
            }
            if (_currentLevel == 7)
            {
                ColorTetrisSpawnerController.AddNextColor();
                ColorTetrisSpawnerController.BombProb = 0.05f;
                //ColorTetrisSpawnerController.AddNextColor();
            }
            if (_currentLevel == 9)
            {
                StartCoroutine(RaisePieceSize());
                PieceSizeText.gameObject.GetComponent<Text>().text = "" + pieceSize;
            }
        }
    }

    public void AddNewPiece(List<Block> blocksToRemove)
    {
        GameObject newPiece;
        Piece pieceComponent;

        if (IsPieceI(blocksToRemove))
            _colorTetrisSpawnerController.AddStaticPieceToPieces("I");
        else if (IsPieceO(blocksToRemove))
            _colorTetrisSpawnerController.AddStaticPieceToPieces("O");
        else
        {
            newPiece = new GameObject("newBlock");
        
            pieceComponent = newPiece.AddComponent<Piece>();

            var xOffset = GetOffset(blocksToRemove, Direction.X);
            var yOffset = GetOffset(blocksToRemove, Direction.Y);

            foreach (var block in blocksToRemove)
            {

                Instantiate(block.BlockObject, pieceComponent.transform);
            }
            for (int i = 0; i < pieceComponent.transform.childCount; i++)
            {
                var child = pieceComponent.transform.GetChild(i);
                var newX = child.transform.localPosition.x + xOffset;
                var newY = child.transform.localPosition.y + yOffset;
                child.transform.localPosition = new Vector3(newX, newY, 1);
            }
            SpawnerController.Pieces.Add(pieceComponent);
            if (SpawnerController.Pieces.Count > 10)
                SpawnerController.Pieces.RemoveAt(0);

            newPiece.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    private float GetOffset(List<Block> blocksToRemove, Direction direction)
    {
        IEnumerable<IGrouping<float, Block>> groupedBlocksBySize; 
        if (direction == Direction.X)
            groupedBlocksBySize = blocksToRemove.GroupBy(x => x.BlockObject.localPosition.x);
        else
            groupedBlocksBySize = blocksToRemove.GroupBy(x => x.BlockObject.localPosition.y);
        if (groupedBlocksBySize.Count() % 2 != 0)
        {
            groupedBlocksBySize = groupedBlocksBySize.OrderBy(x => x.Key);
            var midGroupIndex = (groupedBlocksBySize.Count() / 2);
            var midGroup = groupedBlocksBySize.ToList()[midGroupIndex];
            return -midGroup.Key;
        }
        else if (groupedBlocksBySize.Count() == 2)
        {
            var biggerGroup = groupedBlocksBySize.OrderBy(x => x.Count()).Last();
            return -biggerGroup.Key;
        }   
        else
        {
            groupedBlocksBySize = groupedBlocksBySize.OrderBy(x => x.Key);
            var midGroupIndex = (groupedBlocksBySize.Count() / 2);
            var midGroup = groupedBlocksBySize.ToList()[midGroupIndex - 1];
            return -midGroup.Key;
        }   
    }

    private bool IsPieceI(List<Block> blocksToRemove)
    {
        if (blocksToRemove.Count == 4)
        {
            var xPos = (int)blocksToRemove.First().BlockObject.localPosition.x;
            var yPos = (int)blocksToRemove.First().BlockObject.localPosition.y;

            if (blocksToRemove.All(x => Math.Round(x.BlockObject.localPosition.x, 0) == xPos))
                return true;
            if (blocksToRemove.All(x => Math.Round(x.BlockObject.localPosition.y, 0) == yPos))
                return true;
        }

        return false;
    }

    private bool IsPieceO(List<Block> blocksToRemove)
    {
        if (blocksToRemove.Count == 4)
        {
            var firstBlock = blocksToRemove[0].BlockObject;
            float maxX = firstBlock.localPosition.x,
                maxY = firstBlock.localPosition.y,
                minX = firstBlock.localPosition.x,
                minY = firstBlock.localPosition.y;

            for (int i = 1; i < 4; i++)
            {
                var block = blocksToRemove[i].BlockObject;
                if (block.localPosition.x > maxX)
                    maxX = block.localPosition.x;
                if (block.localPosition.y > maxY)
                    maxY = block.localPosition.y;
                if (block.localPosition.x < minX)
                    minX = block.localPosition.x;
                if (block.localPosition.y < minY)
                    minY = block.localPosition.y;
            }
            if (Math.Round(maxX - minX) == 60 && Math.Round(maxY - minY) == 60) return true;
        }

        return false;
    }

    private IEnumerator RaisePieceSize()
    {
        pieceSize++;
        SizeBlockText.GetComponent<Text>().text = pieceSize + " BLOCKS";
        SizeBlockText.GetComponent<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1);
        SizeBlockText.SetActive(true);
        BlockSizeAnimator.Play("Size Block Fade", -1, 0f);
        yield return new WaitForSeconds(0.1f);
    }
}