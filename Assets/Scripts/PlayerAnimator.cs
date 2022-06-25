using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    private static readonly int PunchLeft = Animator.StringToHash("PunchLeft");
    private static readonly int PunchRight = Animator.StringToHash("PunchRight");

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Jump()
    {
        _animator.SetTrigger(Jump1);
    }

    public void PlayPunchAnimation(Vector3 dir)
    {
        _animator.SetTrigger(dir == Vector3.left ? PunchLeft : PunchRight);
    }
}
