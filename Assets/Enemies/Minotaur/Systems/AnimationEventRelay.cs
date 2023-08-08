using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{

public class AnimationEventRelay : MonoBehaviour
{
    [SerializeField] AnimationSystem animationSystem;

    public void OnAttackActiveEvent()
    {
        animationSystem.OnAttackActiveEvent();
    }

    public void OnAttackInactiveEvent()
    {
        animationSystem.OnAttackInactiveEvent();
    }
}

}