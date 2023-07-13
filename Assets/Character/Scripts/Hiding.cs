using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Character
{

public class Hiding : MonoBehaviour
{
    [SerializeField] string bushTag;

    public bool isHiding;

    void Start()
    {
       isHiding = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == bushTag)
        {
            isHiding = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == bushTag)
        {
            isHiding = false;
        }
    }
}

}