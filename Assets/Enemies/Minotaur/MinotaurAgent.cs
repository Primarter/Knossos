using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos;

/*
Minotaur AI

Sleeping:
    do nothing
    if [trigger]
        -> Alert
        go into follow player

Alert:
    Pathfind to Sound
    if [isAtSoundSource]
        -> Patrol

Follow player:
    follow the player
    if [inRangeOfPlayer]
        attack player

Patrol:
    while [amountOfTime]
        go to intersection node
        if [atIntersection]
            find new intersection node
    go to nearest lair
*/

namespace Knossos.Minotaur
{
    public class MinotaurAgent : MonoBehaviour
    {
        public FSM.StateMachine stateMachine;

        [SerializeField] public State currentState;

        [HideInInspector]
        public LocomotionSystem locomotionSystem;
        [HideInInspector]
        public VisionSystem visionSystem;
        [HideInInspector]
        public PathSystem pathSystem;

        void Awake()
        {
            locomotionSystem = GetComponent<LocomotionSystem>();
            visionSystem = GetComponent<VisionSystem>();
            pathSystem = GetComponent<PathSystem>();
        }

        void Start()
        {
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(State));

            // stateMachine.RegisterState<StateIdle>(State.Idle);
            stateMachine.RegisterState<StatePatrol>(State.Patrol);
            stateMachine.RegisterState<StateFollow>(State.Follow);

            stateMachine.ChangeState(State.Patrol);
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
