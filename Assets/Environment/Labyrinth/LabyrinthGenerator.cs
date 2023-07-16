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

        // North
        if (y > 0)
            neigbours[0] = map[(y - 1) * mapWidth + x];

        // East
        if (x < mapWidth - 1)
            neigbours[1] = map[y * mapWidth + (x + 1)];

        // South
        if (y < mapHeight - 1)
            neigbours[2] = map[(y + 1) * mapWidth + x];

        // West
        if (x > 0)
            neigbours[3] = map[y * mapWidth + (x - 1)];

        return neigbours;
    }

    bool isTileValid(Knossos.Map.Tile tile, int[] neighbours)
    {
            // if (tile.hasConditionN)
            //     if ()
        return true;
    }

    GameObject getTile(int x, int y)
    {
        int index = (y * image.width) + x;
        int id = map[index];

        foreach (var tile in tileConfig.tiles)
        {
            if (tile.id != id) continue;

            // (int N, int E, int S, int W) = getNeighbourCells(x, y);
            int[] neighbours = getNeighbourCells(x, y);
            // isTileValid(tile, neighbours);



            // check for the 4 direction and also rotated in 4 ways
        }

        return tileConfig.tiles[0].prefab;
    }

    void generateMap()
    {
        GameObject parent = new GameObject("Labyrinth");

        for (int y = 0 ; y < mapHeight ; ++y) {
        for (int x = 0 ; x < mapWidth ; ++x) {

            int index = (y * mapWidth) + x;
            if (map[index] != 1) continue; // is not wall

            GameObject prefab = getTile(x, y);
            Instantiate(prefab, new Vector3(x, 0, y) * tileConfig.scale, Quaternion.identity, parent.transform);
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
