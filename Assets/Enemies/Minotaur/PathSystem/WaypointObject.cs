using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{
    public class WaypointObject : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawCube(transform.position, Vector3.one);
        }
    }
}
