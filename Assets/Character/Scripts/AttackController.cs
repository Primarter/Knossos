using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController), typeof(AnimationController))]
public class AttackController : MonoBehaviour
{
    PlayerController playerController;
    AnimationController animationController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animationController = GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (!playerController.dashing && InputManager.CheckBuffer(BufferedInput.Attack))
            animationController.TriggerAttack();
        // TODO
        // Set PlayerController.active to false sor the duration of the attack
        // if attack is buffered when the previous ends, trigger again
        // have attack stages 1, 2, 3 with different timers for each to match animation
    }

}
