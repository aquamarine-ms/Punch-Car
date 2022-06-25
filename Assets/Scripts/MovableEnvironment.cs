using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableEnvironment : MonoBehaviour
{
    [SerializeField] private GameObject environmentPrefab;
    
    [SerializeField] private int tilesOnScreenAmount = 5;
    [SerializeField] private float tileSize = 10;
    
    [SerializeField] private Vector3 startSpawnPoint;

    [SerializeField] private float maxDistance = 10;
    
    [SerializeField] private List<GameObject> activeTiles;
    
     private float _spawnOffset;

     private Transform _playerTransform;

     [SerializeField] private Vector3 tileRotation;

    void Start()
    {
        activeTiles = new List<GameObject>();

        _playerTransform = FindObjectOfType<PlayerMovement>().playerTransform;
        
        SpawnStartTiles();
        _spawnOffset -= tileSize; //costil
    }

    public void ChangeEnvironment(GameObject go)
    {
        environmentPrefab = go;
    }

    void Update()
    {
        CheckDistanceBetweenTile();
    }

    private void CheckDistanceBetweenTile()
    {
        if (_playerTransform.position.y  > activeTiles[0].transform.position.y + maxDistance)
        {
            SpawnTile();
            DeleteTile();
        }
    }

    private void SpawnStartTiles()
    {
        for (int i = 0; i < tilesOnScreenAmount; i++)
        {
            SpawnTile();
            _spawnOffset += tileSize;
        }
    }

    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private void SpawnTile()
    {
        GameObject go = Instantiate(environmentPrefab, transform);

        Transform goTransform = go.transform;

        Vector3 position = goTransform.position;
        Vector3 newPosition = new Vector3(position.x,_spawnOffset,position.z);
        goTransform.position = newPosition;
        
        goTransform.rotation = Quaternion.Euler(tileRotation);

        activeTiles.Add(go);
        
        go.AddComponent<MoveEnvironment>();
    }


}
