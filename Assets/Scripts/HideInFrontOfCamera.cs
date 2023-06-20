using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInFrontOfCamera : MonoBehaviour
{
    MeshRenderer meshRenderer;
    Collider objCollider;
    Camera mainCamera;
    Transform playerTransform;

    Plane[] planes;

    [SerializeField] LayerMask layer;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        objCollider = GetComponent<Collider>();
        mainCamera = Camera.main;
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
    }

    void OnDrawGizmos()
    {
        if (Application.isEditor) return;

        Gizmos.DrawSphere(mainCamera.transform.position, 2f);
        Gizmos.DrawSphere(playerTransform.position, 2f);
    }

    void Update()
    {
        float distanceCameraPlayer = Vector3.Distance(mainCamera.transform.position, playerTransform.position);

        RaycastHit hit;
        bool didHit = Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, distanceCameraPlayer, layer);

        // Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward * distanceCameraPlayer, Color.red, 2f);
        if (didHit)
        {
            meshRenderer.enabled = hit.transform.gameObject == this;
        }
        else
        {
            meshRenderer.enabled = true;
        }

        // Plane playerCameraPlane = new Plane(mainCamera.transform.forward, playerTransform.position);
        // bool side = playerCameraPlane.GetSide(transform.position);
        // meshRenderer.enabled = side;

        // planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        // if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        // {
        //     meshRenderer.enabled = false;
        // }
        // else
        // {
        //     meshRenderer.enabled = true;
        // }
    }
}
