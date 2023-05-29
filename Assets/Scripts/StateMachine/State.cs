using UnityEngine;

public abstract class State
{
    public GameObject obj;
    int id;

    // Called when State is registered
    public abstract void Init();

    // Called when State become the active
    public abstract void Enter();

    // call each Update
    public abstract void Update();

    // call each FixedUpdate
    public abstract void FixedUpdate();

    // Called when State become the active
    public abstract void Exit();
}
