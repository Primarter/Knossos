using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Knossos.Minotaur
{
    public class Waypoint
    {
        public Waypoint(GameObject _obj)
        {
            obj = _obj;
            neighbours = null;
        }

        public GameObject obj;
        public Waypoint[] neighbours;
    }

    public class PathSystem : MonoBehaviour
    {
        Waypoint[] waypoints;
        [SerializeField] LayerMask obstructionLayer;

        void Start()
        {
            GameObject[] waypointsObjects = GameObject.FindGameObjectsWithTag("Waypoint");
            waypoints = (from waypointObject in waypointsObjects select new Waypoint(waypointObject)).ToArray();

            foreach (Waypoint waypoint in waypoints)
            {
                waypoint.neighbours = (getWaypointInSight(waypoint.obj.transform.position).Where(w => w != waypoint)).ToArray();
            }
        }

        public Waypoint[] getWaypointInSight(Vector3 position)
        {
            List<Waypoint> waypointsInSight = new List<Waypoint>();

            foreach (var w in waypoints)
            {
                Vector3 dir = w.obj.transform.position - position;
                float distance = dir.magnitude;
                dir.Normalize();

                bool hit = Physics.Raycast(position, dir, distance, obstructionLayer);

                if (hit == false) // if no wall is obstructing
                    waypointsInSight.Add(w);
            }

            return waypointsInSight.ToArray();
        }

        public Waypoint getClosestWaypoint()
        {
            Waypoint[] orderedWaypoints = waypoints.OrderBy( x => Vector3.Distance(transform.position, x.obj.transform.position) ).ToArray();

            foreach (var w in orderedWaypoints)
            {
                Vector3 dir = w.obj.transform.position - transform.position;
                float distance = dir.magnitude;
                dir.Normalize();

                bool hit = Physics.Raycast(transform.position, dir, distance, obstructionLayer);

                if (hit == false) // if no wall is obstructing
                    return w;
            }

            return null;
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
            if (waypoints == null) return;

            Waypoint waypoint = getClosestWaypoint();
            if (waypoint != null)
                Gizmos.DrawLine(transform.position, waypoint.obj.transform.position);
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position, Vector3.one * 2f);
            }

            Gizmos.color = Color.cyan;
            foreach (Waypoint w in waypoints)
            {
                foreach (Waypoint n in w.neighbours)
                {
                    Gizmos.DrawLine(w.obj.transform.position, n.obj.transform.position);
                }
            }
        }
    }
}
