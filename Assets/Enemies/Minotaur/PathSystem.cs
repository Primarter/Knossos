using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Knossos.Minotaur
{
    [Serializable]
    public struct Link
    {
        public GameObject waypointA;
        public GameObject waypointB;
    }

    public class PathSystem : MonoBehaviour
    {
        [SerializeField] GameObject[] waypoints;
        [SerializeField] Link[] links;
        [SerializeField] LayerMask obstructionLayer;

        public GameObject getClosestWaypoint()
        {
            GameObject[] orderedWaypoints = waypoints.OrderBy( x => Vector3.Distance(transform.position, x.transform.position) ).ToArray();

            foreach (var w in orderedWaypoints)
            {
                Vector3 dir = w.transform.position - transform.position;
                float distance = dir.magnitude;
                dir.Normalize();

                bool hit = Physics.Raycast(transform.position, dir, distance, obstructionLayer);

                if (hit == false) // if no wall is obstructing
                    return w;
            }

            return null;
        }

        public GameObject[] getLinkedWaypoints(GameObject waypoint)
        {
            List<GameObject> linkedWaypoints = new List<GameObject>();

            foreach (Link link in links)
            {
                if (link.waypointA == waypoint)
                    linkedWaypoints.Add(link.waypointB);
                else if (link.waypointB == waypoint)
                    linkedWaypoints.Add(link.waypointA);
            }

            return linkedWaypoints.ToArray();
        }

        void OnDrawGizmos()
        {
            // if (waypoints != null)
            // {
            //     Gizmos.color = Color.magenta;
            //     foreach (GameObject waypoint in waypoints)
            //     {
            //         Gizmos.DrawSphere(waypoint.transform.position, 2f);
            //     }
            // }
            GameObject obj = getClosestWaypoint();
            if (obj != null)
                Gizmos.DrawLine(transform.position, obj.transform.position);
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position, Vector3.one * 2f);
            }

            if (links != null)
            {
                Gizmos.color = Color.cyan;
                foreach (Link link in links)
                {
                    Gizmos.DrawLine(link.waypointA.transform.position, link.waypointB.transform.position);
                }
            }
        }
    }
}
