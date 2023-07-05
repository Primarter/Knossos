using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] string[] tags;

    public delegate void TriggerEnter(Collider other);
    public delegate void TriggerExit(Collider other);

    public TriggerEnter OnTriggerIn;
    public TriggerExit OnTriggerOut;

    [HideInInspector] public List<GameObject> collidingGameObjects = new();

    public GameObject[] getColliding()
    {
        return collidingGameObjects.ToArray();
    }

    void OnTriggerEnter(Collider other)
    {
        // print((other.tag, other.gameObject.tag));
        if (tags.Contains(other.tag))
        {
            // print("ENTER");
            collidingGameObjects.Add(other.gameObject);
            if (OnTriggerIn != null)
                OnTriggerIn(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            // print("EXIT");
            collidingGameObjects.Remove(other.gameObject);
            if (OnTriggerOut != null)
                OnTriggerOut(other);
        }
    }
}
