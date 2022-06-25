using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerRaycaster : MonoBehaviour
{
    private PlayerAnimator _playerAnimator;
    private Transform _playerTransform;

    [SerializeField] private float maxDistance = 100f;

    public float viewRadius;
    [Range(0, 360)] public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    public float forwardRayDistance = 3f;

    private void Start()
    {
        _playerAnimator = GetComponent<PlayerAnimator>();
        _playerTransform = transform;
    }

    public bool Hit(Vector3 direction)
    {
        IHittable hit = CheckForTile(direction);

        if (hit == null)
            return false;
        
        hit.TakeHit(_playerTransform); 
        _playerAnimator.PlayPunchAnimation(direction);
        
        return true;
    }

    private IHittable CheckForTile(Vector3 direction)
    {
        IHittable hittable = null;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(_playerTransform.position, viewRadius, targetMask);

        foreach (var t in targetsInViewRadius)
        {
            Transform target = t.transform;
            Vector3 dirToTarget = (target.position - _playerTransform.position).normalized;
            if (Vector3.Angle(direction, dirToTarget) < viewAngle / 2)
            {
                hittable = target.GetComponent<IHittable>();
            }
        }

        return hittable;
    }

    public bool CheckForward()
    {
        return Physics.Raycast(_playerTransform.position, _playerTransform.TransformDirection(Vector3.up), forwardRayDistance);
    }

    public Vector3 DirFromAngle(float angleInDegrees , bool angleIsGlobal)
    {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees  * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
