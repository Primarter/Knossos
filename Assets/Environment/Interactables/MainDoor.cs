using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Knossos.Interaction
{

[RequireComponent(typeof(Interactable))]
public class MainDoor : MonoBehaviour
{
    [SerializeField] Door[] doors = new Door[4];

    int sigilsToCollect;

    private void Awake()
    {
        GetComponent<Interactable>().onInteractCallbacks += TryUseSigil;
    }

    private void Start()
    {
        sigilsToCollect = doors.Length;
    }

    public void TryUseSigil(GameObject player)
    {
        Character.SigilManager sm = player.GetComponent<Character.SigilManager>();
        if (sm != null && sm.hasSigil)
        {
            Debug.Log("Used sigil");
            doors[doors.Length - sigilsToCollect].OpenDoor();
            sigilsToCollect -= 1;
            sm.hasSigil = false;
            if (sigilsToCollect == 0)
                Debug.Log("Congrats, you got out!");
        }
    }
}

}