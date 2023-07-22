using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knossos;

namespace Knossos.Bust
{
    [RequireComponent(typeof(TargetingSystem), typeof(LocomotionSystem), typeof(StaggerSystem))]
    [RequireComponent(typeof(DetectionSystem), typeof(Enemies.OnHitEventSystem), typeof(CapsuleCollider))]
    public class BustAgent : Enemies.EnemyAgent
    {
        public FSM.StateMachine stateMachine;

        [SerializeField] public BustConfig config;
        [SerializeField] public State currentState {get => (State)stateMachine.currentState;}

        // public bool isAttacking;
        // public bool canAttack;

        [HideInInspector]
        public TargetingSystem targetingSystem;
        [HideInInspector]
        public LocomotionSystem locomotionSystem;
        [HideInInspector]
        public StaggerSystem staggerSystem;
        [HideInInspector]
        public DetectionSystem detectionSystem;
        [HideInInspector]
        public Enemies.OnHitEventSystem onHitEventSystem;
        [HideInInspector]
        public CapsuleCollider capsuleCollider;

        void Awake()
        {
            targetingSystem = GetComponent<TargetingSystem>();
            locomotionSystem = GetComponent<LocomotionSystem>();
            staggerSystem = GetComponent<StaggerSystem>();
            detectionSystem = GetComponent<DetectionSystem>();
            onHitEventSystem = GetComponent<Enemies.OnHitEventSystem>();
            capsuleCollider = GetComponent<CapsuleCollider>();
        }

        void Start()
        {
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(State));

            stateMachine.RegisterState<StatePursue>(State.Pursue);
            stateMachine.RegisterState<StateIdle>(State.Idle);
            stateMachine.RegisterState<StatePatrol>(State.Patrol);
            stateMachine.RegisterState<StateChargeAttack>(State.ChargeAttack);
            stateMachine.RegisterState<StateAttacking>(State.Attacking);
            stateMachine.RegisterState<StateEndlag>(State.Endlag);
            stateMachine.RegisterState<StateCooldown>(State.Cooldown);
            stateMachine.RegisterState<StateStagered>(State.Staggered);

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

        void OnDrawGizmosSelected()
        {
            if (config != null)
                Gizmos.DrawWireSphere(transform.position, config.detectionRange);
        }

        public override void Disable()
        {
            targetingSystem.enabled = false;
            locomotionSystem.enabled = false;
            staggerSystem.enabled = false;
            detectionSystem.enabled = false;
            onHitEventSystem.enabled = false;
            this.enabled = false;
        }

        public override void Enable()
        {
            targetingSystem.hasTarget = true;
            stateMachine.ChangeState(Bust.State.Pursue);
        }
    }
}
