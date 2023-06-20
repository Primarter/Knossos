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

// public class MinotaurAI : MonoBehaviour
// {
//     NavMeshAgent agent;
//     [SerializeField] GameObject player;

//     void Awake()
//     {
//         agent = GetComponent<NavMeshAgent>();
//     }

//     void Update()
//     {
//         if (player.GetComponent<Hiding>().isHidding)
//         {
//             agent.isStopped = true;
//         }
//         else
//         {
//             agent.isStopped = false;
//             agent.destination = player.transform.position;
//         }
//     }
// }

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

            // stateMachine.RegisterState<EnemyStatePatrol>(State.Patrol);
            // stateMachine.RegisterState<EnemyStateAttacking>(State.Follow);

            // stateMachine.ChangeState(config.initialState);
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
