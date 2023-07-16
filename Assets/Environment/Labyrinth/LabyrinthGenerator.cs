using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
            imageToMap();
            generateMap();
        }
    }

    // ---

    [System.NonSerialized] string[] colorWalls = {"000080", "0000FF"}; // System.NonSerialized is needed because otherwise the values don't get updated when changed
    int[] map;
    int mapWidth, mapHeight;

    int[] getNeighbourCells(int x, int y)
    {
        // North, East, South, West
        int[] neigbours = new int[] {0, 0, 0, 0};

        // South
        if (y > 0)
            neigbours[2] = map[(y - 1) * mapWidth + x];

        // East
        if (x < mapWidth - 1)
            neigbours[1] = map[y * mapWidth + (x + 1)];

        // North
        if (y < mapHeight - 1)
            neigbours[0] = map[(y + 1) * mapWidth + x];

        // West
        if (x > 0)
            neigbours[3] = map[y * mapWidth + (x - 1)];

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

    (GameObject, float rotation) getTile(int x, int y)
    {
        int index = (y * image.width) + x;
        int id = map[index];

        foreach (var tile in tileConfig.tiles)
        {
            if (tile.id != id) continue;

            int[] neighbours = getNeighbourCells(x, y);

            Debug.Log((x, y, (neighbours[0], neighbours[1], neighbours[2], neighbours[3])));

            if (tile.canRotate)
            {
                var neighboursRotate90 = new int[] {neighbours[3], neighbours[0], neighbours[1], neighbours[2]};
                var neighboursRotate180 = new int[] {neighbours[2], neighbours[3], neighbours[0], neighbours[1]};
                var neighboursRotate270 = new int[] {neighbours[1], neighbours[2], neighbours[3], neighbours[0]};
                var neigboursRotation = new int[][] {neighbours, neighboursRotate90, neighboursRotate180, neighboursRotate270};
                // TODO: check for the 4 direction and also rotated in 4 ways
                for (int i = 0 ; i < 4 ; ++i)
                {
                    float angle = i * -90f;
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

    void generateMap()
    {
        GameObject parent = new GameObject("Labyrinth");

        for (int y = 0 ; y < mapHeight ; ++y) {
        for (int x = 0 ; x < mapWidth ; ++x) {
            int index = (y * mapWidth) + x;

            (GameObject prefab, float angle) = getTile(x, y);
            if (prefab != null)
                Instantiate(prefab, new Vector3(x, 0, y) * tileConfig.scale, Quaternion.Euler(0f, angle, 0f), parent.transform);
            else
                Debug.LogError($"Tile could no be found for position x:{x} y:{y} in map!");
        }
        }
    }

    void imageToMap()
    {
        if (image == null) {
            Debug.LogError("Image is not set!");
            return;
        }

        mapWidth = image.width;
        mapHeight = image.height;

        Color32[] pixels = image.GetPixels32();

        map = new int[mapWidth * mapHeight];
        Array.Clear(map, 0, map.Length); // fill with zeros

        for (int y = 0 ; y < mapHeight ; ++y) {
        for (int x = 0 ; x < mapWidth ; ++x) {
            int index = (y * mapWidth) + x;

            Color32 pixel = pixels[index];
            string colorHex = ColorUtility.ToHtmlStringRGB(pixel);

            bool isWall = colorWalls.Contains(colorHex);
            map[index] = isWall ? 1 : 0;
        }
        }

    }
}
