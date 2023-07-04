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
    -> GoHome

GoHome:
    go to nearest lair
    if [inRangeOfPlayer]
        attack player
*/

namespace Knossos.Minotaur
{
    public class MinotaurAgent : MonoBehaviour
    {
        public FSM.StateMachine stateMachine;
        [SerializeField] public State currentState;

        [HideInInspector] public PathSystem pathSystem;
        [HideInInspector] public LocomotionSystem locomotionSystem;
        [HideInInspector] public VisionSystem visionSystem;
        [HideInInspector] public SoundSensorSystem soundSensorSystem;
        [HideInInspector] public AttackSystem attackSystem;

        void Awake()
        {
            pathSystem = GetComponent<PathSystem>();
            locomotionSystem = GetComponent<LocomotionSystem>();
            visionSystem = GetComponent<VisionSystem>();
            soundSensorSystem = GetComponent<SoundSensorSystem>();
            attackSystem = GetComponent<AttackSystem>();
        }

        void Start()
        {
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(State));
            stateMachine.RegisterState<StateSleep>(State.Sleep);
            stateMachine.RegisterState<StateAlert>(State.Alert);
            stateMachine.RegisterState<StatePatrol>(State.Patrol);
            stateMachine.RegisterState<StateFollow>(State.Follow);
            stateMachine.RegisterState<StateGoHome>(State.GoHome);
            stateMachine.RegisterState<StateAttack>(State.Attack);
            stateMachine.ChangeState(State.Sleep);
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
