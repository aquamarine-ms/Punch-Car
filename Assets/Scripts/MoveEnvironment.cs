using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnvironment : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float step =  speed * Time.deltaTime;

        _transform.position = _transform.position + Vector3.down * step;
    }
}
