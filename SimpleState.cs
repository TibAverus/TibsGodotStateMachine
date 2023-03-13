using Godot;
using System.Collections.Generic;

public partial class SimpleState : Node
{
	private bool HasBeenInitialized = false;
	private bool OnUpdateHasFired = false;

	[Signal] public delegate void StateStartEventHandler();
	[Signal] public delegate void StateUpdatedEventHandler();
	[Signal] public delegate void StateExitedEventHandler();

    public override void _Ready()
    {
        base._Ready();

		AddUserSignal(nameof(StateStartEventHandler));
		AddUserSignal(nameof(StateUpdatedEventHandler));
		AddUserSignal(nameof(StateExitedEventHandler));
    }


    public virtual void OnStart(Dictionary<string, string> message)
	{
		EmitSignal(nameof(StateStartEventHandler));
		HasBeenInitialized = true;
	}

	public virtual void OnUpdate()
	{
		if (!HasBeenInitialized) return;

		EmitSignal(nameof(StateUpdatedEventHandler));
		OnUpdateHasFired = true;
	}

	public virtual void UpdateState(double dt)
	{
		if (!OnUpdateHasFired) return;
	}

	public virtual void OnExit(string nextState)
	{
		if (!HasBeenInitialized) return;

		EmitSignal(nameof(StateExitedEventHandler));
		HasBeenInitialized = false;
		OnUpdateHasFired = false;
	}
}
