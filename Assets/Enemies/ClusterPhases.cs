using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Enemies
{

public class ClusterPhases : Cluster
{
    [SerializeField] Phase[] phases;
    private int currentPhase = 0;

    [System.Serializable]
    private struct Phase
    {
        public EnemyAgent[] enemies;
    }

    private void Start()
    {
        if (phases.Length == 0)
        {
            enabled = false;
            return;
        }
        foreach (var phase in phases)
        {
            foreach (var enemy in phase.enemies)
            {
                enemy.Disable();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        foreach (EnemyAgent enemy in phases[currentPhase].enemies)
        {
            if (enemy.enabled)
                return;
        }
        currentPhase += 1;
        if (currentPhase < phases.Length)
        {
            foreach (EnemyAgent enemy in phases[currentPhase].enemies)
            {
                enemy.Enable();
            }
        }
    }

    public override void ActivateAgents()
    {
        if (phases.Length > 0)
        {
            foreach (EnemyAgent agent in phases[0].enemies)
            {
                agent.Enable();
            }
        }
    }

}

}