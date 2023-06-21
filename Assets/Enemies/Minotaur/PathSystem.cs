using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

        public GameObject getClosestWaypoint()
        {
            float closestSqrDist = Mathf.Infinity;
            GameObject closestWaypoint = null;

            foreach (var w in waypoints)
            {
                Vector3 offset = w.transform.position - transform.position
                float sqrDist = offset.sqrMagnitude;

                if (sqrDist < closestSqrDist)
                {
                    closestSqrDist = sqrDist;
                    closestWaypoint = w;
                }
            }

            return closestWaypoint;
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
