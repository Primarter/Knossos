using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bust
{

[RequireComponent(typeof(BustAgent))]
public class StaggerSystem : MonoBehaviour
{
    BustAgent agent;

    public Enemies.OnHitEventSystem.HitInfo lastHit;

    void Awake()
    {
        agent = GetComponent<BustAgent>();
        agent.GetComponent<Enemies.OnHitEventSystem>().onHitCallbacks += OnHitCallback;
    }

    public void OnHitCallback(Enemies.OnHitEventSystem.HitInfo hit)
    {
        if (agent.currentState != State.Attacking && agent.isActiveAndEnabled)
        {
            lastHit = hit;
            agent.stateMachine.ChangeState(State.Staggered);
        }
    }
}

}