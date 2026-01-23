using UnityEngine;

namespace Game.StateMachine
{
    public abstract class StateManager<T> : MonoBehaviour where T : StateManager<T>
    {
        // Public

        protected State<T> CurrentState { get; private set; }
        public State<T> PreviousState { get; private set; }

        protected virtual void FixedUpdate()
        {
            if (CurrentState == null) return;

            CurrentState.OnUpdate((T)this);
        }

        public void ChangeState(State<T> _newState)
        {
            if (CurrentState != null) CurrentState.OnExit((T) this);

            PreviousState = CurrentState;

            CurrentState = _newState;
            CurrentState.OnEnter((T) this);

            Debug.Log(CurrentState);
        }
    }
}
