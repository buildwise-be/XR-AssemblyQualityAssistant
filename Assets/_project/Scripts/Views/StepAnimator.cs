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

    public void Setup(int currentStep)
    {
        gameObject.SetActive(currentStep <= Step);
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
    void Setup(int i);
}
