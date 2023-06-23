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
    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
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
        movement = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0) * smoothInput;
        movement = Vector3.ClampMagnitude(movement, 1);

        animator.SetFloat("Speed", movement.magnitude);

        #if UNITY_EDITOR
            animator.SetFloat("DodgeSpeed", config.DodgeAnimationSpeedMultiplier);
        #endif
    }

    public void TriggerDodge()
    {
        print("TriggerDodge");
        animator.SetTrigger("Dodge");
    }

    public void TriggerAttack()
    {
        print("TriggerAttack");
        animator.SetTrigger("Attack");
    }

    public void ResetAttack()
    {
        animator.ResetTrigger("Attack");
    }

    public void ResetDodge()
    {
        animator.ResetTrigger("Dodge");
    }

    //TODO
    // bend back hand when running
    // close hand on idle
}
