using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System;

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
        public Cell[] map;
        public int mapHeight;
        public int mapWidth;
        public Vector3 mapOrigin;
        public float mapScale;
        public LayerMask wallLayer;

        Grid grid;
        Transform playerTransform;
        Camera mainCamera;

        // List<Cell> visibleCells = new();
        // List<Cell> culledCells = new();
        List<GameObject> culledObjects = new();

        [SerializeField] Vector2Int playerCoord;
        [SerializeField] int playerIndex;
        [SerializeField] Vector2Int cameraCoord;
        [SerializeField] int cameraIndex;

        public bool debugOn = false;
        public GameObject debug1;
        public GameObject debug2;

        void Awake()
        {
            grid = GetComponent<Grid>();
            playerTransform = GameObject.FindWithTag("Player").transform;
            mainCamera = Camera.main;
        }

        void Start()
        {
            grid.cellSize = new Vector3(mapScale, 0f, mapScale);

            // foreach (Cell cell in map)
            //     cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }

        void Update()
        {
            Vector3Int playerCoord3D = grid.WorldToCell(playerTransform.position);
            playerCoord = new Vector2Int(playerCoord3D.x, playerCoord3D.z);
            playerIndex = CoordToIndex(playerCoord);

            Vector3Int cameraCoord3D = grid.WorldToCell(mainCamera.transform.position);
            cameraCoord = new Vector2Int(cameraCoord3D.x, cameraCoord3D.z);
            cameraIndex = CoordToIndex(cameraCoord);


            foreach (GameObject obj in culledObjects) // turn back on all culled object
                obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
            culledObjects.Clear();


            UpdateVisibilitySimple();
            // UpdateVisibilityComplex();
            // HideCollidingWall();
            HideWallBetweenCameraAndPlayer();

            foreach (GameObject obj in culledObjects) // turn off visibility of culled objects
                obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }

        Vector3[] GetCellBorderPosition(Cell cell)
        {
            Vector3 center = cell.obj.transform.position;
            var borders = new Vector3[4];
            float offset = mapScale / 2f - 0.001f;
            borders[0] = new Vector3(center.x - offset, 1f, center.z - offset);
            borders[1] = new Vector3(center.x + offset, 1f, center.z - offset);
            borders[2] = new Vector3(center.x - offset, 1f, center.z + offset);
            borders[3] = new Vector3(center.x + offset, 1f, center.z + offset);
            return borders;
        }

        int CoordToIndex(Vector2Int coord)
        {
            return coord.y * mapWidth + coord.x;
        }

        bool IsInsideMap(Vector2Int coord)
        {
            return (coord.x >= 0 && coord.x < mapWidth && coord.y >= 0 && coord.y < mapHeight);
        }

        bool IsTileVisibleFrom(Cell cell, Vector2 p)
        {
            p /= mapScale;

            Vector3[] borders = GetCellBorderPosition(cell);
            foreach (Vector3 border in borders)
            {
                Vector2 p2 = new Vector2(border.x, border.z) / mapScale;

                // bool doCheck = false;
                int count = 0;
                bool isVisible = true;
                foreach (Vector2 tile in GridTraverse(p, p2))
                {
                    var gridPos = Vector2Int.RoundToInt(tile);
                    int index = CoordToIndex(gridPos);

                    // print(gridPos);
                    // if (!isInsideMap(gridPos)) break;
                    if (cell.obj == map[index].obj) break;
                    // if (map[index].type == 0) { doCheck = true };
                    // if (doCheck && map[index].type == 1 ))
                    // if (count > 0 && map[index].type == 1 ))
                    if (count > 0 && map[index].type == 1) // encoutered a wall
                    {
                        isVisible = false;
                        break;
                    }

                    count += 1;
                }

                if (isVisible)
                    return true;
            }
            return false;
        }

        bool AddToCullIfWall(Vector2Int coord)
        {
            int index = CoordToIndex(coord);
            if (!IsInsideMap(coord)) return false;
            if (map[index].type > 1) Debug.Log(map[index].type);
            if (map[index].type != 1) return false;
            culledObjects.Add(map[index].obj);
            return true;
        }

        bool AddToCull(Vector2Int coord)
        {
            int index = CoordToIndex(coord);
            if (!IsInsideMap(coord)) return false;
            culledObjects.Add(map[index].obj);
            return true;
        }

        public Vector2Int GetGridCoordinates(Vector3 worldCoordinates)
        {
            Vector3Int coord3D = grid.WorldToCell(worldCoordinates);
            return new(coord3D.x, coord3D.z);
        }

        void PropagateWallCulling(Vector2Int coord)
        {
            for (int i = 0 ; i < 5; ++i) // +X
                if (AddToCullIfWall(new(coord.x + i, coord.y)) == false) break;
            for (int i = 0 ; i < 5; ++i) // -X
                if (AddToCullIfWall(new(coord.x - i, coord.y)) == false) break;
            for (int i = 0 ; i < 5; ++i) // +Y
                if (AddToCullIfWall(new(coord.x, coord.y + i)) == false) break;
            for (int i = 0 ; i < 5; ++i) // -Y
                if (AddToCullIfWall(new(coord.x, coord.y - i)) == false) break;

            for (int i = 0 ; i < 5; ++i) // +X
                if (AddToCullIfWall(new(coord.x + i, coord.y - 1)) == false) break;
            for (int i = 0 ; i < 5; ++i) // -X
                if (AddToCullIfWall(new(coord.x - i, coord.y - 1)) == false) break;
            for (int i = 0 ; i < 5; ++i) // +Y
                if (AddToCullIfWall(new(coord.x - 1, coord.y + i)) == false) break;
            for (int i = 0 ; i < 5; ++i) // -Y
                if (AddToCullIfWall(new(coord.x - 1, coord.y - i)) == false) break;
        }

        void UpdateVisibilitySimple()
        {
            if (!IsInsideMap(cameraCoord))
                return;
            if (map[cameraIndex].type != 1) // if not inside a wall
                return;

            PropagateWallCulling(cameraCoord);
        }

        void HideWallBetweenCameraAndPlayer()
        {
            if (Physics.Linecast(mainCamera.transform.position, playerTransform.position, out RaycastHit hitInfo, wallLayer))
            {
                Vector3Int coord3D = grid.WorldToCell(hitInfo.point);
                Vector2Int coord = new(coord3D.x, coord3D.z);
                int index = CoordToIndex(coord);

                culledObjects.Add(map[index].obj);
                PropagateWallCulling(coord);

                // if (map[index].type != 1) // if not inside a wall
                //     return;
                // for (int i = 0 ; i < 5; ++i) // +X
                //     if (AddToCullIfWall(new(coord.x + i, coord.y)) == false) break;
                // for (int i = 0 ; i < 5; ++i) // -X
                //     if (AddToCullIfWall(new(coord.x - i, coord.y)) == false) break;
                // for (int i = 0 ; i < 5; ++i) // +Y
                //     if (AddToCullIfWall(new(coord.x, coord.y + i)) == false) break;
                // for (int i = 0 ; i < 5; ++i) // -Y
                //     if (AddToCullIfWall(new(coord.x, coord.y - i)) == false) break;
            }
        }

        void HideCollidingWall()
        {
            Vector3Int tileCamera = grid.WorldToCell(mainCamera.transform.position);
            Vector2Int tileCameraCoord = new(tileCamera.x, tileCamera.z);
            int tileCameraIndex = CoordToIndex(tileCameraCoord);
            if (map[tileCameraIndex].type == 1)
            {
                map[tileCameraIndex].obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
        }

        // void UpdateVisibilityComplex()
        // {
        //     foreach (Cell cell in visibleCells)
        //         cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        //     visibleCells.Clear();

        //     if (!IsInsideMap(playerCoord) || !IsInsideMap(cameraCoord))
        //         return;
        //     if (map[playerIndex].type == 1) // if player is inside a wall
        //         return;

        //     // [-2, 8] approximately frustum of camera
        //     for (int dy = -2 ; dy < 8; ++dy) {
        //     for (int dx = -2 ; dx < 8; ++dx) {
        //         Vector2Int coord = new(playerCoord.x + dx, playerCoord.y + dy);
        //         if (!IsInsideMap(coord)) continue;

        //         int index = CoordToIndex(coord);
        //         visibleCells.Add(map[index]);
        //     }
        //     }

        //     visibleCells.RemoveAll(cell =>
        //     {
        //         Vector3Int tileCamera = grid.WorldToCell(mainCamera.transform.position);
        //         Vector3[] bordersTileCamera = GetCellBorderPosition(map[cameraIndex]);
        //         Vector3[] bordersTilePlayer = GetCellBorderPosition(map[playerIndex]);

        //         foreach (Vector3 border in bordersTileCamera)
        //         {
        //             if (IsTileVisibleFrom(cell, new Vector2(border.x, border.z)))
        //                 return false;
        //         }

        //         foreach (Vector3 border in bordersTilePlayer)
        //         {
        //             if (IsTileVisibleFrom(cell, new Vector2(border.x, border.z)))
        //                 return false;
        //         }

        //         return true;
        //     });

        //     foreach (Cell cell in visibleCells)
        //         cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.On;
        // }

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

        public IEnumerable<Vector2> GridTraverse(Vector2 p0, Vector2 p1)
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
                if (next_t > 1.0f) break;

                Vector2 cmp = Step(t_max, new Vector2(t_max.y, t_max.x));
                t_max += delta * cmp;
                p += stp * cmp;
            }
        }

        void OnDrawGizmos()
        {
            if (!debugOn || debug1 == null || debug2 == null) return;

            Gizmos.color = Color.red;

            Vector2 p1 = new Vector2(debug1.transform.position.x, debug1.transform.position.z) / mapScale;
            Vector2 p2 = new Vector2(debug2.transform.position.x, debug2.transform.position.z) / mapScale;
            foreach (Vector2 tile in GridTraverse(p1, p2))
            {
                Vector2 v = (tile + Vector2.one*0.5f) * mapScale;
                Gizmos.DrawCube(new Vector3(v.x, 30f, v.y), Vector3.one * 8f);
            }
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