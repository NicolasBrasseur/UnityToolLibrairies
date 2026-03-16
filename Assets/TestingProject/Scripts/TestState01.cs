using UnityEngine;

public class TestState01 : BaseState
{
    float time = 0.0f;

    public TestState01(StateMachineOwner owner) : base(owner)
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
            ChangeState(new TestState02(_owner));
            return;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit State 01");
    }

}
