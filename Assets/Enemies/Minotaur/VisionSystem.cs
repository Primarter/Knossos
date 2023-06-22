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

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        void Start()
        {
            hasTarget = false;
        }

        void FixedUpdate()
        {
            if (!player.GetComponent<Hiding>().isHidding &&
                CanSeePosition(player.transform.position))
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

        public bool CanSeePosition(Vector3 p)
        {
            Vector3 towardPosition = p - transform.position;
            Vector3 forward = transform.forward;

            Vector2 towardPosition2D = new Vector2(towardPosition.x, towardPosition.z).normalized;
            Vector2 forward2D = new Vector2(transform.forward.x, transform.forward.z).normalized;

            float angle = Vector2.Angle(towardPosition2D, forward2D);

            print((angle, FOV/2f));

            return (
                Vector3.Distance(transform.position, p) < viewRange &&
                angle < (FOV / 2f)
            );
        }

        void Update()
        {

        }
    }
}
