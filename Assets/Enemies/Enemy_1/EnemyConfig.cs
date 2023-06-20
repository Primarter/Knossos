using UnityEngine;

[CreateAssetMenu()]
public class EnemyConfig : ScriptableObject
{
    [Header("Stats")]
    public float attackSpeed = 25f;
    public float defaultSpeed = 2f;

    [Header("Timings")]
    public float triggerAttackDistance = 5f;
    public float startAttackCooldown = 0.3f;
    public float attackDuration = 0.5f;
    public float endAttackCooldown = 1f;
    public float nextAttackCooldown = 2f;

    [Header("StateMachine")]
    public Knossos.Enemy.State initialState;
}