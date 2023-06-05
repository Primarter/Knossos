using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : ScriptableObject
{
    [Range(0, 5)]
    int detectionPriority = 0;
}
