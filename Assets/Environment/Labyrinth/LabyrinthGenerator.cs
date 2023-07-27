using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Knossos.Map;

public class LabyrinthGenerator : EditorWindow
{
    Texture2D image;
    Knossos.Map.TilesConfig tileConfig;

    [MenuItem("Tools/Knossos/LabyrinthGenerator")]
    public static void ShowWindow()
    {
        LabyrinthGenerator wnd = GetWindow<LabyrinthGenerator>();
        wnd.titleContent = new GUIContent("LabyrinthGenerator");
    }

    void OnGUI()
    {
        GUILayout.Label("Labyrinth Generator", EditorStyles.boldLabel);

        image = (Texture2D) EditorGUILayout.ObjectField("Image", image, typeof (Texture2D), false, GUILayout.Height(EditorGUIUtility.singleLineHeight));
        tileConfig = (Knossos.Map.TilesConfig) EditorGUILayout.ObjectField("TileConfig", tileConfig, typeof(Knossos.Map.TilesConfig), false);

        EditorGUILayout.Separator();
        if (GUILayout.Button("Generate"))
        {
            if (tileConfig == null)
            {
                Debug.LogError("Image is not set!");
            }
            else if (image == null)
            {
                Debug.LogError("Image is not set!");
            }
            else
            {
                generateLabyrinth();
            }
        }
    }

    // ---

    int[] getNeighbourCells(Cell[] map, int x, int y, int width, int height)
    {
        // North, East, South, West
        int[] neigbours = new int[] {0, 0, 0, 0};

        // South
        if (y > 0)
            neigbours[2] = map[(y - 1) * width + x].type;

        // East
        if (x < width - 1)
            neigbours[1] = map[y * width + (x + 1)].type;

        // North
        if (y < width - 1)
            neigbours[0] = map[(y + 1) * width + x].type;

        // West
        if (x > 0)
            neigbours[3] = map[y * width + (x - 1)].type;

        return neigbours;
    }

    bool isTileValid(Knossos.Map.Tile tile, int[] neighbours)
    {
        // North, East, South, West
        var (N, E, S, W) = (neighbours[0], neighbours[1], neighbours[2], neighbours[3]);

        if (tile.hasConditionN && (tile.conditionN != N))
            return false;
        if (tile.hasConditionE && (tile.conditionE != E))
            return false;
        if (tile.hasConditionS && (tile.conditionS != S))
            return false;
        if (tile.hasConditionW && (tile.conditionW != W))
            return false;

        return true;
    }

    (GameObject, float rotation) getTile(Cell[] map, int x, int y, int width, int height)
    {
        int index = (y * image.width) + x;
        int type = map[index].type;

        foreach (var tile in tileConfig.tiles)
        {
            if (tile.type != type) continue;

            int[] neighbours = getNeighbourCells(map, x, y, width, height);

            if (tile.canRotate)
            {
                var neighboursRotate90 = new int[] {neighbours[3], neighbours[0], neighbours[1], neighbours[2]};
                var neighboursRotate180 = new int[] {neighbours[2], neighbours[3], neighbours[0], neighbours[1]};
                var neighboursRotate270 = new int[] {neighbours[1], neighbours[2], neighbours[3], neighbours[0]};
                var neigboursRotation = new int[][] {neighbours, neighboursRotate90, neighboursRotate180, neighboursRotate270};
                for (int i = 0 ; i < 4 ; ++i)
                {
                    float angle = i * -90f; // negative value because of the way the neighbours are manually rotated
                    if (isTileValid(tile, neigboursRotation[i]))
                        return (tile.prefab, angle);
                }
            }
            else
            {
                if (isTileValid(tile, neighbours))
                    return (tile.prefab, 0);
            }
        }

        return (null, 0);
    }

    void generateLabyrinth()
    {
        (Cell[] map, int width, int height) = imageToMap(image);

        GameObject parent = new GameObject("Labyrinth");
        LabyrinthManager labyrinthManager = parent.AddComponent<LabyrinthManager>();

        labyrinthManager.map = map;
        labyrinthManager.mapWidth = width;
        labyrinthManager.mapHeight = height;
        labyrinthManager.mapOrigin = new Vector3(-tileConfig.scale/2f, 0f, -tileConfig.scale/2f); // divide by 2 because tiles are centered
        labyrinthManager.mapScale = tileConfig.scale;

        for (int y = 0 ; y < height ; ++y) {
        for (int x = 0 ; x < width ; ++x) {
            int index = (y * width) + x;

            (GameObject prefab, float angle) = getTile(map, x, y, width, height);
            if (prefab != null)
            {
                GameObject obj = Instantiate(
                    prefab,
                    new Vector3(x, 0, y) * tileConfig.scale + new Vector3(tileConfig.scale/2f, 0, tileConfig.scale/2f), // because tiles are centered
                    Quaternion.Euler(0f, angle, 0f),
                    parent.transform
                );
                labyrinthManager.map[index].obj = obj;
            }
            else
            {
                Debug.LogError($"Tile could no be found for position x:{x} y:{y} in map!");
            }
        }
        }
    }

    public (Cell[] map, int width, int height) imageToMap(Texture2D imageMap)
    {
        string[] colorWalls = {"000080", "0000FF"};
        int width = imageMap.width;
        int height = imageMap.height;

        Color32[] pixels = imageMap.GetPixels32();

        Cell[] map = new Cell[width * height];

        for (int y = 0 ; y < height ; ++y) {
        for (int x = 0 ; x < width ; ++x) {
            int index = (y * width) + x;

            Color32 pixel = pixels[index];
            string colorHex = ColorUtility.ToHtmlStringRGB(pixel);

            bool isWall = colorWalls.Contains(colorHex);
            map[index].type = isWall ? 1 : 0;
            map[index].obj = null;
        }
        }
        return (map, width, height);
    }
}
