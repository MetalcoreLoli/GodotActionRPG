using System;
using Godot;

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


	[Export] private AnimationTree _animationTree = null!;
	[Export] private AnimationPlayer _animationPlayer = null!;

	[Export, ExportGroup("Components")]
	private HealthComponent _healthComponent = null!;

	[Export, ExportGroup("Components")]
	private MovementComponent _movementComponent = null!;

	private AnimationNodeStateMachinePlayback _currentAnimationState = null!;

#region State Implementations
	//TODO: move into a its own class
	private void MoveState(double delta)
	{
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		_movementComponent.Move(delta, direction);

		// trigger for transition to the AttackState
		if (Input.IsActionJustPressed("Attack"))
		{
			_currentState = State.Attack;
		}
		
		// trigger for transition to the RollState
		// TODO: Roll 
		//if (Input.IsActionJustPressed("Roll"))
		//{
		//	_currentState = State.Roll;
		//}
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
        _healthComponent.OnDeath += (Node killer) =>
        {
            GD.Print("Player died");
            //_deathEffectSpawner?.Spawn();
            //QueueFree();
        };
        _animationTree.Active = true;
        _currentAnimationState = (AnimationNodeStateMachinePlayback)(_animationTree?.Get("parameters/playback"));
        //_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

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

	public void _On_HurtBox_AreaEntered(Area2D area)
	{
		if (area is Weapon weapon)
		{
			//_knockback = weapon.KnockbackVector * _knockbackConst;
			_healthComponent.TakeDamage(weapon.Damage, area, this);
			GD.Print($"-{weapon.Damage}");
		}
	}

	public void _OnMovementComponent_PlayerStop()
	{
		_currentAnimationState?.Travel("Idle");
	}

    public void _OnTakeDamage(int damage, Node from, Node to)
    {
        if (from is null)
        {
            throw new ArgumentNullException(nameof(from));
        }

        if (to is null)
        {
            throw new ArgumentNullException(nameof(to));
        }

        GD.Print("-" + damage);
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
