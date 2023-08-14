using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioClip specialAttackSound;
    [SerializeField] AudioClip[] swooshSounds;

    public void PlaySwooshSound()
    {
        SoundManager.PlaySound(
            transform.position,
            ArrayHelper.PickRandom(swooshSounds),
            spatialBlend: 0.5f,
            volume: 0.4f,
            pitch: Random.Range(0.9f, 1.1f)
        );
    }

    public void PlaySpecialAttackSound()
    {
        SoundManager.PlaySound(
            transform.position,
            specialAttackSound,
            spatialBlend: 0.5f,
            volume: 0.5f
        );
    }
}
