using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Torch : MonoBehaviour
{
    [SerializeField] VisualEffect bonfire;
    [SerializeField] VisualEffect sparks;
    [SerializeField] Light pointLight;
    bool on = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!on && other.tag == "Player")
        {
            bonfire.Play();
            sparks.Play();
            pointLight.enabled = true;
            on = true;
            StartCoroutine(StopSparks());
        }
    }

    private IEnumerator StopSparks()
    {
        yield return new WaitForSeconds(.1f);
        sparks.Stop();
    }
}
