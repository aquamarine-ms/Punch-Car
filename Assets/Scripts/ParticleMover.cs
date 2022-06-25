using System.Collections;
using System.Collections.Generic;
using TileSystem;
using UnityEngine;

public class ParticleMover : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField] private GameObject endEffect;
    private ParticleSystem _particle;
    private ITile _tile;

    private bool isStart = false;

    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart && startTime < Time.time - _particle.main.duration)
        {
            if (!_tile.Equals(null))
            {
                Vector3 targetPosition = _tile.GetPosition();
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

                if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
                {
                    _tile.Perform(0);
                    BreakEffect();
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void BreakEffect()
    {
        GameObject go = Instantiate(endEffect, this.transform.position, Quaternion.identity);
        Destroy(go, 1f);
    }
    public void MoveTo(ITile tile)
    {
        _particle = this.GetComponent<ParticleSystem>();
        _tile = tile;
        isStart = true;
        startTime = Time.time;
    }
}
