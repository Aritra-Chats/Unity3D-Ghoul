using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BlindingEnemyHidingState : BlindingEnemyAiBaseState
{
    public BlindingEnemyHidingState(BlindingEnemyAiStateMachine ctx, BlindingEnemyAiStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        CTX.Agent.isStopped = true;
        CTX.Agent.ResetPath();
        CTX.HidingTime = Random.Range(CTX.MinHidingTime, CTX.MaxHidingTime); 
        CTX.HidingTimer = Time.time + CTX.HidingTime;
        CTX.RoamingArea.GetComponent<BoxCollider>().enabled = true;
        CTX.RoamingArea.GetComponent<TriggerScript>().enabled = true;
        CTX.GetComponent<CapsuleCollider>().enabled = false;
        CTX.GetComponent<temp_health>().enabled = false;
        CTX.GetComponent<NavMeshAgent>().enabled = false;
        CTX.Mesh.enabled = false;
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState()
    {
        CTX.Mesh.enabled = true;
        CTX.GetComponent<CapsuleCollider>().enabled = true;
        CTX.GetComponent<temp_health>().enabled = true;
        CTX.GetComponent<NavMeshAgent>().enabled = true;
        if(CTX.isPlayerPresent) CTX.CanSeePlayer = true;
        CTX.RoamingArea.GetComponent<BoxCollider>().enabled = false;
        CTX.RoamingArea.GetComponent<TriggerScript>().enabled = false;
        CTX.Agent.isStopped = false;
        CTX.HidingTimer = Time.time + CTX.HidingTime;
        CTX.HidingTime = Random.Range(CTX.MinHidingTime, CTX.MaxHidingTime);
    }
    public override void CheckSwitchState()
    {
        if (CTX.isPlayerPresent) SwitchState(Factory.Pursuing());
        else if(!CTX.isPlayerPresent && Time.time > CTX.HidingTimer) SwitchState(Factory.Roaming());
    }
}
