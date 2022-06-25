using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileSystem
{
    public interface ITile
    {
        void SetTileMap(ITileMap map);
        void SetIsStop(bool stop);
        bool GetIsStop();
        Vector2Int GetTilePosition();
        Vector3 GetPosition();
        List<ITile> GetNearTiles();
        void PlayDestroyEffect();
        void Stop();
        void Run();
        void Perform();
        void Perform(float time);
        void Delete(float timeToDelete);
        void Dash(Vector2 direction);
        void SetPositionOnTileMap(Vector2Int pos);
        bool IsDown();
        ColorTile GetColor();
        int GetScore();
        void DestroyFromMatch();
    }

    public enum ColorTile
    {
        Red,
        Green,
        Blue,
        Yellow
    }
}
