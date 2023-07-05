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
        [SerializeField] public BustState currentState {get => (BustState)stateMachine.currentState;}

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
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(BustState));

            stateMachine.RegisterState<StateIdle>(BustState.Idle);
            stateMachine.RegisterState<StatePatrol>(BustState.Patrol);
            stateMachine.RegisterState<StateChargeAttack>(BustState.ChargeAttack);
            stateMachine.RegisterState<StateAttacking>(BustState.Attacking);
            stateMachine.RegisterState<StateCooldown>(BustState.Cooldown);
            stateMachine.RegisterState<StateStagered>(BustState.Staggered);

            stateMachine.ChangeState(config.initialState);
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        void Update()
        {
            stateMachine.Update();
        }
    }
}
