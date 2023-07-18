using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

        Grid grid;
        Transform playerTransform;

        List<Cell> visibleCells = new();

        void Awake()
        {
            grid = GetComponent<Grid>();
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        void Start()
        {
            grid.cellSize = new Vector3(16f, 0f, 16f);

            foreach (Cell cell in map)
            {
                // if (cell.type == 1)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly; // ShadowCastingMode.On;
            }
        }

        void Update()
        {
            foreach (Cell cell in visibleCells)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            visibleCells.Clear();

            Vector3Int gridPos3D = grid.WorldToCell(playerTransform.position);
            Vector2Int gridPos = new Vector2Int(gridPos3D.x, gridPos3D.z);
            int mapIndex = (gridPos.y * mapWidth) + gridPos.x;

            if (gridPos.x < 0 || gridPos.x > mapWidth-1 || gridPos.y < 0 || gridPos.y > mapHeight-1) // if outside of map
                return;
            if (map[mapIndex].type == 1) // if inside a wall
                return;


            // TODO: go through each direction to until wall to find visible cells

            // for (int i = 0 ; i < mapWidth ; ++i)
            // {
            //     int index = (gridPos.y * mapWidth) + (gridPos.x + i);
            //     visibleCells.Add(map[index]);
            //     if (map[index].type == 1)
            //         break;
            // }

            // foreach (Cell cell in visibleCells)
            //     cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        }
    }

}
