using System.Collections;
using System.Collections.Generic;
using TileSystem;
using UnityEngine;

public class TileMovement : MonoBehaviour
{
    public Transform tileTransform;

    [SerializeField] private float sideMoveDistance = 1;
    [SerializeField] private float sideMoveSpeed = 5;

    [SerializeField] private float moveDownSpeed = 5;

    public bool isDropped = true; //false

    private bool _canMove;


    void Start()
    {
        tileTransform = GetComponent<Transform>();
        
        _canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        if (!isDropped)
            return;
        
        float step =  moveDownSpeed * Time.deltaTime;

        tileTransform.position = tileTransform.position + Vector3.down * step;
    }

    public void StartMoving()
    {
        isDropped = true;
    }

    public void StopMoving()
    {
        isDropped = false;
    }

    public void MoveToTheSide(ITile tile, Vector3 direction)
    {
        if (!_canMove)
            return;

        //_canMove = false;
        
        //direction += Vector3.down * 0.5f; //remove
        StartCoroutine(MoveToSideSmooth(tile,direction));
        //tileTransform.position = tileTransform.position + direction * sideMoveDistance;
        
        /*
        iTween.MoveTo(this.gameObject, iTween.Hash("position", position, "speed", sideMoveSpeed,
            "oncomletetarget",this.gameObject, "oncomplete", "MoveComplete","easetype", iTween.EaseType.easeInSine));
            */
    }

    private IEnumerator MoveToSideSmooth(ITile tile, Vector3 direction)
    {
        isDropped = false;
        tile.SetIsStop(true);
        Vector3 toPosition = tileTransform.position + direction * sideMoveDistance;
        while (Vector3.Distance(tileTransform.position,toPosition) > 0)
        {
            tileTransform.position =
                Vector3.MoveTowards(tileTransform.position, toPosition, Time.deltaTime * sideMoveSpeed);
            yield return new WaitForSeconds(0.01f);
        }

        tileTransform.position = toPosition;
        isDropped = true;
        tile.SetIsStop(false);
    }

    private void MoveComplete()
    {
        _canMove = true;
    }
}
