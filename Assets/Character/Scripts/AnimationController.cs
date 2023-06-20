using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig config;

    Animator animator;
    Vector3 smoothInput = Vector3.zero;
    Vector3 velocity = Vector3.zero;

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
        smoothInput = Vector3.SmoothDamp(smoothInput, movement, ref velocity, 5 * Time.deltaTime);
        movement = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * smoothInput;
        movement = Vector3.ClampMagnitude(movement, 1);

        animator.SetFloat("Speed", movement.magnitude);

        #if UNITY_EDITOR
            animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
        #endif
    }

    public void TriggerDodge()
    {
        animator.SetTrigger("Dodge");
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
    }
}
