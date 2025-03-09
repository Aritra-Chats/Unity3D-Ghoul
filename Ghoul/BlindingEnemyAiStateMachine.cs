using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlindingEnemyAiStateMachine : MonoBehaviour
{
    #region Variables
    [Header("Player Detection")]
    public LayerMask PlayerMask;
    public LayerMask EnvironmentMask;

    public float SearchRadius;
    [Range(0f, 360f)] public float SearchAngle;
    public float PursueSpeed;

    [Header("Random Roaming")]
    public GameObject RoamingArea;

    public float RoamRadius;
    public float DistanceBetweenPositions;
    public float RoamingSpeed;

    [Header("Hiding")]
    public SkinnedMeshRenderer Mesh;
    [Range(10f, 30f)] public float MinWaitTimeBeforeHiding;
    [Range(50f, 100f)] public float MaxWaitTimeBeforeHiding;
    [Range(10f, 40f)] public float MinHidingTime;
    [Range(50f, 200f)] public float MaxHidingTime;

    GameObject _playerRef;
    NavMeshAgent _agent;
    Animator _animator;

    BlindingEnemyAiBaseState _currentState;
    BlindingEnemyAiStateFactory _states;

    float _temp_SearchRadius;
    float _temp_SearchAngle;
    float _waitingTime;
    float _waitTime;
    float _roamingTime;
    float _roamingTimer;
    float _hidingTime;
    float _hidingTimer;

    bool _canSeePlayer;
    bool _isPlayerPresent;

    #endregion

    #region Getters & Setters
    public GameObject PlayerPref {  get { return _playerRef; } set { _playerRef = value; } }
    public BlindingEnemyAiBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public BlindingEnemyAiStateFactory Factory { get { return _states; } set { _states = value; } }
    public NavMeshAgent Agent { get { return _agent; } set { _agent = value; } }
    public Animator Animator { get { return _animator; } set { _animator = value; } }
    public float WaitingTime { get { return _waitingTime; } set { _waitingTime = value; } }
    public float RoamingTime { get { return _roamingTime; } set { _roamingTime = value; } }
    public float RoamingTimer { get { return _roamingTimer; } set { _roamingTimer = value; } }
    public float HidingTime { get { return _hidingTime; } set { _hidingTime = value; } }
    public float HidingTimer { get { return _hidingTimer; } set { _hidingTimer = value; } }
    public float WaitTime { get { return _waitTime; } set { _waitTime = value; } }
    public float Temp_SearchRadius { get { return _temp_SearchRadius; } }
    public float Temp_SearchAngle { get { return _temp_SearchAngle; } }
    public bool CanSeePlayer { get { return _canSeePlayer; } set { _canSeePlayer = value; } }
    public bool isPlayerPresent { get { return _isPlayerPresent; } }
    #endregion

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _temp_SearchRadius = SearchRadius;
        _temp_SearchAngle = SearchAngle;
        RoamingArea.GetComponent<BoxCollider>().enabled = false;
        RoamingArea.GetComponent<TriggerScript>().enabled = false;

        _animator = GetComponent<Animator>();

        _states = new BlindingEnemyAiStateFactory(this);
        _currentState = _states.Roaming();
        _currentState.EnterState();
        _waitingTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _isPlayerPresent = RoamingArea.GetComponent<TriggerScript>().IsPlayerPresent;
        _currentState.UpdateState();
        SynchronizeAnimatorAndAgent();
    }

    void SynchronizeAnimatorAndAgent()
    {
        _animator.SetBool("isMoving", _agent.velocity.magnitude > 0.1f && _agent.remainingDistance > _agent.stoppingDistance);
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    public IEnumerator SearchForPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfViewCheck();
        }
    }

    void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, SearchRadius, PlayerMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < SearchAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, EnvironmentMask)) _canSeePlayer = true;
                else _canSeePlayer = false;
            }
            else _canSeePlayer = false;
        }
        else if (_canSeePlayer) _canSeePlayer = false;
    }
}
