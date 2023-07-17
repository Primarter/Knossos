using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Map
{
    [System.Serializable]
    public struct Cell
    {
        public int type;
        public GameObject obj;
    }

    // TODO: create struct map to hold width and height with it

    [RequireComponent(typeof(Grid))]
    public class LabyrinthManager : MonoBehaviour
    {
        [SerializeField] public Cell[] map;
        [SerializeField] public int mapHeight;
        [SerializeField] public int mapWidth;
        [SerializeField] public Vector3 origin;
    }

}
