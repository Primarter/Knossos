using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AnimationController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig config;

    public float speed = 10f;

    private CharacterController characterController;
    private AnimationController animationController;

    private Stopwatch dashTimer = new Stopwatch();
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
        targetRotation = Quaternion.LookRotation(transform.forward);
    }

    private IEnumerator TriggerDash()
    {
        dashing = true;
        canDash = false;

        animationController.TriggerDodge();
        stamina -= config.dashCost;
        dashTimer.Restart();

        while (dashTimer.ElapsedMilliseconds < config.dashDuration * 1000f) {
            float curveValue = config.DashSpeedCurve.Evaluate((float)dashTimer.ElapsedMilliseconds / (config.dashDuration * 1000f));
            speed = (curveValue * config.dashStrength + 1) * config.speed;
            yield return null;
        }

        speed = config.speed;
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
            movement = movement.normalized * Mathf.Clamp01(movement.magnitude);
        }
        characterController.Move(movement * Time.deltaTime * speed);

        if (movement.magnitude > 0)
            targetRotation = Quaternion.LookRotation(movement, Vector3.up); 
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * Time.deltaTime);

        // dash
        if (InputManager.inputs.dodge && canDash && stamina >= config.dashCost)
            StartCoroutine(TriggerDash());
        if (!dashing)
            stamina += config.staminaRegenPerSec * Time.deltaTime;
    }
}
