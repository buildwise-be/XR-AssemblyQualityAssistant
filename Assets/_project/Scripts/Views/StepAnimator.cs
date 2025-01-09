using UnityEngine;

public class StepAnimator : MonoBehaviour, IStepBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] int Step;
    public void LeaveStep()
    {
        if(_animator != null)_animator.SetBool("StopAnimBool",true);
    }

    public void EnterStepReverse()
    {
        if(_animator != null) _animator.SetBool("StopAnimBool",false);
    }

    public void Initialize(int currentStep)
    {
        gameObject.SetActive(currentStep >= Step);
    }

    public void EnterStep()
    {
        if(_animator != null) _animator.SetBool("StopAnimBool", false);
    }
}

public interface IStepBehaviour
{
    void EnterStep();
    void LeaveStep();
    void EnterStepReverse();
    void Initialize(int i);
}
