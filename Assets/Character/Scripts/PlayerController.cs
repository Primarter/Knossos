using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AnimationController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig config;

    public float speed = 10f;

    private CharacterController characterController;
    private AnimationController animationController;
    private bool dashing = false;
    private bool canDash = true;
    private Vector3 movement;
    private Quaternion targetRotation;

    private float _stamina;
    public float stamina
    {
        get => _stamina;
        set => _stamina = Mathf.Clamp(value, 0, config.maxStamina);
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
    }

    private void Start()
    {
        _stamina = config.maxStamina;
        speed = config.speed;
    }

    private IEnumerator TriggerDash()
    {
        dashing = true;
        if (InputManager.inputs.dodge) {
            animationController.TriggerDodge();
        }
        canDash = false;
        speed *= config.dashStrength;
        stamina -= config.dashCost;
        yield return new WaitForSeconds(config.dashDuration);
        speed /= config.dashStrength;
        dashing = false;
        yield return new WaitForSeconds(config.dashCooldown);
        canDash = true;
    }

    private void Update()
    {
        // movement
        if (!dashing) {
            movement = new Vector3(InputManager.inputs.horizontal, 0, InputManager.inputs.vertical);
            movement = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * movement;
            movement = movement.normalized * movement.magnitude;
        }
        characterController.Move(movement * Time.deltaTime * speed);

        targetRotation = Quaternion.LookRotation(movement, Vector3.up); 
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // dash
        if (InputManager.inputs.dodge && canDash && stamina >= config.dashCost)
            StartCoroutine(TriggerDash());
        if (!dashing)
            stamina += config.staminaRegenPerSec * Time.deltaTime;
    }
}
