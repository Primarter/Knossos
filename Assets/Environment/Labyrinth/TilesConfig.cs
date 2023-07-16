using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Map
{
    [System.Serializable]
    public struct Tile
    {
        public int id;

        public GameObject prefab;
        public bool canRotate;

        public bool hasConditionN;
        public int conditionN;

        public bool hasConditionE;
        public int conditionE;

        public bool hasConditionS;
        public int conditionS;

        public bool hasConditionW;
        public int conditionW;
    }

    [CreateAssetMenu()]
    public class TilesConfig : ScriptableObject
    {
        public List<Tile> tiles;
        public float scale;
    }
}