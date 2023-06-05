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
        Vector3 movement = new Vector3(InputManager.inputs.horizontal, 0f, InputManager.inputs.vertical);

        animator.SetFloat("Speed", movement.magnitude);

        movement = Quaternion.LookRotation(transform.forward, Vector3.up) * movement;

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveZ", movement.z);

        #if UNITY_EDITOR
            animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
        #endif
    }

    public void TriggerDodge()
    {
        animator.SetTrigger("Dodge");
    }
}
