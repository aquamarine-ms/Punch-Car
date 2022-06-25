using System;
using System.Collections;
using System.Collections.Generic;
using TileSystem;
using UnityEngine;

public class TileHit : MonoBehaviour , IHittable
{
    private TileMovement tileMovement;
    
    private void Start()
    {
        tileMovement = GetComponent<TileMovement>();
    }

    public void TakeHit(Transform player)
    {
        Vector3 playerPosition = player.position;
        ITile tile = GetComponent<ITile>();
        tileMovement.MoveToTheSide(tile,CalculateDirection(playerPosition));
    }

    private Vector3 CalculateDirection(Vector3 hitPosition)
    {
        return hitPosition.x > tileMovement.tileTransform.position.x ? Vector3.left : Vector3.right;
    }
}
