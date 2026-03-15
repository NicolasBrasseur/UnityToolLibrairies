using UnityEngine;

namespace NB_ToolLibrary
{
    // Based on https://discussions.unity.com/t/c-proper-state-machine/613267/2
    public class StateMachine
    {
        private BaseState _currentState;

        public void ChangeState(BaseState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            if(_currentState == null) return;

            _currentState.Update();
        }



    }

}
