using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEngine.Rendering;

public class WallOpacity : MonoBehaviour
{
    [SerializeField]
    private float minOpacity = .17f;

    Stopwatch sw = new Stopwatch();
    private MeshRenderer renderer;

    private float _opacity = 1f;
    private float opacity
    {
        get => _opacity;
        set => _opacity = Mathf.Clamp(value, minOpacity, 1f);
    }

    private bool _display = false;
    public bool display
    {
        get => _display;
        set
        {
            _display = value;
            // renderer.shadowCastingMode = value ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly;
            sw.Restart();
        }
    }

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        sw.Start();
    }

    void Update()
    {
        if (!display && sw.ElapsedMilliseconds > 200)
        {
            display = true;
        }

        if (display)
        {
            opacity += 1f * Time.deltaTime;
        }
        else
        {
            opacity -= 1f * Time.deltaTime;
        }

        renderer.material.SetFloat("_Opacity", opacity);
    }
}
