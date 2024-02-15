namespace StateMachine.EventArguments
{
    public class StateExitedEventArgs
    {
        public IFsmState State { get; set; }

        public StateExitedEventArgs(IFsmState state)
        {
            State = state;
        }
    }
}