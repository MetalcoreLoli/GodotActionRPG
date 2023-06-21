using Godot;
using System;

public partial class MovementComponent : Node2D
{
#region Private members
	[Export] private float _speed = 80.0f;
	[Export] private float _acceleration = 500.0f;
	[Export] private float _friction = 500.0f;

    [Export] private CharacterBody2D _movableCharacter;

	[Export(PropertyHint.Range, "-1,1")]
	private Vector2 _currentDirection = Vector2.Zero;
    private float _delta;

    private void Move()
	{
		_ = _movableCharacter.MoveAndSlide();
	}
#endregion

#region Properties
	public float Speed => _speed;
	public float Friction => _friction;
	public float Acceleration => _acceleration;

#endregion

#region Signals
    [Signal] public delegate void OnMoveEventHandler(Vector2 direction);
    [Signal] public delegate void OnStopEventHandler();
#endregion

#region Public Methods

    public void MoveTo(Vector2 desination)
    {
        var velocity = _movableCharacter.Velocity;
        var direction = desination.Normalized();
        if (direction != Vector2.Zero)
        {
            _currentDirection = direction.Normalized();
            EmitSignal(nameof(OnMove), _currentDirection);
            velocity = Position.DirectionTo(desination) * Speed;//velocity.MoveToward(desination * Speed, Acceleration * _delta);
        }
        else
        {
            EmitSignal(nameof(OnStop));
            velocity = velocity.MoveToward(Vector2.Zero, Friction * _delta);
        }
        _movableCharacter.Velocity = velocity;
        _ = _movableCharacter.MoveAndSlide();
    }

	public void Move(double delta, Vector2 direction)
	{
		Vector2 velocity = _movableCharacter.Velocity;
		if (direction != Vector2.Zero)
		{
			_currentDirection = direction;
            EmitSignal(nameof(OnMove), _currentDirection);
			velocity = velocity.MoveToward(direction * Speed, Acceleration * (float)delta);
		}
		else
		{
            EmitSignal(nameof(OnStop));
			velocity = velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
		}

		_movableCharacter.Velocity = velocity;
        _ = _movableCharacter.MoveAndSlide();
	}

#endregion
#region Godot Stuff
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    public override void _PhysicsProcess(double delta)
    {
        _delta = (float)delta;
    }
#endregion
}
