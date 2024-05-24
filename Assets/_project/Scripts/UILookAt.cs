using MixedReality.Toolkit.SpatialManipulation;
using UnityEngine;
/// <summary>
/// InFront solver positions an object 2m in front of the tracked transform target
/// </summary>
public class UILookAt : Solver
{
    public override void SolverUpdate()
    {
        if (SolverHandler != null && SolverHandler.TransformTarget != null)
        {
            var target = SolverHandler.TransformTarget;
            GoalRotation = Quaternion.LookRotation(target.position - transform.position);
        }
    }
}