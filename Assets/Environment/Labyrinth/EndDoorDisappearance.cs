using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EndDoorDisappearance : MonoBehaviour
{
    [SerializeField] MeshRenderer doorTile;
    bool on = true;
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
        // if (on && doorTile.shadowCastingMode == ShadowCastingMode.ShadowsOnly)
        // {
        //     on = false;
        //     MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        //     foreach (var rend in renderers)
        //     {
        //         rend.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        //     }
        // }
        // if (!on && doorTile.shadowCastingMode == ShadowCastingMode.On)
        // {
        //     on = false;
        //     MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        //     foreach (var rend in renderers)
        //     {
        //         rend.shadowCastingMode = ShadowCastingMode.On;
        //     }
        // }
    }
}
