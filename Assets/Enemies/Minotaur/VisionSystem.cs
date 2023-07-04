using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Minotaur
{
    public class VisionSystem : MonoBehaviour
    {
        GameObject player;

        public bool hasTarget = false;
        GameObject target;

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
            float detectionRange = 15f;

            if (!player.GetComponent<Character.Hiding>().isHidding &&
                Vector3.Distance(transform.position, player.transform.position) < detectionRange)
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
    }
}
