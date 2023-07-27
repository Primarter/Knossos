using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityFade : MonoBehaviour
{
    Renderer myRenderer;
    MaterialPropertyBlock propertyBlock;

    void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    void Update()
    {
        Color color = myRenderer.material.color;
        color.a = 0.0f;

        // propertyBlock.SetColor("_Color", color);
        myRenderer.SetPropertyBlock(propertyBlock);
    }
}
