using System.Diagnostics;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField] float hoverAmplitude = 5f;
    [SerializeField] float hoverSpeed = 2f;

    Vector3 startPosition;
    Stopwatch sw = new();

    private void Start()
    {
        startPosition = transform.localPosition;
        sw.Start();
    }

    private void Update()
    {
        transform.localPosition = startPosition + new Vector3(0f, Mathf.Sin(sw.ElapsedMilliseconds / 1000f * hoverSpeed) * hoverAmplitude, 0f);
    }
}
