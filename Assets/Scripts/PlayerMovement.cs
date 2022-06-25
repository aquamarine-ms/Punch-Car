using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform playerTransform;

    [SerializeField] private float sideMoveDistance = 5;
    [SerializeField] private float sideSpeed = 5;

    [SerializeField] private float rowOffset = -1.5f;
    
    private int _currentRow = 1;

    private bool _canMove;

    private void Start()
    {
        playerTransform = GetComponent<Transform>();

        _canMove = true;
        
        SetPositionToRow(1);
    }
    
    public void MoveToTheSide(Vector3 direction)
    {
        if (!_canMove)
            return;

        _canMove = false;
        
        Vector3 position = playerTransform.position + direction * sideMoveDistance;
        
        iTween.MoveTo(this.gameObject, iTween.Hash("position", position, "speed", sideSpeed,
            "oncomletetarget",this.gameObject, "oncomplete", "MoveComplete","oncompleteparams",direction,"easetype", iTween.EaseType.easeInSine));
    }

    private void MoveComplete(Vector3 direction)
    {
        _canMove = true;
        
        MoveOnRow(direction);
        
        if (IsNeedTeleport(_currentRow, TileCreator.minRows, TileCreator.maxRows))
            SetPositionToRow(GetTeleportRow(_currentRow,  TileCreator.minRows, TileCreator.maxRows));
    }

    private void MoveOnRow(Vector3 direction)
    {
        if (IsMoveToRight(direction))
        {
            _currentRow++;
        }
        else
        {
            _currentRow--;
        }
    }

    private bool IsMoveToRight(Vector3 direction)
    {
        return direction == Vector3.right;
    }

    private bool IsNeedTeleport(int currentRow, int minRow, int maxRow)
    {
        if (currentRow > maxRow)
            return true;
        
        return currentRow < minRow - TileCreator.freeMoveRowCount;
    }
    
    private int GetTeleportRow(int currentRow, int minRow, int maxRow)
    {
        if (currentRow > maxRow)
        {
            return minRow - TileCreator.freeMoveRowCount;
        }

        return maxRow;
    }

    private void SetPositionToRow(int row)
    {
        var position = playerTransform.position;
        position = new Vector3(row - rowOffset, position.y, position.z);
        playerTransform.position = position;
        _currentRow = row;
    }
}
