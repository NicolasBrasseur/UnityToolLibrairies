using NB_ToolLibrary;
using UnityEngine;

public class TestState02 : BaseState
{
    public TestState02(StateMachineOwner owner) : base(owner)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter State 02");
    }

    public override void Update()
    {

    }


}
