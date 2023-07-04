using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knossos;

namespace Knossos.Enemy1
{
    public class EnemyAgent : MonoBehaviour
    {
        public FSM.StateMachine stateMachine;

        [SerializeField] public EnemyConfig config;
        [SerializeField] public State currentState;

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
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(State));

            stateMachine.RegisterState<EnemyStateIdle>(State.Idle);
            stateMachine.RegisterState<EnemyStatePatrol>(State.Patrol);
            stateMachine.RegisterState<EnemyStateChargeAttack>(State.ChargeAttack);
            stateMachine.RegisterState<EnemyStateAttacking>(State.Attacking);
            stateMachine.RegisterState<EnemyStateStagerred>(State.Stagerred);

            stateMachine.ChangeState(config.initialState);
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void Update()
        {
            stateMachine.Update();
            currentState = (State)stateMachine.currentState;
        }
    }
}
