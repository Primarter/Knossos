using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public struct Inputs
    {
        public float horizontal;
        public float vertical;
    }

    public static Inputs inputs;

    void Update()
    {
        inputs.horizontal = Input.GetAxis("Horizontal");
        inputs.vertical = Input.GetAxis("Vertical");
    }
}
