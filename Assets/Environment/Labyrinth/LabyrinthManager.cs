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
        [SerializeField] public Vector3 mapOrigin;
        [SerializeField] public float mapScale;

        [SerializeField] LayerMask wallLayer;
        Grid grid;
        Transform playerTransform;
        Camera mainCamera;

        List<Cell> visibleCells = new();

        [SerializeField] public bool debugOn = false;
        [SerializeField] public GameObject debug1;
        [SerializeField] public GameObject debug2;

        void Awake()
        {
            grid = GetComponent<Grid>();
            playerTransform = GameObject.FindWithTag("Player").transform;
            mainCamera = Camera.main;
        }

        void Start()
        {
            grid.cellSize = new Vector3(mapScale, 0f, mapScale);

            foreach (Cell cell in map)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }

        void Update()
        {
            // updateVisibilitySimple();
            updateVisibilityComplex();

            // hide tile camera is within if wall
            Vector3Int tileCamera = grid.WorldToCell(mainCamera.transform.position);
            Vector2Int tileCameraCoord = new Vector2Int(tileCamera.x, tileCamera.z);
            int tileCameraIndex = CoordToIndex(tileCameraCoord);
            if (map[tileCameraIndex].type == 1)
                map[tileCameraIndex].obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;

            // hide wall if between camera and player
            RaycastHit hitInfo;
            if (Physics.Linecast(mainCamera.transform.position, playerTransform.position, out hitInfo, wallLayer))
            {
                hitInfo.collider.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
        }

        Vector3[] getCellBorderPosition(Cell cell)
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

        bool isInsideMap(Vector2Int coord)
        {
            return (coord.x >= 0 && coord.x < mapWidth && coord.y >= 0 && coord.y < mapHeight);
        }

        bool isTileVisibleFrom(Cell cell, Vector2 p)
        {
            p = p / mapScale;

            Vector3[] borders = getCellBorderPosition(cell);
            foreach (Vector3 border in borders)
            {
                Vector2 p2 = new Vector2(border.x, border.z) / mapScale;

                // bool doCheck = false;
                int count = 0;
                bool isVisible = true;
                foreach (Vector2 tile in gridTraverse(p, p2))
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

        void updateVisibilitySimple()
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

        void updateVisibilityComplex()
        {
            foreach (Cell cell in visibleCells)
                cell.obj.GetComponent<MeshRenderer>().shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            visibleCells.Clear();

            Vector3Int playerCoord3D = grid.WorldToCell(playerTransform.position);
            Vector2Int playerCoord = new Vector2Int(playerCoord3D.x, playerCoord3D.z);
            int mapIndex = CoordToIndex(playerCoord);

            Vector3Int cameraCoord3D = grid.WorldToCell(mainCamera.transform.position);
            Vector2Int cameraCoord = new Vector2Int(cameraCoord3D.x, cameraCoord3D.z);
            int cameraMapIndex = CoordToIndex(cameraCoord);

            if (!isInsideMap(playerCoord) || !isInsideMap(cameraCoord))
                return;
            if (map[mapIndex].type == 1) // if player is inside a wall
                return;

            // [-2, 8] approximately frustum of camera
            for (int dy = -2 ; dy < 8; ++dy) {
            for (int dx = -2 ; dx < 8; ++dx) {
                Vector2Int coord = new(playerCoord.x + dx, playerCoord.y + dy);
                if (!isInsideMap(coord)) continue;

                int index = CoordToIndex(coord);
                visibleCells.Add(map[index]);
            }
            }

            visibleCells.RemoveAll(cell =>
            {
                Vector3Int tileCamera = grid.WorldToCell(mainCamera.transform.position);
                Vector3[] bordersTileCamera = getCellBorderPosition(map[cameraMapIndex]);
                Vector3[] bordersTilePlayer = getCellBorderPosition(map[mapIndex]);

                foreach (Vector3 border in bordersTileCamera)
                {
                    if (isTileVisibleFrom(cell, new Vector2(border.x, border.z)))
                        return false;
                }

                foreach (Vector3 border in bordersTilePlayer)
                {
                    if (isTileVisibleFrom(cell, new Vector2(border.x, border.z)))
                        return false;
                }

                return true;
            });

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

        public IEnumerable<Vector2> gridTraverse(Vector2 p0, Vector2 p1)
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
            foreach (Vector2 tile in gridTraverse(p1, p2))
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