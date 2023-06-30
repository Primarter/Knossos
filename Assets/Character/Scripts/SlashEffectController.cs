using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(AnimationController))]
public class SlashEffectController : MonoBehaviour
{
    [SerializeField]
    private VisualEffect[] visualEffects;

    public void PlayEffect(int effectIdx)
    {
        if (effectIdx >= 0 && effectIdx < visualEffects.Length)
            visualEffects[effectIdx].Play();
        else
            Debug.LogError("Invalid effectIdx in PlayEffect");
    }

    public void PauseEffect(int effectIdx)
    {
        if (effectIdx >= 0 && effectIdx < visualEffects.Length)
        {
            print(effectIdx);
            visualEffects[effectIdx].playRate = 0f;
        } else
            Debug.LogError("Invalid effectIdx in PlayEffect");
    }

    public void ResumeEffect(int effectIdx)
    {
        if (effectIdx >= 0 && effectIdx < visualEffects.Length)
        {
            print(effectIdx);
            visualEffects[effectIdx].playRate = 1f;
        } else
            Debug.LogError("Invalid effectIdx in PlayEffect");
    }

}
