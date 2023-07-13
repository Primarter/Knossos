using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Bell
{

public class AttractMinotaur : MonoBehaviour
{
    public AudioClip bellSound;

    public void RingBell()
    {
        LoudSoundManager.playLoudSound(transform.position);
        SoundManager.PlaySound(transform.position, bellSound, 3, maxDistance: 40, pitch: Random.Range(0.95f, 1.05f));
    }
}

}