using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

namespace Knossos.Environment
{

[RequireComponent(typeof(VisualEffect), typeof(BoxCollider))]
public class FireWall : MonoBehaviour
{
    VisualEffect fire;
    BoxCollider boxCollider;
    NavMeshObstacle navMeshObstacle;

    private void Awake()
    {
        fire = GetComponent<VisualEffect>();
        boxCollider = GetComponent<BoxCollider>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();

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
        navMeshObstacle.enabled = false;
    }

    public void TurnOnWall()
    {
        fire.Play();
        boxCollider.enabled = true;
        navMeshObstacle.enabled = true;
    }

    public void TurnOffWall()
    {
        fire.Stop();
        boxCollider.enabled = false;
        navMeshObstacle.enabled = false;
    }
}

}