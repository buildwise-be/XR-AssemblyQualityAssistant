using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public int Step;
    public void StopAnimation()
    {
        _animator.SetBool("StopAnimBool",true);
    }
    public void PlayAnimation()
    {
        _animator.SetBool("StopAnimBool",false);
    }
}
