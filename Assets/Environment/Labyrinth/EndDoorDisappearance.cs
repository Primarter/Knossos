using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EndDoorDisappearance : MonoBehaviour
{
    [SerializeField] MeshRenderer doorTile;
    MeshRenderer[] renderers;

    private void Start()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
    }

    private void Update()
    {
        foreach (var rend in renderers)
        {
            rend.shadowCastingMode = doorTile.shadowCastingMode;
        }
    }
}
