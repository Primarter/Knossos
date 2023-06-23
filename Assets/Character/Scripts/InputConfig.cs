using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class InputConfig : ScriptableObject
{
    [Header("Buffer config, all in ms")]
    public long dodgeBufferDuration = 300;
    public long attackBufferDuration = 100;
}