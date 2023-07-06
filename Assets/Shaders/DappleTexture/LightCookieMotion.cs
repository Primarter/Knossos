using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCookieMotion : MonoBehaviour
{
    [SerializeField] Material lightCookieMaterial;

    [Header("Texture 1")]
    [SerializeField] Vector2 cycleDuration1UV = new Vector2(20f, 20f);
    [SerializeField] AnimationCurve movementPath1U, movementPath1V;
    [SerializeField] Vector2 movementMagnitude1UV = new Vector2(0.1f, 0.1f);
    [SerializeField] Vector2 movementTimeOffset1UV = new Vector2();
    [SerializeField] Vector2 tex1TilingUV = new Vector2(1f, 1f);
    [SerializeField] Vector2 tex1OffsetUV = new Vector2(0f, 0f);

    [Header("Texture 2")]
    [SerializeField] Vector2 cycleDuration2UV = new Vector2(20f, 20f);
    [SerializeField] AnimationCurve movementPath2U, movementPath2V;
    [SerializeField] Vector2 movementMagnitude2UV = new Vector2(0.1f, 0.1f);
    [SerializeField] Vector2 movementTimeOffset2UV = new Vector2();
    [SerializeField] Vector2 tex2TilingUV = new Vector2(1f, 1f);
    [SerializeField] Vector2 tex2OffsetUV = new Vector2(0f, 0f);

    float time1U, time1V;
    float time2U, time2V;

    void Update()
    {
        time1U = Time.time % cycleDuration1UV.x;
        time1U /= cycleDuration1UV.x;

        time1U = Time.time % cycleDuration1UV.y;
        time1U /= cycleDuration1UV.y;

        time2U = Time.time % cycleDuration2UV.x;
        time2U /= cycleDuration2UV.x;

        time2U = Time.time % cycleDuration2UV.y;
        time2U /= cycleDuration2UV.y;

        UpdateMaterial();
    }

    void UpdateMaterial()
    {
        float newU1 = movementPath1U.Evaluate(time1U + movementTimeOffset1UV.x) * movementMagnitude1UV.x;
        float newV1 = movementPath1V.Evaluate(time1V + movementTimeOffset1UV.y) * movementMagnitude1UV.y;
        var newUV1 = new Vector4(tex1TilingUV.x, tex1TilingUV.y, newU1 + tex1OffsetUV.x, newV1 + tex1OffsetUV.y);
        lightCookieMaterial.SetVector("_Texture1_ST", newUV1);

        float newU2 = movementPath2U.Evaluate(time2U + movementTimeOffset2UV.x) * movementMagnitude2UV.x;
        float newV2 = movementPath2V.Evaluate(time2V + movementTimeOffset2UV.y) * movementMagnitude2UV.y;
        var newUV2 = new Vector4(tex2TilingUV.x, tex2TilingUV.y, newU2 + tex2OffsetUV.x, newV2 + tex2OffsetUV.y);
        lightCookieMaterial.SetVector("_Texture2_ST", newUV2);
    }

    void OnValidate()
    {
        UpdateMaterial();
    }
}
