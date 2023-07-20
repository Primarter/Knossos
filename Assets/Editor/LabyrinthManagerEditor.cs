using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Knossos.Map;

[CustomEditor(typeof(LabyrinthManager))]
public class LabyrinthManagerEditor : Editor
{
    public void OnSceneGUI()
    {
        var e = (LabyrinthManager)target;

        // if (e.debug1 == null || e.debug2 == null) return;

        // Vector2 p1 = new Vector2(e.debug1.transform.position.x, e.debug1.transform.position.z) / e.mapScale;
        // Vector2 p2 = new Vector2(e.debug2.transform.position.x, e.debug2.transform.position.z) / e.mapScale;
        // foreach (Vector2 tile in e.gridTraverse(p1, p2))
        // {
        //     Vector2 v = (tile + Vector2.one*0.5f) * e.mapScale;
        //     Handles.DrawWireCube(new Vector3(v.x, 30f, v.y), Vector3.one * (e.mapScale/2f));
        // }
    }
}
