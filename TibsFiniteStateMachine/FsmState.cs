using System;
using System.Diagnostics;
using Godot;

namespace TibsFiniteStateMachine
{
    public partial class FsmState : Node, IFsmState
    {
        private Stopwatch _timer;
        private bool _hasBeenInitialized;
        private bool _onUpdateHasFired;

        public virtual void Enter(Godot.Collections.Dictionary<string, Variant> message = null)
        {
            _timer ??= new Stopwatch();
            
            _hasBeenInitialized = true;
            _timer.Reset();
            _timer.Start();
        }

        public virtual void Process()
        {
            if (!_hasBeenInitialized) return;

            _onUpdateHasFired = true;
        }

        public virtual void Update(double dt)
        {
            if (!_onUpdateHasFired) return;
        }

        public virtual void Exit(IFsmState nextState)
        {
            if (!_hasBeenInitialized) return;
            _timer.Stop();

            _hasBeenInitialized = false;
            _onUpdateHasFired = false;
        }
        
        public long GetTimeInCurrentState(TimeInStateFormat type = TimeInStateFormat.Milliseconds)
        {
            return type switch
            {
                TimeInStateFormat.Milliseconds => _timer.ElapsedMilliseconds,
                TimeInStateFormat.Seconds => _timer.Elapsed.Seconds,
                TimeInStateFormat.Ticks => _timer.ElapsedTicks,
                _ => throw new ArgumentOutOfRangeException(nameof(type),
                    type, "Invalid type")
            };
        }
        
        public enum TimeInStateFormat
        {
            Milliseconds,
            Seconds,
            Ticks
        }
    }
}