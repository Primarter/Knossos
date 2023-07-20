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

        [SerializeField] GameObject debug1;
        [SerializeField] GameObject debug2;

        void Awake()
        {
            grid = GetComponent<Grid>();
            playerTransform = GameObject.FindWithTag("Player").transform;
            mainCamera = Camera.main;
        }

        void Start()
        {
            grid.cellSize = new Vector3(16f, 0f, 16f);

            foreach (Cell cell in map)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly; // ShadowCastingMode.On;
        }

        void Update()
        {
            // updateVisibility();
            updateVisibility2();
        }

        bool isInsideMap(Vector2Int coord)
        {
            return !(coord.x < 0 || coord.x > mapWidth-1 || coord.y < 0 || coord.y > mapHeight-1);
        }

        Vector3[] getCellBorderPosition(Cell cell)
        {
            Vector3 center = cell.obj.transform.position;
            var borders = new Vector3[4];
            borders[0] = new Vector3(center.x - 7.999f, 1f, center.z - 7.999f);
            borders[1] = new Vector3(center.x + 7.999f, 1f, center.z - 7.999f);
            borders[2] = new Vector3(center.x - 7.999f, 1f, center.z + 7.999f);
            borders[3] = new Vector3(center.x + 7.999f, 1f, center.z + 7.999f);
            return borders;
        }

        // Vector3[] getCellBorderPosition(Cell cell) // 8 point for traversal (not really the borders)
        // {
        //     Vector3 center = cell.obj.transform.position;
        //     var borders = new Vector3[8];
        //     borders[0] = new Vector3(center.x - 7.9990f, 1f, center.z - 7.9995f);
        //     borders[1] = new Vector3(center.x - 7.9995f, 1f, center.z - 7.9990f);

        //     borders[2] = new Vector3(center.x + 7.9990f, 1f, center.z - 7.9995f);
        //     borders[3] = new Vector3(center.x + 7.9995f, 1f, center.z - 7.9990f);

        //     borders[4] = new Vector3(center.x - 7.9990f, 1f, center.z + 7.9995f);
        //     borders[5] = new Vector3(center.x - 7.9995f, 1f, center.z + 7.9990f);

        //     borders[6] = new Vector3(center.x + 7.9990f, 1f, center.z + 7.9995f);
        //     borders[7] = new Vector3(center.x + 7.9995f, 1f, center.z + 7.9990f);
        //     return borders;
        // }

        bool isTileVisibleFrom(Cell cell, Vector2 p)
        {
            Vector3Int startTile3D = grid.WorldToCell(new Vector3(p.x, 0f, p.y));
            Vector2Int startTile = new(startTile3D.x, startTile3D.z);
            p = p / grid.cellSize.x;

            int startTileType = map[startTile.y * mapWidth + startTile.x].type;

            Vector3[] borders = getCellBorderPosition(cell);
            foreach (Vector3 border in borders)
            {
                Vector2 p2 = new Vector2(border.x, border.z) / 16f;
                int wallCount = 0;

                // TODO: don't just check if first cell is a wall, need to check when the traversal get out of walls for the first time
                bool doCheck = false;// = startTileType == 1 ? false : true;
                bool isVisible = true;
                foreach (Vector2 tile in gridTraverse(p, p2))
                {
                    var gridPos = Vector2Int.RoundToInt(tile);
                    int index = gridPos.y * mapWidth + gridPos.x;
                    if (cell.Equals(map[index])) break;
                    // if (doCheck && map[index].type == 1 && !cell.Equals(map[index] )) // encoutered a wall
                    if (map[index].type == 0)
                        doCheck = true;
                    // if (map[index].type == 1 && !cell.Equals(map[index] )) // encoutered a wall
                    // if (wallCount > 2 && map[index].type == 1 && !cell.Equals(map[index] )) // encoutered a wall
                    if ((doCheck || wallCount >= 2) && map[index].type == 1 && !cell.Equals(map[index] )) // encoutered a wall
                    {
                        isVisible = false;
                        break;
                    }

                    if (map[index].type == 1)
                        wallCount += 1;
                }

                // if (startTileType == 1 && wallCount > 1) // if camera is in wall, also check if in direct sight of player character
                // {
                //     RaycastHit hitInfo;
                //     bool hit = Physics.Linecast(playerTransform.position, border, out hitInfo, wallLayer);
                //     // TODO: traverse map from camera pose, if no wall, then false (OR MAYBE ONLY USE CAMERA instead of raycasting from player)
                //     if (hit && Vector3.Distance(hitInfo.point, border) > 0.1f) {
                //         isVisible = false;
                //     }
                // }

                if (isVisible)
                    return true;
            }
            return false;
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

            for (int dy = -2 ; dy <= 2; ++dy) {
            for (int dx = -2 ; dx <= 2; ++dx) {
                Vector2Int coord = new(playerCoord.x + dx, playerCoord.y + dy);
                if (!isInsideMap(coord)) continue;

                int index = coord.y * mapWidth + coord.x;
                visibleCells.Add(map[index]);
            }
            }

            foreach (Cell cell in visibleCells)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        }

        void updateVisibility2()
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
            /*
                max view range
                +X = 6
                -X = 1
                +Z = 6
                -Z = 1
            */
            for (int dy = -2 ; dy < 6; ++dy) {
            for (int dx = -2 ; dx < 6; ++dx) {
                Vector2Int coord = new(playerCoord.x + dx, playerCoord.y + dy);
                if (!isInsideMap(coord)) continue;

                int index = coord.y * mapWidth + coord.x;
                visibleCells.Add(map[index]);
            }
            }

            // Solution 2
            visibleCells.RemoveAll(cell =>
            {
                Vector2 p = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);// / 16f;
                if (isTileVisibleFrom(cell, p))
                    return false;
                return true;
            });

            // Solution 1
            // visibleCells.RemoveAll(cell =>
            // {
            //     Vector3[] borders = getCellBorderPosition(cell);
            //     foreach (Vector3 border in borders)
            //     {
            //         RaycastHit hitInfo;
            //         bool hit = Physics.Linecast(playerTransform.position, border, out hitInfo, wallLayer);
            //         // TODO: traverse map from camera pose, if no wall, then false (OR MAYBE ONLY USE CAMERA instead of raycasting from player)
            //         if (hit == false || Vector3.Distance(hitInfo.point, border) <= 0.1f) {
            //             return false;
            //         }
            //     }
            //     return true;
            // });
            // Vector2 p1 = new Vector2(debug1.transform.position.x, debug1.transform.position.z) / 16f;
            // Vector2 p2 = new Vector2(debug2.transform.position.x, debug2.transform.position.z) / 16f;
            // foreach (Vector2 tile in gridTraverse(p1, p2))
            // {
            //     var gridPos = Vector2Int.RoundToInt(tile);
            //     int index = gridPos.y * mapWidth + gridPos.x;
            //     visibleCells.Add(map[index]);
            // }

            foreach (Cell cell in visibleCells)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        }

        Vector2 Vector2Abs(Vector2 v)
        {
            return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }

        Vector2 Step(Vector2 edge, Vector2 x)
        {
            return new(
                x.x < edge.x ? 0.0f : 1.0f,
                x.y < edge.y ? 0.0f : 1.0f
            );
        }

        IEnumerable<Vector2> gridTraverse(Vector2 p0, Vector2 p1)
        {
            Vector2 rd = p1 - p0;
            Vector2 p = new(Mathf.Floor(p0.x), Mathf.Floor(p0.y));
            Vector2 rdinv = Vector2.one / rd;
            Vector2 stp = new(rd.x == 0f ? 0f : Mathf.Sign(rd.x),
                              rd.y == 0f ? 0f : Mathf.Sign(rd.y));

            Vector2 delta = new(Mathf.Min(rdinv.x * stp.x, 1f), Mathf.Min(rdinv.y * stp.y, 1f));
            Vector2 t_max = Vector2Abs((p + Vector2.Max(stp, Vector2.zero) - p0) * rdinv);

            for (int i = 0 ; i < 16 ; ++i)
            {
                yield return p; // TODO: return Vector2Int

                float next_t = Mathf.Min(t_max.x, t_max.y);
                if (next_t > 1.0) break;

                Vector2 cmp = Step(t_max, new Vector2(t_max.y, t_max.x));
                t_max += delta * cmp;
                p += stp * cmp;
            }
        }

        void OnDrawGizmos()
        {
            if (debug1 == null || debug2 == null) return;

            Gizmos.color = Color.red;

            Vector2 p1 = new Vector2(debug1.transform.position.x, debug1.transform.position.z) / 16f;
            Vector2 p2 = new Vector2(debug2.transform.position.x, debug2.transform.position.z) / 16f;
            foreach (Vector2 tile in gridTraverse(p1, p2))
            {
                Vector2 v = (tile + Vector2.one*0.5f) * 16f;
                Gizmos.DrawCube(new Vector3(v.x, 30f, v.y), Vector3.one * 8f);
            }
        }

    }
    /*
        https://www.shadertoy.com/view/XddcWn
        void traverse(vec2 p0, vec2 p1) {
            vec2 rd = p1 - p0;
            vec2 p = floor(p0);
            vec2 rdinv = 1.0 / rd;
            vec2 stp = sign(rd);
            vec2 delta = min(rdinv * stp, 1.0);
            // start at intersection of ray with initial cell
            vec2 t_max = abs((p + max(stp, vec2(0.0)) - p0) * rdinv);

            for (int i = 0; i < 128; ++i) {
                set_source_rgba(0.2,0.5,1.0,0.5);
                rectangle(p.x, p.y, 1.0, 1.0);
                fill();

                float next_t = min(t_max.x,t_max.y);
                if (next_t > 1.0) break;

                set_source_rgb(vec3(0.0));
                circle(p0 + next_t * rd, 0.15);
                fill();

                vec2 cmp = step(t_max.xy, t_max.yx);
                t_max += delta * cmp;
                p += stp * cmp;
            }
        }
    */

}
