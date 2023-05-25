using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerConfig config;

    public float speed = 10f;

    private CharacterController characterController;
    private bool dashing = false;
    private bool canDash = true;
    private Vector3 movement = new Vector3();

    private float _stamina;
    public float stamina
    {
        get => _stamina;
        set => _stamina = Mathf.Clamp(value, 0, config.maxStamina);
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _stamina = config.maxStamina;
        speed = config.speed;
    }

    private IEnumerator TriggerDash()
    {
        dashing = true;
        canDash = false;
        speed *= config.dashStrength;
        stamina -= config.dashCost;
        yield return new WaitForSeconds(config.dashDuration);
        speed /= config.dashStrength;
        dashing = false;
        yield return new WaitForSeconds(config.dashCooldown);
        canDash = true;
    }

    void Update()
    {
        // movement
        if (!dashing) {
            movement = new Vector3(InputManager.inputs.horizontal, 0, InputManager.inputs.vertical).normalized;
            movement = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0) * movement;
        }
        characterController.Move(movement * Time.deltaTime * speed);

        // dash
        if (InputManager.inputs.dash && canDash && stamina >= config.dashCost)
            StartCoroutine(TriggerDash());
        if (!dashing)
            stamina += config.staminaRegenPerSec * Time.deltaTime;
    }
}
