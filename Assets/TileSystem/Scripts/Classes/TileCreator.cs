using System;
using System.Collections;
using System.Collections.Generic;
using TileSystem;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileCreator : MonoBehaviour
{
    public static int maxRows;
    public static int minRows = 0;

    public static int freeMoveRowCount = 1;

    [SerializeField] private Vector2Int size;
    [SerializeField] private int countTopTilesGenerate;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private float duration;
    [SerializeField] private List<GameObject> prefabTiles;
    [SerializeField] private int endGameRow;

    private Transform _root;

    private ITileMap _tileMap;

    private bool _isLooping;

    private float time;
    public int RowsCount => size.x;

    void Start()
    {
        _root = new GameObject("Tiles").transform;
        
        _tileMap = new TileMap(startPos,size, endGameRow, countTopTilesGenerate);
        Generate();

        maxRows = size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isLooping)
            Loop();
    }

    public void StartLooping()
    {
        _isLooping = true;
    }

    private void Generate()
    {
        for (int y = 0; y < countTopTilesGenerate; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                Transform tileTransform = Instantiate(prefabTiles[Random.Range(0, prefabTiles.Count)], startPos,
                    Quaternion.identity).transform;
            
                tileTransform.SetParent(_root);
            
                ITile tile = tileTransform.GetComponent<ITile>();
                
                tile.SetTileMap(_tileMap);
                tile.SetPositionOnTileMap(new Vector2Int(x,y));
            }
        }
    }

    public void SetCountTopTiles(int count)
    {
        countTopTilesGenerate = count;
        size.x = count;
    }

    private void Loop()
    {
        if (time + duration < Time.time && !_tileMap.IsEnd())
        {
            List<Vector2Int> vectors = new List<Vector2Int>();
            for (int x = 0; x < size.x; x++)
            {
                Vector2Int check = new Vector2Int(x, countTopTilesGenerate - 1);
                if (_tileMap.CheckTile(check))
                {
                    vectors.Add(check);
                }
            }
            
            if (vectors.Count == 0)
            {
                DisplaceTiles();
                GenerateTilesLine(0);
            }
            else
            {
                int num = Random.Range(0, vectors.Count);
                ITile tile = _tileMap.GetTile(vectors[num]);
                tile.Run();
            }

            time = Time.time;
        }
    }

    private void DisplaceTiles()
    {
        for (int y = countTopTilesGenerate - 2; y >= 0; y--)
        {
            for (int x = 0; x < size.x; x++)
            {
                Vector2Int vector = new Vector2Int(x, y);
                ITile tile = _tileMap.GetTile(vector);
                if (tile != null)
                {
                    tile.SetPositionOnTileMap(vector + Vector2Int.up);
                }
            }
        }
    }

    private void GenerateTilesLine(int line)
    {
        for (int x = 0; x < size.x; x++)
        {
            Transform tileTransform = Instantiate(prefabTiles[Random.Range(0, prefabTiles.Count)], startPos,
                Quaternion.identity).transform;
            
            tileTransform.SetParent(_root);
            
            ITile tile = tileTransform.GetComponent<ITile>();
            
            tile.SetTileMap(_tileMap);
            tile.SetPositionOnTileMap(new Vector2Int(x,line));
        }
    }
    
    private void OnDrawGizmos()
    {
        Vector3 right = startPos + Vector3.right * size.x;
        Vector3 offset = Vector3.left / 2 + Vector3.up / 2;
        for (int y = 0; y <= size.y; y++)
        {
            Gizmos.DrawLine(startPos + Vector3.down * y + offset, right + Vector3.down * y + offset);
        }

        Vector3 down = startPos + Vector3.down * size.y;
        for (int x = 0; x <= size.x; x++)
        {
            Gizmos.DrawLine(startPos + Vector3.right * x + offset, down + Vector3.right * x + offset);
        }
        
        if(countTopTilesGenerate < 0) return;
        if (countTopTilesGenerate < size.y && countTopTilesGenerate >= 1)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        
        right = startPos + Vector3.right * size.x;
        offset = Vector3.left / 2 + Vector3.up / 2;
        for (int y = 0; y <= countTopTilesGenerate; y++)
        {
            Gizmos.DrawLine(startPos + Vector3.down * y + offset, right + Vector3.down * y + offset);
        }

        down = startPos + Vector3.down * countTopTilesGenerate;
        for (int x = 0; x <= size.x; x++)
        {
            Gizmos.DrawLine(startPos + Vector3.right * x + offset, down + Vector3.right * x + offset);
        }
        
        Gizmos.color = Color.yellow;
        
        right = startPos + Vector3.right * size.x;
        offset = Vector3.left / 2 + Vector3.up / 2;
        for (int y = size.y - endGameRow + 1; y > size.y - endGameRow - 1; y--)
        {
            Gizmos.DrawLine(startPos + Vector3.down * y + offset, right + Vector3.down * y + offset);
        }

        down =new Vector3(startPos.x, startPos.y - size.y + endGameRow, startPos.z);
        for (int x = 0; x <= size.x; x++)
        {
            Gizmos.DrawLine(new Vector3(startPos.x, startPos.y - size.y + endGameRow - 1, startPos.z) + Vector3.right * x + offset, down + Vector3.right * x + offset);
        }

    }
}
