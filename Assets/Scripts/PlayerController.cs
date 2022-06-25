using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement), typeof(PlayerRaycaster))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerRaycaster _playerRaycaster;
    

    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerRaycaster = GetComponent<PlayerRaycaster>();
    }

    public void Left()
    {
        MoveSide(Vector3.left);
    }

    public void Right()
    {
        MoveSide(Vector3.right);
    }

    private void MoveSide(Vector3 direction)
    { 

        if (_playerRaycaster.Hit(direction))
            return;

        _playerMovement.MoveToTheSide(direction);
    }
}
