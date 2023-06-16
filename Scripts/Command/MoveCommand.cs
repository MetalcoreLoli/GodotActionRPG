using Godot;
using System;

namespace ActionRPG.Scripts.Command;
public partial class MoveCommand : UnitCommand
{
#region Private members
	[Export] private Node2D _agent = null!;

	[ExportGroup("Components")]
	[Export] private MovementComponent _moveComponent = null!;
	[Export] private PathfindingComponent _pathfindingComponent = null!;

	private double _delta;
	private bool _isMoving;
	#endregion

	#region Properties
	[Export] public Vector2 Destination { get; set; } = Vector2.Zero;
#endregion

#region Public members
	public override void Execute()
	{
		_pathfindingComponent.MovementTarget = Destination;
		var currentAgentPosition = _agent.GlobalTransform.Origin;
		Vector2 nextPathPosition = _pathfindingComponent.GetNextPathPosition();
		var direction = (nextPathPosition - currentAgentPosition).Normalized();
		_isMoving = true;
		if ((Destination - _agent.GlobalTransform.Origin).LengthSquared() <= 1)
			_isMoving = false;
		else 
			_moveComponent.Move(_delta, direction);
	}
	#endregion

#region Godot stuff
	public override void _UnhandledInput(InputEvent @event)
	{
		// TODO: change later
		if (@event is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == MouseButton.Left)
		{
			Destination = buttonEvent.Position;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._Process(delta);
		_delta = delta;
		if (_isMoving && Destination != _agent.GlobalTransform.Origin)
			Execute();
	}
#endregion
}
