using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Knossos.FSM;

namespace Knossos.Player
{
    public class StateDodge : FSM.State
    {
        AnimationController controller;

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
        }
}
}