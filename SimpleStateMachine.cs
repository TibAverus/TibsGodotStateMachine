using Godot;
using System.Linq;
using System.Collections.Generic;
using System;

public partial class SimpleStateMachine : Node
{
	[Signal] public delegate void PreStartEventHandler();
	[Signal] public delegate void PostStartEventHandler();
	[Signal] public delegate void PreExitEventHandler();
	[Signal] public delegate void PostExitEventHandler();

	public List<SimpleState> States;

	public string CurrentState;

	public string LastState;

	protected SimpleState state = null;

    public override void _Ready()
    {
        base._Ready();

        AddUserSignal(nameof(PreStartEventHandler));
        AddUserSignal(nameof(PostStartEventHandler));
        AddUserSignal(nameof(PreExitEventHandler));
        AddUserSignal(nameof(PostExitEventHandler));

        States = GetNode<Node>("States").GetChildren().OfType<SimpleState>().ToList();
    }

	private void SetState(SimpleState _state, Dictionary<string, string> message)
	{
		if (_state == null) return;

		if (state != null)
		{
			if (_state.Name == CurrentState) return;
			
			EmitSignal(nameof(PreExitEventHandler));
			state.OnExit(_state.GetType().ToString());
			EmitSignal(nameof(PostExitEventHandler));
		}

		LastState = CurrentState;
		CurrentState = _state.GetType().ToString();

		state = _state;
		EmitSignal(nameof(PreStartEventHandler));
		state.OnStart(message);
		EmitSignal(nameof(PostStartEventHandler));
		state.OnUpdate();
	}

	public void ChangeState(string stateName, Dictionary<string, string> message = null)
	{
		foreach (SimpleState _state in States)
		{
			if (stateName == _state.GetType().ToString())
			{
				SetState(_state, message);
				return;
			}
		}
	}

    public override void _Process(double delta)
    {
		if (state == null) return;

		state.UpdateState(delta);
    }
}
