using NB_ToolLibrary;
using UnityEngine;

public class TestState01 : BaseState
{
    float time = 0.0f;

    public TestState01(StateMachineOwner owner, StateMachine stateMachine) : base(owner, stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("Enter State 01");
        time = 0.0f;
    }

    public override void Update()
    {
        time += Time.deltaTime;

        if(time >= 5.0f)
        {
            _stateMachine.ChangeState(new TestState02(_owner, _stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit State 01");
    }

}
