using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

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

        [SerializeField] LayerMask wallLayer;
        Grid grid;
        Transform playerTransform;
        Camera mainCamera;

        List<Cell> visibleCells = new();

        void Awake()
        {
            grid = GetComponent<Grid>();
            playerTransform = GameObject.FindWithTag("Player").transform;
            mainCamera = Camera.main;
        }

        void Start()
        {
            grid.cellSize = new Vector3(16f, 0f, 16f);

            // foreach (Cell cell in map)
            //     cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly; // ShadowCastingMode.On;
        }

        bool isInsideMap(Vector2Int coord)
        {
            return !(coord.x < 0 || coord.x > mapWidth-1 || coord.y < 0 || coord.y > mapHeight-1);
        }

        Vector3[] getCellBorderPosition(Cell cell)
        {
            Vector3 center = cell.obj.transform.position;
            var borders = new Vector3[4];
            borders[0] = new Vector3(center.x - 8f, 1f, center.z - 8f);
            borders[1] = new Vector3(center.x + 8f, 1f, center.z - 8f);
            borders[2] = new Vector3(center.x - 8f, 1f, center.z + 8f);
            borders[3] = new Vector3(center.x + 8f, 1f, center.z + 8f);
            return borders;
        }

        void updateVisibility()
        {
            foreach (Cell cell in visibleCells)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            visibleCells.Clear();

            Vector3Int playerCoord3D = grid.WorldToCell(playerTransform.position);
            Vector2Int playerCoord = new Vector2Int(playerCoord3D.x, playerCoord3D.z);
            int mapIndex = (playerCoord.y * mapWidth) + playerCoord.x;

            if (!isInsideMap(playerCoord))
                return;
            if (map[mapIndex].type == 1) // if inside a wall
                return;

            // Naive solution: get walls in area that match view frustrum around player, then do a raycast to each wall from the player to see if it can be seen
            // can't get it to work
            /*
                max view range
                +X = 5
                -X = 1
                +Z = 6
                -Z = 1
            */
            for (int dy = -1 ; dy < 6; ++dy) {
            for (int dx = -1 ; dx < 5; ++dx) {
            // for (int dy = -1 ; dy <= 1; ++dy) {
            // for (int dx = -1 ; dx <= 1; ++dx) {
                Vector2Int coord = new(playerCoord.x + dx, playerCoord.y + dy);
                if (!isInsideMap(coord)) continue;

                int index = coord.y * mapWidth + coord.x;
                visibleCells.Add(map[index]);
            }
            }

            /*
            def raytrace(v0, v1):
                # The equation of the ray is v = v0 + t*d
                d = v1 - v0
                inc = np.sign(d)  # Gives the quadrant in which the ray progress

                # Rounding coordinates give us the current tile
                tile = np.array(np.round(v0), dtype=int)
                tiles = [tile]
                v = v0
                endtile = np.array(np.round(v1), dtype=int)

                # Continue as long as we are not in the last tile
                while np.max(np.abs(endtile - v)) > 0.5:
                    # L = (Lx, Ly) where Lx is the x coordinate of the next vertical
                    # line and Ly the y coordinate of the next horizontal line
                    L = tile + 0.5*inc

                    # Solve the equation v + d*t == L for t, simultaneously for the next
                    # horizontal line and vertical line
                    t = (L - v)/d

                    if t[0] < t[1]:  # The vertical line is intersected first
                        tile[0] += inc[0]
                        v += t[0]*d
                    else:  # The horizontal line is intersected first
                        tile[1] += inc[1]
                        v += t[1]*d

                    tiles.append(tile)

                return tiles
            */

            // visibleCells.RemoveAll(cell =>
            // {
            //     Vector3[] borders = getCellBorderPosition(cell);
            //     foreach (Vector3 border in borders)
            //     {
            //         RaycastHit hitInfo;
            //         bool hit = Physics.Linecast(playerTransform.position, border, out hitInfo, wallLayer);
            //         TODO: traverse map from camera pose, if no wall, then false (OR MAYBE ONLY USE CAMERA instead of raycasting from player)
            //         if (hit == false || Vector3.Distance(hitInfo.point, border) <= 0.1f) {
            //             return false;
            //         }
            //     }
            //     return true;
            // });

            /* better solution
                Do dijkstra search
                    - if wall, just stop there and take it
                    - if floor in same direction as previous continue
                    - if floor but it turned, stop
            */
            // code here


            foreach (Cell cell in visibleCells)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        }

        void Update()
        {
            // updateVisibility();
        }
    }

}
