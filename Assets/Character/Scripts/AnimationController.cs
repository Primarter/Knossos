using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig config;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
    }

    private void Update()
    {
        Vector2 movement = new Vector2(InputManager.inputs.horizontal, InputManager.inputs.vertical);

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveZ", movement.y);

        animator.SetFloat("Speed", movement.magnitude);

        #if UNITY_EDITOR
            animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
        #endif
    }

    public void TriggerDodge()
    {
        animator.SetTrigger("Dodge");
    }
}
