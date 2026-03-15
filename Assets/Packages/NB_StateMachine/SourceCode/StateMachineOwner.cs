using UnityEngine;
using NB_ToolLibrary;
using UnityEditor;
using System;

public class StateMachineOwner : MonoBehaviour
{
    [SerializeField] private MonoScript _initialStateScript; // Get script object since non-monobehaviour abstract class with custom contructor in not serializable
    private StateMachine _stateMachine = new StateMachine();

    private void Start()
    {
        SetInitialState();
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    void SetInitialState()
    {
        Type initialStateClass = _initialStateScript.GetClass(); // Get class from script object
        if (!typeof(BaseState).IsAssignableFrom(initialStateClass))
        {
            //If the selected script object don't contain a class that inherit from BaseState
            Debug.LogError($"{initialStateClass.Name} is not a State script and does not inherit from BaseState");
            return;
        }

        BaseState initialState = (BaseState)Activator.CreateInstance(initialStateClass, new object[] { this }); // Use reflection to create the class instance without knowing its name beforehand
        _stateMachine.ChangeState(initialState);
    }
}
