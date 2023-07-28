using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

namespace Knossos.Minotaur
{

// Used to know if the minotaur is visible from the camera
public class PlayerMinotaurVisibilitySystem : MonoBehaviour
{
    MinotaurAgent agent;
    Camera mainCamera;

    [SerializeField] Collider objectCollider;
    [SerializeField] LayerMask occlusionLayer;

    [SerializeField] public float timeSinceVisible = 0f;
    [SerializeField] public bool isVisible = false;

    void Awake()
    {
        mainCamera = Camera.main;
        agent = GetComponent<MinotaurAgent>();
    }

    void FixedUpdate()
    {
        timeSinceVisible += Time.fixedDeltaTime;

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        if (
            GeometryUtility.TestPlanesAABB(planes, objectCollider.bounds) &&
            !Physics.Linecast(mainCamera.transform.position, transform.position, occlusionLayer))
        {
            timeSinceVisible = 0f;
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }
    }
}

}
