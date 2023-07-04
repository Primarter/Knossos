using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColliderMemory : MonoBehaviour
{
    [SerializeField] string[] tags;

    public delegate void TriggerEnter(Collider other);
    public delegate void TriggerExit(Collider other);

    public TriggerEnter OnTriggerIn;
    public TriggerExit OnTriggerOut;

    public List<GameObject> collidingGameObjects = new();

    void OnTriggerEnter(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            collidingGameObjects.Add(other.gameObject);
            OnTriggerIn(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            collidingGameObjects.Remove(other.gameObject);
            OnTriggerOut(other);
        }
    }
}
