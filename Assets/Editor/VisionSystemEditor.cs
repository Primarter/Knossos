using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Knossos.Minotaur;

[CustomEditor(typeof(VisionSystem))]
public class VisionSystemEditor : Editor
{
    public void OnSceneGUI()
    {
        var vs = (VisionSystem)target;

        Handles.color = Color.white;
        Handles.DrawWireDisc(vs.transform.position, Vector3.up, vs.viewRange);
        Handles.DrawWireDisc(vs.transform.position, Vector3.up, vs.minDetectionRange);

        Vector3 forward2D = new Vector3(vs.transform.forward.x, 0f, vs.transform.forward.z);
        Vector3 viewAngleL = Quaternion.Euler(0f, -vs.FOV / 2f, 0f) * forward2D;
        Vector3 viewAngleR = Quaternion.Euler(0f, vs.FOV / 2f, 0f) * forward2D;

        Handles.DrawLine(vs.transform.position, vs.transform.position + viewAngleL.normalized * vs.viewRange);
        Handles.DrawLine(vs.transform.position, vs.transform.position + viewAngleR.normalized * vs.viewRange);
    }
}
