using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public enum BufferedInput
{
    Dodge,
    Attack
}

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

    [SerializeField]
    private InputConfig config;

    private static Stopwatch sw = new Stopwatch();
    private static Queue<(BufferedInput, long)> inputBuffer = new Queue<(BufferedInput, long)>();

    private void Start()
    {
        sw.Start();
    }

    private void Update()
    {
        (BufferedInput, long) buffered;
        while (inputBuffer.TryPeek(out buffered) && buffered.Item2 < sw.ElapsedMilliseconds)
        {
            inputBuffer.Dequeue();
        }

        inputs.horizontal = Input.GetAxisRaw("Horizontal");
        inputs.vertical = Input.GetAxisRaw("Vertical");
        inputs.dodge = Input.GetButtonDown("Dash");
        inputs.attack = Input.GetButtonDown("Attack");
        if (inputs.dodge)
        {
            inputBuffer.Enqueue((BufferedInput.Dodge, sw.ElapsedMilliseconds + config.dodgeBufferDuration));
        }
        if (inputs.attack)
        {
            inputBuffer.Enqueue((BufferedInput.Attack, sw.ElapsedMilliseconds + config.attackBufferDuration));
        }
    }

    public static bool CheckBuffer(BufferedInput input, bool consume = true)
    {
        (BufferedInput, long) buffered;
        if (inputBuffer.TryPeek(out buffered) && buffered.Item1 == input)
        {
            if (consume)
                inputBuffer.Dequeue();
            return true;
        }

        return false;
    }
}
