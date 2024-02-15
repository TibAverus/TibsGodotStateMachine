namespace TibsFiniteStateMachine.EventArguments
{
    public class StateEnteredEventArgs
    {
        public IFsmState State { get; set; }

        public StateEnteredEventArgs(IFsmState state)
        {
            State = state;
        }
    }
}