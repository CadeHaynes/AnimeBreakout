using UnityEngine;

namespace Game.StateMachine
{
    public abstract class State<T> : ScriptableObject where T : StateManager<T>
    {
        public abstract void OnEnter(T sm);
        
        public abstract void OnUpdate(T sm);

        public abstract void OnExit(T sm);
    }
}