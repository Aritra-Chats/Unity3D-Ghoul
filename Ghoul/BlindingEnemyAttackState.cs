using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindingEnemyAttackState : BlindingEnemyAiBaseState
{
    public BlindingEnemyAttackState(BlindingEnemyAiStateMachine ctx, BlindingEnemyAiStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        CTX.Animator.Play("Attack",0,0);
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        CTX.Agent.ResetPath();
    }
    public override void CheckSwitchState()
    {
        if (CTX.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.75f && CTX.Animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")) 
        {
            if (CTX.CanSeePlayer) SwitchState(Factory.Pursuing());
            else if (!CTX.CanSeePlayer) SwitchState(Factory.Roaming()); 
        }
    }
}
