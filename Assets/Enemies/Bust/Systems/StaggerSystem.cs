using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{

[RequireComponent(typeof(BustAgent))]
public class StaggerSystem : MonoBehaviour
{
    BustAgent agent;

    void Awake()
    {
        agent = GetComponent<BustAgent>();
        agent.GetComponent<Enemies.OnHitEventSystem>().onHitCallbacks += OnHitCallback;
    }

    public void OnHitCallback(int damage)
    {
        if (agent.currentState != State.Attacking && agent.isActiveAndEnabled)
        {
            agent.stateMachine.ChangeState(State.Staggered);
        }
    }
}

}