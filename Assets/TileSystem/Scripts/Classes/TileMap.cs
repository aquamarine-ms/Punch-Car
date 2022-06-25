using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileSystem
{
    public class TileMap : ITileMap
    {
        private Vector2Int _size;
        private Vector3 _startBoxPos;
        private Vector3 _endBoxPos;
        private int _endGameRow;
        private int _countTopTilesGenerate;
        private int _maxRow;
        private bool _isEnd;
        private ITile[,] _tiles;

        private float _dx;
        private float _dy;

        public TileMap(Vector3 startBoxPos, Vector2Int size, int endGameRow, int countTopTilesGenerate)
        {
            _isEnd = false;
            _endGameRow = endGameRow;
            _countTopTilesGenerate = countTopTilesGenerate;
            _size = size;
            _tiles = new ITile[_size.y, _size.x];
            _startBoxPos = startBoxPos;
            _endBoxPos = startBoxPos + new Vector3Int(1, 1, 0) * new Vector3Int(size.x, size.y, 0);
            _dx = (_endBoxPos.x - _startBoxPos.x) / _size.x;
            _dy = (_startBoxPos.y - _endBoxPos.y) / _size.y;
        }

        public bool CheckTile(Vector2Int pos)
        {
            if (_tiles[pos.y, pos.x] != null) return true;
            return false;
        }

        public bool PositionIsPossible(Vector2Int pos)
        {
            if (pos.x >= _size.x || pos.y >= _size.y || pos.x < 0 || pos.y < 0) return false;
            return true;
        }

        public bool IsEnd()
        {
            return _isEnd;
        }

        public bool BeyondSizeY(Vector2Int pos)
        {
            return pos.y >= _size.y;
        }

        private void CalculateMaxRow()
        {
            bool flag = false;
            int y = 1;
            int x = 0;
            Vector2Int v = new Vector2Int();
            for (y = _size.y - 1; y >= _size.y - _endGameRow - 1; y--)
            {
                for (x = 0; x < _size.x; x++)
                {
                    if (_tiles[y,x] != null)
                    {
                        flag = true;
                        v = new Vector2Int(x, y);
                        break;
                    }
                }

                if (!flag) break;
                flag = false;
            }

            _maxRow = _size.y - y - 1;
            if (_maxRow > _endGameRow && _tiles[v.y,v.x].GetColor() != _tiles[v.y + 1, v.x].GetColor()) _isEnd = true;
        }

        public void SetTile(ITile tile, Vector2Int pos)
        {
            if (!CheckTile(pos) && PositionIsPossible(pos))
            {
                _tiles[pos.y, pos.x] = tile;
                CalculateMaxRow();
            }
        }

        public ITile GetTile(Vector2Int pos)
        {
            if (!PositionIsPossible(pos)) return null;
            return _tiles[pos.y, pos.x];
        }

        public List<ITile> GetTiles(Vector2Int pos, params Vector2Int[] vectors)
        {
            List<ITile> tiles = new List<ITile>();

            for (int i = 0; i < vectors.Length; i++)
            {
                ITile tile = GetTile(pos + vectors[i]);
                if (tile != null)
                    tiles.Add(tile);
            }

            return tiles;
        }

        public List<ITile> GetRandomTilesInLowerLines(int count)
        {
            List<ITile> tiles = new List<ITile>();
            for (int y = 0; y < _countTopTilesGenerate; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    Vector2Int check = new Vector2Int(x, y);
                    if (CheckTile(check))
                    {
                        tiles.Add(GetTile(check));
                    }
                }
            }

            if (tiles.Count > count)
            {
                int dCount = tiles.Count - count;
                /*
                for (int i = 0; i < tiles.Count; i++)
                {
                    if (Random.Range(0f, 1f) < i * 1f / (tiles.Count-dCount))
                    {
                        dCount--;
                        tiles.RemoveAt(i);
                        if (dCount <= 0) break;
                    }
                }*/
                tiles.RemoveRange(0,dCount);
            }

            return tiles;
        }

        public void RemoveTile(ITile tile)
        {
            for (int y = 0; y < _size.y; y++)
            for (int x = 0; x < _size.x; x++)
                if (_tiles[y, x] == tile)
                {
                    _tiles[y, x] = null;
                    return;
                }
        }

        public Vector2Int WorldToTileCoordinate(Vector3 coordinate)
        {
            Vector2 dCoord = coordinate - _startBoxPos;
            return new Vector2Int((int) (dCoord.x / _dx), (int) (dCoord.y / _dy));
        }

        public Vector3 TileToWorldCoordinate(Vector2Int coordinate)
        {
            return new Vector3(this._startBoxPos.x + coordinate.x * _dx, _startBoxPos.y + coordinate.y * _dy, _startBoxPos.z);
        }

        public override string ToString()
        {
            string s = "";
            for (int y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    s += _tiles[y, x] != null ? "X" : "O";
                }

                s += Environment.NewLine;
            }

            return s;
        }
    }
}
