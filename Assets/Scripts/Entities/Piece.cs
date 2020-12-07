using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    

    private ISpawner _spawnerController;
    private GridController _gridController;

    private KeyCode _leftArrow;
    private KeyCode _upArrow;
    private KeyCode _rightArrow;
    private KeyCode _downArrow;

    private const float X_LEFT_COLLISION = -300f;
    private const float X_RIGHT_COLLISION = 240f;
    private const float Y_COLLISION = 660f;
    private const float MOVE_UNIT = 60f;

    private bool _isMoving = false;
    private bool _isFalling = false;
    public float _fallDelay = 0.8f;
    private IEnumerator _fallRoutine;
    public int counter;

    public void SetButtons(bool isDefault = true)
    {
        if (isDefault)
        {
            _leftArrow = KeyCode.LeftArrow;
            _rightArrow = KeyCode.RightArrow;
            _upArrow = KeyCode.UpArrow;
            _downArrow = KeyCode.DownArrow;
        }
        else
        {
            _leftArrow = KeyCode.A;
            _rightArrow = KeyCode.D;
            _upArrow = KeyCode.W;
            _downArrow = KeyCode.S;
        }
    }

    public bool CheckInitialPosition()
    {
        //foreach (Transform block in transform)
        //{
        //    if (!CheckSpaceInGrid(block))
        //    {
        //        AddToGrid();
        //        //_gridController.GameOver();
        //        return false;
        //    }
        //}
        if (!CheckPosition())
        {
            AddToGrid();

            _gridController.GameOver();
            
            return false;
        }
        return true;
    }

    public void StartFalling()
    {
        _isMoving = true;
        _isFalling = true;
        _fallRoutine = Fall();
        StartCoroutine(_fallRoutine);
    }

    void Update()
    {
        if (_isMoving)
        {
            if (Input.GetKeyDown(_leftArrow))
            {
                transform.localPosition += new Vector3(-MOVE_UNIT, 0, 0);
                if (!CheckPosition()) transform.localPosition += new Vector3(MOVE_UNIT, 0, 0);
            }
            else if (Input.GetKeyDown(_rightArrow))
            {
                transform.localPosition += new Vector3(MOVE_UNIT, 0, 0);
                if (!CheckPosition()) transform.localPosition += new Vector3(-MOVE_UNIT, 0, 0);
            }
            else if (Input.GetKeyDown(_upArrow))
            {
                Rotate(-90f);
            }
            if (Input.GetKeyDown(_downArrow))
            {
                StopCoroutine(_fallRoutine);
                //_isFalling = true;
                _fallDelay = 0.05f;
                StartCoroutine(_fallRoutine);
            }
            if (Input.GetKeyUp(_downArrow))
            {
                StopCoroutine(_fallRoutine);
                //_isFalling = false;
                _fallDelay = 0.8f;
                StartCoroutine(_fallRoutine);
            }
        }
    }

    private IEnumerator Fall()
    {
        while (_isFalling)
        {
            yield return new WaitForSeconds(_fallDelay);
            while (GameController.IsPaused) yield return new WaitForSeconds(0.1f);
            transform.localPosition += new Vector3(0, -MOVE_UNIT, 0);
            if (!CheckPosition())
            {
                transform.localPosition += new Vector3(0, +MOVE_UNIT, 0);
                _isFalling = false;
                StartCoroutine(EndMovement());
            }
            //else
            //    yield return new WaitForSeconds(_fallDelay);
        }
    }

    private void Rotate(float degree)
    {
        transform.Rotate(new Vector3(0, 0, degree));
        if (!CheckPosition()) transform.Rotate(new Vector3(0, 0, (-degree)));
        else
        {
            foreach(Transform child in transform)
            {
                child.GetChild(0).transform.Rotate(new Vector3(0, 0, (-degree)));
            }
        }
    }

    private Vector3 GetBlockPosition(Transform block)
    {
        var piece = block.parent.gameObject.transform;

        var xPiecePosition = piece.localPosition.x;
        var yPiecePosition = piece.localPosition.y;

        var xBlockPosition = block.localPosition.x;
        var yBlockPosition =  + block.localPosition.y;

        var rotation = piece.eulerAngles.z;
        //while (rotation < 0) rotation += 360;
        //while (rotation >= 360) rotation -= 360;

        Vector3 position;
        if ((int)Math.Round(rotation) == 90)
            position = new Vector3(-yBlockPosition, xBlockPosition);
        else if ((int)Math.Round(rotation) == 180)
            position = new Vector3(-xBlockPosition, -yBlockPosition);
        else if ((int)Math.Round(rotation) == 270)
            position = new Vector3(yBlockPosition, -xBlockPosition);
        else
            position = new Vector3(xBlockPosition, yBlockPosition, 1);

        position += new Vector3(xPiecePosition, yPiecePosition, 0);
        return position;
    }

    bool CheckPosition()
    {
        foreach (Transform child in transform)
        {

            var blockPosition = GetBlockPosition(child);
            if (Math.Round(blockPosition.x, 1) > X_RIGHT_COLLISION || Math.Round(blockPosition.x) < X_LEFT_COLLISION)
                return false;
            if (Math.Round(blockPosition.y, 1) < - Y_COLLISION)
            {
                return false;
            }
            if (!CheckSpaceInGrid(child))
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator EndMovement()
    {
        if (_isMoving)
        {
            _isMoving = false;
        }
        yield return new WaitForSeconds(0.3f);
        AddToGrid();
        _gridController.CheckBlocks();
        while(_gridController.GetIsHighlightingBlocks()) yield return new WaitForSeconds(0.1f);

        _spawnerController.SummonNextPiece();
    }

    private bool CheckSpaceInGrid(Transform block)
    {
        var blockPosition = GetBlockPosition(block);
        var xPosition = (int)(Math.Round(blockPosition.x - X_LEFT_COLLISION) / MOVE_UNIT);
        var yPosition = (int)(Math.Round(blockPosition.y + Y_COLLISION) / MOVE_UNIT);
        try
        {
            return _gridController.GetBlock(xPosition, yPosition) == null;
        } catch (IndexOutOfRangeException)
        {
            return true;
        }
    }

    private void AddToGrid()
    {
        var blocks = new List<Transform>();
        var positions = new List<Vector3>();
        //var isPositionOk = true;
        foreach (Transform block in transform)
        {
            var blockPosition = GetBlockPosition(block);
            var xPosition = (int)(Math.Round(blockPosition.x - X_LEFT_COLLISION) / MOVE_UNIT);
            var yPosition = (int)(Math.Round(blockPosition.y + Y_COLLISION) / MOVE_UNIT);

            _gridController.SetBlock(block, xPosition, yPosition);
            
            blocks.Add(block);
            positions.Add(block.position);
            
        }
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].parent = _spawnerController.GetGamePanel().transform;
            blocks[i].position = positions[i];
        }
        
    }

    public void SetSpawner(ISpawner spawnerController)
    {
        _spawnerController = spawnerController;
    }

    public void SetGridController(GridController gridController)
    {
        _gridController = gridController;
    }

    public void SetIsFalling(bool isFalling)
    {
        _isFalling = isFalling;
    }

}
