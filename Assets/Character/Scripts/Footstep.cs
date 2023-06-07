using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Transform leftFoot;
    [SerializeField] Transform rightFoot;

    [SerializeField] AudioClip[] footstepSounds;

    bool lastLeftHit = false;
    bool lastRightHit = false;

    const float footHeightBias = 0.2f;
    const float raycastDistance = 0.2f;
    // TODO: add foot speed affecting volume

    AudioClip PickRandom(AudioClip[] sounds)
    {
        return sounds[Random.Range(0, sounds.Length)];
    }

    void Update()
    {
        Vector3 offset = new Vector3(0f, footHeightBias, 0f);

        bool leftHit = Physics.Raycast(leftFoot.position + offset, Vector3.down, footHeightBias + raycastDistance, groundLayer);
        bool rightHit = Physics.Raycast(rightFoot.position + offset, Vector3.down, footHeightBias + raycastDistance, groundLayer);

        if (leftHit && lastLeftHit == false)
        {
            SoundManager.PlaySound(leftFoot.position, PickRandom(footstepSounds), pitch: 0.7f, minDistance: 5f, maxDistance: 50f);
        }

        if (rightHit && lastRightHit == false)
        {
            SoundManager.PlaySound(rightFoot.position, PickRandom(footstepSounds), pitch: 0.7f, minDistance: 5f, maxDistance: 50f);
        }

        lastLeftHit = leftHit;
        lastRightHit = rightHit;
    }
}
