using UnityEngine;

namespace NB_ToolLibrary
{
    // Based on https://discussions.unity.com/t/c-proper-state-machine/613267/2
    public class StateMachine
    {
        private BaseState _currentState;

        /// <summary>
        /// Change the current state of the state machine, call the Exit() method of the previous state and the Enter() method of the next state.
        /// </summary>
        /// <param name="newState">New instance of the state class to which we want to transition</param>
        public void ChangeState(BaseState newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = newState;
            _currentState.Enter();
        }

        /// <summary>
        /// Call the Update() method of the current state, should only be called by the state machine owner
        /// </summary>
        public void Update()
        {
            if(_currentState == null) return;

            _currentState.Update();
        }



    }

}
