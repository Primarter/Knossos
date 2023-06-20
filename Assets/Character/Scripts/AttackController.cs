using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AnimationController))]
public class AttackController : MonoBehaviour
{
    CharacterController characterController;
    AnimationController animationController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (InputManager.inputs.attack)
            animationController.TriggerAttack();
        // TODO
        // Set PlayerController.active to false sor the duration of the attack
        // if attack is buffered when the previous ends, trigger again
        // have attack stages 1, 2, 3 with different timers for each to match animation
    }

}
