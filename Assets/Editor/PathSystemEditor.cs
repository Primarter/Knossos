using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Knossos.Minotaur;

[CustomEditor(typeof(PathSystem))]
public class PathSystemEditor : Editor
{
    public void OnSceneGUI()
    {
        var ps = (PathSystem)target;

        if (ps.waypoints == null) return;

        Waypoint waypoint = ps.getClosestWaypoint();
        Handles.DrawLine(ps.transform.position, waypoint.obj.transform.position);

        Handles.color = Color.blue;
        foreach (Waypoint w in ps.waypoints)
        {
            foreach (Waypoint n in w.neighbours)
            {
                Handles.DrawLine(w.obj.transform.position, n.obj.transform.position);
            }
        }
    }
}
