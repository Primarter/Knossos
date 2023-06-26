using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Knossos;

namespace Knossos.Player
{
    public class AnimationController : MonoBehaviour
    {
        public FSM.StateMachine stateMachine;
        [SerializeField] public State currentState;

        Dictionary<State, string> stateName = new Dictionary<State, string>()
        {
            { State.Movement, "Movement" },
            { State.Dodge, "Dodge" }
        };

        [HideInInspector]
        public Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            stateMachine = new FSM.StateMachine(this.gameObject, typeof(State));

            stateMachine.RegisterState<StateMovement>(State.Movement);
            stateMachine.RegisterState<StateDodge>(State.Dodge);

            stateMachine.ChangeState(State.Movement);
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

        public void TransitionTo(State state, float fadeTime)
        {
            if ((State)stateMachine.currentState != state)
            {
                stateMachine.ChangeState(state);
                animator.CrossFade(stateName[state], fadeTime, 0);
            }
        }

        // public State getAnimationState()
        // {
        // }
    }
}
