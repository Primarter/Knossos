using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{
    public class VisionSystem : MonoBehaviour
    {
        GameObject player;
        GameObject target;

        public bool hasTarget = false;

        [SerializeField] public float FOV = 150f; // field of view degrees
        [SerializeField] public float viewRange = 20f;
        [SerializeField] public float minDetectionRange = 5f;
        [SerializeField] LayerMask obstructionLayer;

        Vector3 suspiciousSoundPosition;
        bool heardSuspiciousSound;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
            _ = heardSuspiciousSound;
        }

        void Start()
        {
            hasTarget = false;
            heardSuspiciousSound = false;
        }

        void FixedUpdate()
        {
            if (
                !player.GetComponent<Character.Hiding>().isHiding &&
                CanSeePlayer())
            {
                hasTarget = true;
                target = player;
            }
            else
            {
                hasTarget = false;
                target = null;
            }
        }

        void Update()
        {
        }

        public bool CanSeePlayer()
        {
            return CanSeePosition(player.transform.position);
        }

        public bool CanSeePosition(Vector3 p)
        {
            Vector3 towardPosition = p - transform.position;
            Vector3 forward = transform.forward;

            Vector2 towardPosition2D = new Vector2(towardPosition.x, towardPosition.z).normalized;
            Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z).normalized;

            float angle = Vector2.Angle(towardPosition2D, forward2D);
            // print((angle, FOV/2f));

            bool isInViewRange = Vector3.Distance(transform.position, p) < viewRange;
            bool isInFOV = angle < (FOV / 2f);
            bool isInDirectVision = !Physics.Raycast(transform.position, towardPosition, towardPosition.magnitude, obstructionLayer);
            bool isInMinDetectionRange = Vector3.Distance(transform.position, p) < minDetectionRange;

            return (
                isInMinDetectionRange ||
                (
                    isInViewRange &&
                    isInFOV &&
                    isInDirectVision
                )
            );
        }
    }
}