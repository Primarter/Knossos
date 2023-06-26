using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TrailController : MonoBehaviour
{
    [SerializeField]
    private TrailRenderer trailRenderer;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        trailRenderer.widthMultiplier = animator.GetFloat("TrailSize");
    }

    public void EnableTrail()
    {
        trailRenderer.enabled = true;
    }

    public void DisableTrail()
    {
        trailRenderer.enabled = false;
        trailRenderer.Clear();
    }
}
