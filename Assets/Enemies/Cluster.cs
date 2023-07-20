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

    Bust.BustAgent[] busts;
    bool startedEncounter = false;

    private void Awake()
    {
        busts = GetComponentsInChildren<Bust.BustAgent>();
        if (busts.Length == 0)
            this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (startedEncounter)
        {
            foreach (var bust in busts)
            {
                if (bust.gameObject.activeSelf)
                    return;
            }
            endEncounterEvent.Invoke();
            if (endEncounterCallbacks != null)
                endEncounterCallbacks();
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
        }
    }
}

}