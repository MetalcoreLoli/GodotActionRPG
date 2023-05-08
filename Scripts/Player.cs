using Godot;
using System;

public partial class Player : CharacterBody2D
{
#region Private members
	[Export] private float _rollSpeed = 0.5f;

	[Export(PropertyHint.Range, "-1,1")]
	private Vector2 _currentDirection = Vector2.Zero;

	[Export(PropertyHint.Flags, "Idle,Attack,Move,Roll")]
	private State _currentState = State.Idle;

	//TODO: create weaponhitbox class 
	[Export] private Weapon _weapon = null!;

	[Export] private Stats _stats = null!;

	[Export] private AnimationTree _animationTree = null!;
	[Export] private AnimationPlayer _animationPlayer = null!;
    [Export] private MovementComponent _movementComponent = null!;

	private AnimationNodeStateMachinePlayback _currentAnimationState = null!;

	private void Move()
	{
		_ = MoveAndSlide();
	}

#region State Implementations
	//TODO: move into a its own class
	private void MoveState(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        _movementComponent.Move(delta, direction);
		//if (direction != Vector2.Zero)
		//{
		//	_currentDirection = direction;
		//	if (_weapon is not null)
		//		_weapon.KnockbackVector = _currentDirection;

		//	_animationTree?.Set("parameters/Idle/blend_position", direction);
		//	_animationTree?.Set("parameters/Attack/blend_position", direction);
		//	_animationTree?.Set("parameters/Run/blend_position", direction);
		//	_animationTree?.Set("parameters/Roll/blend_position", direction);

		//	_currentAnimationState?.Travel("Run");
		//	velocity = velocity.MoveToward(direction * Speed, Acceleration * (float)delta);
		//}
		//else
		//{
		//	_currentAnimationState?.Travel("Idle");
		//	velocity = velocity.MoveToward(Vector2.Zero, Friction * (float)delta);
		//}

		//Velocity = velocity;
		//Move();

		// trigger for transition to the AttackState
		if (Input.IsActionJustPressed("Attack"))
		{
			_currentState = State.Attack;
		}
		
		// trigger for transition to the RollState
		if (Input.IsActionJustPressed("Roll"))
		{
			_currentState = State.Roll;
		}
	}

	private void AttackState(double delta)
	{

		Velocity = Vector2.Zero;
		_currentAnimationState?.Travel("Attack");
	}

	private void RollState(double delta)
	{
		//Velocity = _currentDirection * RollSpeed;
		//_currentAnimationState?.Travel("Roll");
        //_movementComponent.Move(delta, _currentDirection * RollSpeed);
	}
#endregion
#endregion

#region Properties
	public float RollSpeed => _rollSpeed;
#endregion

#region Godot Stuff
	public override void _Ready()
	{
		_animationTree.Active = true;
		_currentAnimationState = (AnimationNodeStateMachinePlayback)(_animationTree?.Get("parameters/playback"));
		//_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	// public float Gravity => ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		switch (_currentState)
		{
			case State.Attack:
				AttackState(delta);
				break;
			case State.Idle: // handled inside of the MoveState method
			case State.Move:
				MoveState(delta);
				break;
			case State.Roll:
				RollState(delta);
				break;
			default:
				break;
		}
	}

	public void _OnArea2D_AreaEntered(Area2D area)
	{
	}

    public void _OnMovementComponent_PlayerMove(Vector2 direction)
    {
        if (_weapon is not null)
            _weapon.KnockbackVector = direction;

        _animationTree?.Set("parameters/Idle/blend_position", direction);
        _animationTree?.Set("parameters/Attack/blend_position", direction);
        _animationTree?.Set("parameters/Run/blend_position", direction);
        _animationTree?.Set("parameters/Roll/blend_position", direction);

        _currentAnimationState?.Travel("Run");
    }

    public void _OnMovementComponent_PlayerStop()
    {
        _currentAnimationState?.Travel("Idle");
    }

	// this method will be called at the end of attack animation
	public void RollAnimationFinished()
	{
		_currentState = State.Idle;
	}

	// this method will be called at the end of attack animation
	public void AttackAnimationFinished()
	{
		_currentState = State.Idle;
	}
#endregion

	[Flags]
	public enum State: byte
	{
		Idle = 1 << 1,
		Attack= 1 << 2,
		Move= 1 << 3,
		Roll= 1 << 4
	}
}
