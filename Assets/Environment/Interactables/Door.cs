using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [SerializeField] float openingDuration = 3f;
    private Vector3 doorStartPosition;
    private Vector3 doorEndPosition;
    private System.Diagnostics.Stopwatch sw = new();
    private bool opened = false;

    private void Start()
    {
        doorStartPosition = transform.position;

        doorEndPosition = transform.position + new Vector3(0f, GetComponent<Collider>().bounds.center.y * -2, 0f);
    }

    public void OpenDoor()
    {
        if (!opened)
            StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
        sw.Start();

        while (sw.ElapsedMilliseconds <= openingDuration * 1000f)
        {
            float progress = Easing.easeInOutSine(sw.ElapsedMilliseconds / (openingDuration * 1000f));
            transform.position = Vector3.Lerp(doorStartPosition, doorEndPosition, progress);
            yield return null;
        }
        opened = true;
    }
}
