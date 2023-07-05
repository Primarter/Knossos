using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knossos;

namespace Knossos.Bust
{
    public class BustAgent : MonoBehaviour
    {
        public FSM.StateMachine stateMachine;

        [SerializeField] public BustConfig config;
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

            stateMachine.RegisterState<StateIdle>(State.Idle);
            stateMachine.RegisterState<StatePatrol>(State.Patrol);
            stateMachine.RegisterState<StateChargeAttack>(State.ChargeAttack);
            stateMachine.RegisterState<StateAttacking>(State.Attacking);
            stateMachine.RegisterState<StateCooldown>(State.Cooldown);

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
