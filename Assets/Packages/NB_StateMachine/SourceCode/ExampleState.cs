using UnityEngine;

public class ExampleState : BaseState
{
    public ExampleState(StateMachineOwner owner) : base(owner)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter");
    }

    public override void Update()
    {
        Debug.Log("Update");
    }

    public override void Exit()
    {
        Debug.Log("Exit");
    }

}
