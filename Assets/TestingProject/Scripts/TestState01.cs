using UnityEngine;

public class TestState01 : BaseState
{
    public TestState01(StateMachineOwner owner) : base(owner)
    {
        Debug.Log("Sub-class contructor");
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
