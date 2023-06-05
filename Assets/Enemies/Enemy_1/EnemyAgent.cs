using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    public StateMachine stateMachine;

    [SerializeField] public EnemyConfig config;
    [SerializeField] public EnemyState currentState;

    // public bool isAttacking;
    // public bool canAttack;

    [HideInInspector]
    public TargetingSystem targetingSystem;
    [HideInInspector]
    public LocomotionSystem locomotionSystem;

    void Awake()
    {
        targetingSystem = GetComponent<TargetingSystem>();
        locomotionSystem = GetComponent<LocomotionSystem>();
    }

    void Start()
    {
        stateMachine = new StateMachine(this.gameObject, typeof(EnemyState));

        stateMachine.RegisterState<EnemyIdleState>(EnemyState.Idle);
        stateMachine.RegisterState<EnemyPatrolState>(EnemyState.Patrol);
        stateMachine.RegisterState<EnemyChargeAttackState>(EnemyState.ChargeAttack);
        stateMachine.RegisterState<EnemyAttackingState>(EnemyState.Attacking);
        stateMachine.RegisterState<EnemyStagerredState>(EnemyState.Stagerred);

        stateMachine.ChangeState(config.initialState);
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    void Update()
    {
        stateMachine.Update();
        currentState = (EnemyState)stateMachine.currentState;
    }
}
