using Godot;

namespace TibsFiniteStateMachine
{
    public interface IFsmState
    {
        public void Enter(Godot.Collections.Dictionary<string, Variant> message);
        public void Process();
        public void Update(double dt);
        public void Exit(IFsmState nextState);
    }
}