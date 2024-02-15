using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using TibsFiniteStateMachine.EventArguments;

namespace TibsFiniteStateMachine
{
	public partial class FiniteStateMachine : Node
	{
		public List<IFsmState> States;

		public IFsmState CurrentState;

		public IFsmState LastState;

		public delegate void StateChangedEventHandler(object sender,
			StateChangedEventArgs e);

		public delegate void StateEnteredEventHandler(object sender,
			StateEnteredEventArgs e);

		public delegate void StateExitedEventHandler(object sender,
			StateExitedEventArgs e);

		public event StateChangedEventHandler StateChanged;
		public event StateEnteredEventHandler StateEntered;
		public event StateExitedEventHandler StateExited;

		private IFsmState _activeState;

		public override void _Ready()
		{
			base._Ready();

			Node statesContainerNode = GetNode<Node>("States");
			
			if (statesContainerNode is null)
				throw new Exception(
					"States container doesn't exist!" +
					" Please store the states in a node called \"States\".");
			
			States = statesContainerNode.GetChildren().OfType<IFsmState>().ToList();
		}

		private void SetState(IFsmState nextState, Godot.Collections.Dictionary<string, Variant> message = null)
		{
			if (nextState is null) return;

			if (_activeState is not null)
			{
				if (nextState.Equals(CurrentState)) return;

				_activeState.Exit(nextState);
				StateExited?.Invoke(this, new StateExitedEventArgs(_activeState));
			}
			
			LastState = CurrentState;
			CurrentState = nextState;

			_activeState = nextState;
			_activeState.Enter(message);
			StateEntered?.Invoke(this, new StateEnteredEventArgs(_activeState));
			_activeState.Process();
			
			StateChanged?.Invoke(this, new StateChangedEventArgs(
				CurrentState, LastState));
		}

		public void ChangeState(IFsmState nextState, Godot.Collections.Dictionary<string, Variant> message = null)
		{
			if (!States.Contains(nextState)) return;
			
			SetState(nextState, message);
		}

		public override void _PhysicsProcess(double delta)
		{
			_activeState?.Update(delta);
		}
	}
}