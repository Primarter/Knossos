using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2 movement = new Vector2(InputManager.inputs.horizontal, InputManager.inputs.vertical);

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveZ", movement.y);

        animator.SetFloat("Speed", movement.magnitude);
    }

    public void TriggerDodge()
    {
        animator.SetTrigger("Dodge");
    }
}
