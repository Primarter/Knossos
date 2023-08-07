using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace Knossos.Minotaur
{

public class StaggerSystem : MonoBehaviour
{
    MinotaurAgent agent;

    void Awake()
    {
        agent = GetComponent<MinotaurAgent>();
        if (!agent)
            this.enabled = false;
    }

    public void Stagger()
    {
        agent.stateMachine.ChangeState(State.Staggered);
    }
}

}
