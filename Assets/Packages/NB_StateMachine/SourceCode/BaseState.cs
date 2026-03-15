using UnityEngine;

public abstract class BaseState
{
    private StateMachineOwner _owner;

    public BaseState(StateMachineOwner owner)
    {
        _owner = owner;
        Debug.Log("Base class contructor");
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }
}
