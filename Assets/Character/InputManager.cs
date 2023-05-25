using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public struct Inputs
    {
        public float horizontal;
        public float vertical;
        public bool dash;
    }

    public static Inputs inputs;

    void Update()
    {
        inputs.horizontal = Input.GetAxisRaw("Horizontal");
        inputs.vertical = Input.GetAxisRaw("Vertical");
        inputs.dash = Input.GetButtonDown("Dash");
    }
}
