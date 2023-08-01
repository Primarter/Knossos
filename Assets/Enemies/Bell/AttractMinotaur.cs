using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bell
{

public class AttractMinotaur : MonoBehaviour
{
    public AudioClip bellSound;
    GameObject minautor;

    MinotaurSpawnPoint[] closeMinautorSpawnPoints;

    void Awake()
    {
        minautor = GameObject.FindWithTag("Minotaur");
        closeMinautorSpawnPoints = transform.parent.GetComponentsInChildren<MinotaurSpawnPoint>();
    }

    public void RingBell()
    {
        if (minautor != null && !minautor.activeInHierarchy) // if not active, place minautor at random spawnPoint and activate it
        {
            Vector3 p = closeMinautorSpawnPoints[Random.Range(0, closeMinautorSpawnPoints.Length)].transform.position;
            minautor.SetActive(true);
            minautor.GetComponent<Minotaur.LocomotionSystem>().navMeshAgent.Warp(p);
        }

        LoudSoundManager.playLoudSound(transform.position);
        SoundManager.PlaySound(transform.position, bellSound, 3, maxDistance: 40, pitch: Random.Range(0.95f, 1.05f));
    }
}

}