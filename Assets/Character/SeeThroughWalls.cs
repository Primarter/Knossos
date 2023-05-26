using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughWalls : MonoBehaviour
{
    private Transform player;
    public LayerMask wallMask;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        wallMask = LayerMask.GetMask("Walls");
    }

    void FixedUpdate()
    {
        Vector3 dir = player.position - transform.position;
        RaycastHit[] hits = Physics.RaycastAll(transform.position, dir, dir.magnitude, wallMask);
        foreach (var hit in hits)
        {
            WallOpacity wallOp = hit.collider.GetComponent<WallOpacity>();
            if (wallOp != null)
            {
                wallOp.display = false;
            }
        }
    }
}
