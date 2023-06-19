using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiding : MonoBehaviour
{
    [SerializeField] string bushTag;

    public bool isHidding;

    void Start()
    {
       isHidding = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == bushTag)
        {
            isHidding = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == bushTag)
        {
            isHidding = false;
        }
    }
}
