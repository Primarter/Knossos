using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(AnimationController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig config;


    private CharacterController characterController;
    private AnimationController animationController;
    private Camera mainCam;

    private Stopwatch dashTimer = new Stopwatch();
    private bool canDash = true;

    private Vector3 movement;
    private Quaternion targetRotation;
    private float speed = 10f;
    private float attMoveSpeedMult = 1f;
    private float attRotSpeedMult = 1f;

    private float _stamina;
    public float stamina
    {
        get => _stamina;
        set => _stamina = Mathf.Clamp(value, 0, config.maxStamina);
    }

    private bool _dashing = false;
    public bool dashing
    {
        get => _dashing;
        private set => _dashing = value;
    }

    private bool _attacking = false;
    public bool attacking
    {
        get => _attacking;
        set
        {
            _attacking = value;
            canDash = !value;
            attMoveSpeedMult = value ? config.attackMoveSpeedMultiplier : 1f;
            attRotSpeedMult = value ? config.attackRotationSpeedMultiplier : 1f;
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        mainCam = Camera.main;
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
        if (movement.magnitude == 0f)
        {
            movement = transform.forward;
        }

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
            movement = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0) * movement;
            movement = Vector3.ClampMagnitude(movement, 1);
            characterController.Move(movement * Time.deltaTime * speed * attMoveSpeedMult);
        }
        else
            characterController.Move(movement.normalized * Time.deltaTime * speed);

        if (movement.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        }
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, config.rotationSpeed * attRotSpeedMult * Time.deltaTime);

        // dash
        if (canDash && stamina >= config.dashCost && InputManager.CheckBuffer(BufferedInput.Dodge))
            StartCoroutine(TriggerDash());
        if (!dashing)
            stamina += config.staminaRegenPerSec * Time.deltaTime;
    }
}
