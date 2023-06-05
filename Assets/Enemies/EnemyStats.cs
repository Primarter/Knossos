using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class EnemyStats : ScriptableObject
{
    [Range(0, 5)]
    public int detectionPriority = 0;
    public int life = 100;
    public int damage = 3;
    public int moveSpeed = 5;
}
