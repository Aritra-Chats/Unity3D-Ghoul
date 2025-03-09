using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindingEnemyPursuingState : BlindingEnemyAiBaseState
{
    public BlindingEnemyPursuingState(BlindingEnemyAiStateMachine ctx, BlindingEnemyAiStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        CTX.Agent.speed = CTX.PursueSpeed;
        CTX.Agent.stoppingDistance = 1.5f;
        CTX.SearchRadius = Mathf.Infinity;
        CTX.SearchAngle = 360f;
        CTX.StartCoroutine(CTX.SearchForPlayer());
    }
    public override void UpdateState()
    {
        CTX.Agent.SetDestination(CTX.PlayerPref.transform.position);
        CTX.transform.LookAt(CTX.PlayerPref.transform.position);
        CheckSwitchState();
    }
    public override void ExitState()
    {
    }
    public override void CheckSwitchState()
    {
        if (!CTX.CanSeePlayer) SwitchState(Factory.Roaming());
        if(CTX.Agent.remainingDistance <= CTX.Agent.stoppingDistance && CTX.CanSeePlayer) SwitchState(Factory.Attacking());
    }
}
