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
        movement = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * movement;
        movement = Vector3.ClampMagnitude(movement, 1);

        animator.SetFloat("Speed", movement.magnitude);

        var rot = Quaternion.Euler(0f, Mathf.Acos(Mathf.Clamp(Vector3.Dot(transform.forward, movement.normalized), -1f, 1f)), 0f);

        movement = rot * new Vector3(InputManager.inputs.horizontal, 0f, InputManager.inputs.vertical);

        movement = Quaternion.Euler(0f, -transform.rotation.eulerAngles.y, 0f) * movement;

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
