using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos;

/*
Minotaur AI

sleeping:
    not moving
follow player:
    follow the player
patrol:
    go until intersection
*/


namespace Knossos.Minotaur
{
    public class MinotaurAgent : MonoBehaviour
    {
        public FSM.StateMachine stateMachine;

        [SerializeField] public State currentState;

        [HideInInspector]
        public LocomotionSystem locomotionSystem;

        void Awake()
        {
            locomotionSystem = GetComponent<LocomotionSystem>();
        }

        void Start()
        {
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(State));

            stateMachine.RegisterState<StatePatrol>(State.Patrol);
            stateMachine.RegisterState<StateFollow>(State.Follow);

            stateMachine.ChangeState(State.Follow);
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
