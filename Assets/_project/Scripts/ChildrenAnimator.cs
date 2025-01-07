using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class ChildrenAnimator : MonoBehaviour
{
    private Animator[] _childrenAnimators;

    private float _animationDelay;
    public float AnimationDelay { set => _animationDelay = value; }

    private void Start()
    {
        _childrenAnimators = GetComponentsInChildren<Animator>();
    }

    public void PlayNextWithDelay()
    {
        StartCoroutine(PlayNextWithDelayCoroutine(_animationDelay));
    }

    private IEnumerator PlayNextWithDelayCoroutine(float delay)
    {
        foreach (var animator in _childrenAnimators)
        {
            StepOrder[] stepOrders = animator.GetComponentsInChildren<StepOrder>();
            stepOrders.OrderBy(stepOrder => stepOrder.Order);

            foreach (var step in stepOrders)
            {
                Animator anim = step.GetComponent<Animator>();
                //anim.SetTrigger(trigger);

                AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
                if (info.IsName("Initial State"))
                {
                    anim.SetTrigger("StartRemove");
                }
                else if (info.IsName("Removing"))
                {
                    anim.SetTrigger("StopRemove");
                    anim.ResetTrigger("StartRemove");
                }
                else if (info.IsName("Removed"))
                {
                    anim.SetTrigger("StartReplace");
                    anim.ResetTrigger("StopRemove");
                }
                else if (info.IsName("Replacing"))
                {
                    anim.SetTrigger("StopReplace");
                    anim.ResetTrigger("StartReplace");
                }
                else if (info.IsName("Not Placed"))
                {
                    anim.SetTrigger("StartPlacingNew");
                }
                else if (info.IsName("Placing New"))
                {
                    anim.SetTrigger("StopPlacingNew");
                    anim.ResetTrigger("StartPlacingNew");
                }
                else if (info.IsName("Placed"))
                {
                    anim.SetTrigger("StartOpening");
                    anim.ResetTrigger("StopPlacingNew");
                }
                else if (info.IsName("Opening"))
                {
                    anim.SetTrigger("StopOpening");
                    anim.ResetTrigger("StartOpening");
                }
                else if (info.IsName("Open"))
                {
                    anim.SetTrigger("Hide");
                    anim.ResetTrigger("StopOpening");
                }
                else if (info.IsName("Hidden"))
                {
                    anim.SetTrigger("Show");
                    anim.ResetTrigger("Hide");
                }
                yield return new WaitForSeconds(delay);
            }
        }
    }

    public void PlayNextWithoutDelay()
    {
        foreach (var anim in _childrenAnimators)
        {
            AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Initial State"))
            {
                anim.SetTrigger("StartRemove");
            }
            else if (info.IsName("Removing"))
            {
                anim.SetTrigger("StopRemove");
                anim.ResetTrigger("StartRemove");
            }
            else if (info.IsName("Removed"))
            {
                anim.SetTrigger("StartReplace");
                anim.ResetTrigger("StopRemove");
            }
            else if (info.IsName("Replacing"))
            {
                anim.SetTrigger("StopReplace");
                anim.ResetTrigger("StartReplace");
            }
            else if (info.IsName("Not Placed"))
            {
                anim.SetTrigger("StartPlacingNew");
            }
            else if (info.IsName("Placing New"))
            {
                anim.SetTrigger("StopPlacingNew");
                anim.ResetTrigger("StartPlacingNew");
            }
            else if (info.IsName("Placed"))
            {
                anim.SetTrigger("StartOpening");
                anim.ResetTrigger("StopPlacingNew");
            }
            else if (info.IsName("Opening"))
            {
                anim.SetTrigger("StopOpening");
                anim.ResetTrigger("StartOpening");
            }
            else if (info.IsName("Open"))
            {
                anim.SetTrigger("Hide");
                anim.ResetTrigger("StopOpening");
            }
            else if (info.IsName("Hidden"))
            {
                anim.SetTrigger("Show");
                anim.ResetTrigger("Hide");
            }
        }
    }

    public void StopAllWithoutDelay()
    {
        foreach (var animator in _childrenAnimators)
        {
            animator.SetTrigger("StopRemove");
            animator.SetTrigger("StopReplace");
        }
    }
}