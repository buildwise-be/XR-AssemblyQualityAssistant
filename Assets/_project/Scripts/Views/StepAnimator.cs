using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepAnimator : MonoBehaviour, IStepAnimation
{
    [SerializeField] private Animator _animator;
    
    public int Step;
    public void StopAnimation()
    {
        _animator.SetBool("StopAnimBool",true);
    }

    public void PlayAnimationReverse()
    {
        _animator.SetBool("StopAnimBool",false);
    }

    public void PlayAnimation()
    {
        _animator.SetBool("StopAnimBool",false);
    }
}

public interface IStepAnimation
{
    void PlayAnimation();
    void StopAnimation();
    void PlayAnimationReverse();
}