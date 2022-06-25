using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileSystem
{
    public interface ITileMap
    {
        bool CheckTile(Vector2Int pos);
        bool PositionIsPossible(Vector2Int pos);
        bool IsEnd();
        bool BeyondSizeY(Vector2Int pos);
        void SetTile(ITile tile, Vector2Int pos);
        ITile GetTile(Vector2Int pos);
        List<ITile> GetTiles(Vector2Int pos, params Vector2Int[] vectors);
        List<ITile> GetRandomTilesInLowerLines(int count);
        
        void RemoveTile(ITile tile);

        Vector2Int WorldToTileCoordinate(Vector3 coordinate);
        Vector3 TileToWorldCoordinate(Vector2Int coordinate);
        string ToString();
    }
}
