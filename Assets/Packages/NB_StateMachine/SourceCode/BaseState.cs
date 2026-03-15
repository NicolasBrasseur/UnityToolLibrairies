using NB_ToolLibrary;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachineOwner _owner;
    protected StateMachine _stateMachine;

    public BaseState(StateMachineOwner owner, StateMachine stateMachine)
    {
        _owner = owner;
        _stateMachine = stateMachine;
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
