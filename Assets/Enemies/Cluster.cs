using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Knossos.Enemies
{

public class Cluster : MonoBehaviour
{
    [SerializeField] UnityEvent startEncounterEvent;
    [SerializeField] UnityEvent endEncounterEvent;

    public delegate void EncounterEventCallback();
    public EncounterEventCallback startEncounterCallbacks;
    public EncounterEventCallback endEncounterCallbacks;

    EnemyAgent[] enemies;
    protected bool startedEncounter = false;

    private void Awake()
    {
        enemies = GetComponentsInChildren<EnemyAgent>();
        if (enemies.Length == 0)
            this.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (startedEncounter)
        {
            foreach (var bust in enemies)
            {
                if (bust.gameObject.activeSelf)
                    return;
            }
            endEncounterEvent.Invoke();
            if (endEncounterCallbacks != null)
            {
                endEncounterCallbacks();
                this.enabled = false;
            }
        }
    }

    public void StartEncounter()
    {
        if (!startedEncounter)
        {
            startEncounterEvent.Invoke();
            if (startEncounterCallbacks != null)
                startEncounterCallbacks();
            startedEncounter = true;
            ActivateAgents();
        }
    }

    public virtual void ActivateAgents()
    {
        foreach (var agent in enemies)
        {
            agent.Enable();
        }
    }
}

}