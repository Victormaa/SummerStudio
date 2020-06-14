namespace StateMachine
{
    public class StateMachine<T>
    {
        public State<T> CurrentState { get; private set; }
        public T Owner;

        public StateMachine(T _owner)
        {
            Owner = _owner;
            CurrentState = null;
        }

        public void ChangeState(State<T> _newState)
        {
            if (CurrentState != null)
                CurrentState.onExit(Owner);
            CurrentState = _newState;
            CurrentState.onEnter(Owner);
        }

        public void Update()
        {
            if (CurrentState != null)
                CurrentState.onUpdate(Owner);
        }

    }

    public abstract class State<T>
    {
        public abstract void onEnter(T _owner);
        public abstract void onUpdate(T _owner);
        public abstract void onExit(T _owner);
    }
}