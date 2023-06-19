using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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


// public class MinotaurAgent : MonoBehaviour
// {
//     public StateMachine stateMachine;

//     // [SerializeField] public EnemyState currentState;

//     [HideInInspector]
//     public LocomotionSystem locomotionSystem;

//     void Awake()
//     {
//         locomotionSystem = GetComponent<LocomotionSystem>();
//     }

//     void Start()
//     {
//         stateMachine = new StateMachine(this.gameObject, typeof(EnemyState));

//         stateMachine.RegisterState<EnemyPatrolState>(EnemyState.Patrol);
//         stateMachine.RegisterState<EnemyAttackingState>(EnemyState.Attacking);

//         stateMachine.ChangeState(config.initialState);
//     }

//     void FixedUpdate()
//     {
//         stateMachine.FixedUpdate();
//     }

//     void Update()
//     {
//         stateMachine.Update();
//         currentState = (EnemyState)stateMachine.currentState;
//     }
// }
