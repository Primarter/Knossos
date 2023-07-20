using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Knossos.Environment
{

[RequireComponent(typeof(VisualEffect), typeof(BoxCollider))]
public class FireWall : MonoBehaviour
{
    VisualEffect fire;
    BoxCollider boxCollider;

    private void Awake()
    {
        fire = GetComponent<VisualEffect>();
        boxCollider = GetComponent<BoxCollider>();
        if (transform.parent != null && transform.parent.tag == "Cluster")
        {
            var cluster = transform.parent.GetComponent<Enemies.Cluster>();
            if (cluster != null)
            {
                cluster.startEncounterCallbacks += TurnOnWall;
                cluster.endEncounterCallbacks += TurnOffWall;
            }
        }
    }

    private void Start()
    {
        fire.Stop();
        boxCollider.enabled = false;
    }

    public void TurnOnWall()
    {
        fire.Play();
        boxCollider.enabled = true;
    }

    public void TurnOffWall()
    {
        fire.Stop();
        boxCollider.enabled = false;
    }
}

}