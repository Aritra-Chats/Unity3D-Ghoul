using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.AI;

public class BlindingEnemyRoamingState : BlindingEnemyAiBaseState
{
    public BlindingEnemyRoamingState(BlindingEnemyAiStateMachine ctx, BlindingEnemyAiStateFactory factory) : base(ctx, factory) { }

    public override void EnterState()
    {
        CTX.SearchAngle = CTX.Temp_SearchAngle;
        CTX.RoamingTime = Random.Range(CTX.MinWaitTimeBeforeHiding, CTX.MaxWaitTimeBeforeHiding);
        CTX.RoamingTimer = Time.time + CTX.RoamingTime;
        CTX.PlayerPref = GameObject.FindGameObjectWithTag("Player");
        CTX.SearchRadius = CTX.Temp_SearchRadius;
        CTX.Agent.stoppingDistance = 0f;
        CTX.StartCoroutine(CTX.SearchForPlayer());
    }
    public override void UpdateState()
    {
        Roaming();
        CheckSwitchState();
    }
    public override void ExitState()
    {
        CTX.RoamingTime = Random.Range(CTX.MinWaitTimeBeforeHiding, CTX.MaxWaitTimeBeforeHiding);
        CTX.RoamingTimer = Time.time + CTX.RoamingTime;
    }
    public override void CheckSwitchState()
    {
        if (CTX.CanSeePlayer) SwitchState(Factory.Pursuing());
        else if(!CTX.CanSeePlayer && Time.time > CTX.RoamingTimer) SwitchState(Factory.Hiding());
    }
    void Roaming()
    {
        if(CTX.Agent.remainingDistance <= CTX.Agent.stoppingDistance)
        {
            if (Time.time < CTX.WaitingTime) CTX.Agent.speed = 0f;
            else
            {
                CTX.Agent.speed = CTX.RoamingSpeed;
                Vector3 point;
                if (RandomPoint(CTX.RoamingArea.transform.position, CTX.RoamRadius, out point))
                {
                    CTX.Agent.SetDestination(point);
                    CTX.transform.LookAt(point);
                }
                CTX.WaitingTime = Time.time + CTX.WaitTime;
                CTX.WaitTime = Random.Range(1f, 3f);
            }
        }
    }

    bool RandomPoint(Vector3 center, float radius, out Vector3 point)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * radius;
        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPoint, out hit, CTX.DistanceBetweenPositions, NavMesh.AllAreas))
        {
            point = hit.position;
            return true;
        }

        point = Vector3.zero;
        return false;
    }
}
