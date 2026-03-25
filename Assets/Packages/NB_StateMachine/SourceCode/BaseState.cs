using NB_ToolLibrary;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachineOwner _owner;
    protected ScriptableObject _entityData;
    protected SceneRefHolder _sceneReferences;

    public BaseState(StateMachineOwner owner)
    {
        _owner = owner;
        _entityData = _owner.EntityData;
        _sceneReferences = _owner.SceneReferences;
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

    /// <summary>
    /// Change the current state of the state machine, call the Exit() method of the previous state and the Enter() method of the next state.
    /// </summary>
    /// <param name="newState">New instance of the state class to which we want to transition</param>
    protected void ChangeState(BaseState newState)
    {
        _owner.StateMachine.ChangeState(newState);
    }
}
