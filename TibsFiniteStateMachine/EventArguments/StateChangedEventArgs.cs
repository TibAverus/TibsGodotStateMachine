namespace TibsFiniteStateMachine.EventArguments
{
    public class StateChangedEventArgs
    {
        public IFsmState ChangingTo { get; set; }
        public IFsmState ChangingFrom { get; set; }

        public StateChangedEventArgs(IFsmState changingTo,
            IFsmState changingFrom)
        {
            ChangingTo = changingTo;
            ChangingFrom = changingFrom;
        }
    }
}