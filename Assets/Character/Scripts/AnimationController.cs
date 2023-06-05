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
        movement = Vector3.ClampMagnitude(movement, 1);

        animator.SetFloat("Speed", movement.magnitude);

        // movement = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * movement;

        // var rot = Quaternion.Euler(0f, Mathf.Acos(Vector3.Dot(transform.forward, movement)), 0f);

        // movement = rot * new Vector3(InputManager.inputs.horizontal, 0f, InputManager.inputs.vertical);

        movement = Quaternion.Euler(0f, -transform.rotation.eulerAngles.y, 0f) * movement;

        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveZ", movement.z);

        Debug.DrawRay(transform.position, transform.forward, Color.green);
        Debug.DrawRay(transform.position, movement, Color.red);

        print((movement.x, movement.z));

        #if UNITY_EDITOR
            animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
        #endif
    }

    public void TriggerDodge()
    {
        animator.SetTrigger("Dodge");
    }
}
