using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewDistanceShader : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] Transform playerTransform;

    [SerializeField] Color color;
    [SerializeField] Vector2 range;

    void Update()
    {
        material.SetVector("_PlayerPosition", playerTransform.position);
        material.SetVector("_Color", color);
        material.SetVector("_Range", range);
    }
}
