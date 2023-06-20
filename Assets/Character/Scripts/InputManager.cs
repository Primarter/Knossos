using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public struct Inputs
    {
        public float horizontal;
        public float vertical;
        public bool dodge;
        public bool attack;
    }

    public static Inputs inputs;

    private void Update()
    {
        inputs.horizontal = Input.GetAxisRaw("Horizontal");
        inputs.vertical = Input.GetAxisRaw("Vertical");
        inputs.dodge = Input.GetButtonDown("Dash");
        inputs.attack = Input.GetButtonDown("Attack");
    }
}
