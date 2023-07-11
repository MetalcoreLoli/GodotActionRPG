using Godot;
using System;

namespace ActionRPG.Scripts.Command;
public partial class MoveCommand : UnitCommand
{
#region Private members
	[ExportGroup("Components")]
	[Export] private MovementComponent _moveComponent = null!;
    [Export] private PathfindingComponent _pathfindingComponent = null!;

    private double _delta;
    private bool _isMoving;
    private Vector2 _direction = Vector2.Zero;
#endregion

#region Properties
    [Export] public Vector2 Destination { get; set; } = Vector2.Zero;
#endregion

#region Public members
    public override void Execute(Unit unit)
    {
        if (unit is null)
            return;

        if ((Destination - unit.GlobalTransform.Origin).LengthSquared() <= 1)
        {
            CurrentState = State.Success;
            return;
        }
        _pathfindingComponent.MovementTarget = Destination;
        var currentAgentPosition = unit.GlobalTransform.Origin;
        Vector2 nextPathPosition = _pathfindingComponent.GetNextPathPosition();
        _direction = (nextPathPosition - currentAgentPosition).Normalized();
        _moveComponent.MoveTo(_direction);
        CurrentState = State.Running;
    }

#endregion
#region Godot stuff

    public override void _Input(InputEvent @event)
	{
		// TODO: change later
		if (@event is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == MouseButton.Left)
		{
            Destination = GetViewport().GetMousePosition();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._Process(delta);
		_delta = delta;
		if (CurrentState == State.Running)
            _moveComponent.MoveTo(_direction);
	}
#endregion
}
