using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knossos.FSM;

namespace Knossos.Player
{
    public class StateMovement : FSM.State
    {
        AnimationController controller;

        Vector3 smoothInput = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        public override void Init()
        {
            controller = obj.GetComponent<AnimationController>();
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void FixedUpdate()
        {
        }

        public override void Update()
        {
            Vector3 movement = new Vector3(InputManager.inputs.horizontal, 0f, InputManager.inputs.vertical);
            smoothInput = Vector3.SmoothDamp(smoothInput, movement, ref velocity, 5 * Time.deltaTime);
            movement = Vector3.ClampMagnitude(smoothInput, 1);

            controller.animator.SetFloat("Speed", movement.magnitude);

            // #if UNITY_EDITOR
            //     animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
            // #endif
        }
}
}