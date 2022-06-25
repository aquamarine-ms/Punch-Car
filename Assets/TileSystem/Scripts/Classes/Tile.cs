using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TileSystem
{
    public class Tile : MonoBehaviour, ITile, IHittable
    {
        private ITileMap _map;
        [SerializeField] private ColorTile _color;
        [SerializeField] private int _score;

        [SerializeField] private bool isStop = true;

        [SerializeField] private float timeToDelete = 2f;

        private bool isDown = false;
        private bool isTrash = false;
        private TileMovement _tileMovement;
        private TileAnimation _tileAnimation;

        void Start()
        {
            _tileMovement = GetComponent<TileMovement>();
            _tileAnimation = GetComponent<TileAnimation>();
        }
        
        private void Update()
        {
            CheckTile();
        }
        
        public void SetTileMap(ITileMap map)
        {
            _map = map;
        }

        public Vector2Int GetTilePosition()
        {
            return _map.WorldToTileCoordinate(transform.position);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public List<ITile> GetNearTiles()
        {
            List<ITile> tiles = new List<ITile>();
            tiles.Add(this);
            for (int i = 0; i < tiles.Count; i++)
            {
                if (tiles[i].GetColor() != _color)
                {
                    tiles.Remove(tiles[i]);
                    i--;
                }
                else
                {
                    Vector2Int pos = tiles[i].GetTilePosition();
                    tiles.AddRange(_map.GetTiles(pos, Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right));
                    tiles = tiles.Distinct().ToList();
                }
            }
            if (tiles.Count == 1) tiles.Remove(tiles[0]);
            return tiles;
        }

        private void CheckTile()
        {
            if (!isStop)
            {
                if (!isDown)
                {
                    Vector2Int tileCoord = _map.WorldToTileCoordinate(_tileMovement.tileTransform.position) +
                                           Vector2Int.up;
                    if (!isTrash)
                    {
                        if (_map.PositionIsPossible(tileCoord) && !_map.CheckTile(tileCoord))
                        {
                            _tileMovement.StartMoving();
                        }
                        else
                        {
                            Stop();
                        }
                    }
                    else
                    {
                        _tileMovement.StartMoving();
                        if (_map.BeyondSizeY(tileCoord))
                        {
                            Delete(0);
                            //TODO: если улетели ниже то что то сделать
                        }
                    }
                }
                else
                {
                    Vector2Int tileCoord = _map.WorldToTileCoordinate(_tileMovement.tileTransform.position) +
                                           Vector2Int.up;
                    if (_map.PositionIsPossible(tileCoord))
                    {
                        isDown = _map.CheckTile(tileCoord);
                    }

                    if (!isDown) _map.RemoveTile(this);
                }
            }
        }

        public void SetIsStop(bool stop)
        {
            isStop = stop;
        }

        public bool GetIsStop()
        {
            return isStop;
        }
        
        public void PlayDestroyEffect()
        {
            
        }

        public void Stop()
        {
            _tileMovement.StopMoving();
            isDown = true;
            Vector2Int pos = _map.WorldToTileCoordinate(transform.position);
            SetPositionOnTileMap(pos);
            List<ITile> tiles = GetNearTiles();
            if (tiles.Count > 0)
            {
                int allScore = 0;
                this.DestroyFromMatch();
                foreach (var tile in tiles)
                {
                    allScore += tile.GetScore();
                    tile.Perform();
                }
            }
            else
            {
                _map.SetTile(this,pos);   
            }
        }

        public void Run()
        {
            _map.RemoveTile(this);
            isStop = false;
        }

        public void Perform()
        {
            ScoreManager.Instance.AddScore();
            Delete(timeToDelete);
        }
        
        public void Perform(float time)
        {
            ScoreManager.Instance.AddScore();
            Delete(time);
        }

        public void Dash(Vector2 direction)
        {
            if (!isStop)
            {
                Vector3 position = transform.position;
                Vector2 vector = new Vector2(position.x + direction.x, position.y + direction.y);
                Vector2Int tilePos = _map.WorldToTileCoordinate(vector);
                if (_map.PositionIsPossible(tilePos))
                {
                    if (!_map.CheckTile(tilePos))
                    {
                        _tileMovement.MoveToTheSide(this,direction);
                    }
                }
                else
                {
                    _tileMovement.MoveToTheSide(this,direction);
                    isTrash = true;
                }
            }
        }

        public void SetPositionOnTileMap(Vector2Int pos)
        {
            if (_map.PositionIsPossible(pos) && !_map.CheckTile(pos))
            {
                _map.RemoveTile(this);
                _map.SetTile(this, pos);
                this.transform.position = _map.TileToWorldCoordinate(pos);
            }
            else
            {
                Delete(0);
            }
        }

        public bool IsDown()
        {
            return isDown;
        }

        public ColorTile GetColor()
        {
            return _color;
        }

        public void Delete(float time)
        {
            isStop = true;
            _map.RemoveTile(this);
            _tileAnimation.PlayMatchAnimation();
            StartCoroutine(DeleteWithDelay(time));
            
        }

        private IEnumerator DeleteWithDelay(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(this.gameObject);
        }

        public int GetScore()
        {
            return _score;
        }

        public void DestroyFromMatch()
        {
           List<ITile> tiles = _map.GetRandomTilesInLowerLines(Random.Range(1,3));
           foreach (var tile in tiles)
           {
               _tileAnimation.PlayMatchAnimation(this._tileMovement.tileTransform.position, tile);   
           }
        }

        public void TakeHit(Transform player)
        {
            Vector2 playerPos = player.position;
            this.Dash(CalculateDirection(playerPos));
            _tileAnimation.PlayPunchedAnimation();
        }

        private Vector2 CalculateDirection(Vector2 hit)
        {
            return hit.x > _tileMovement.transform.position.x ? Vector2.left : Vector2.right;
        }
    }
}
