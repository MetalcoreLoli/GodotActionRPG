using Godot;
using System.Linq;
using System;
using System.Collections.Generic;
using ActionRPG.Scripts.Ai;
using System.Runtime.CompilerServices;
using ActionRPG.Scripts.Dices;

public partial class Bat : CharacterBody2D
{
#region Private Members
	[Export] private Area2D _hurtbox = null!;
	[Export] private float _friction = 200;
	[Export] private float _knockbackConst = 135;

	[Export] private Player _player = null!; // TODO: remove later 
	[Export] private Vector2 _movementTargetPosition = Vector2.Zero;

	[Export] private Weapon _weapon = null!;

	[ExportGroup("Animations")]
	[Export] private AnimationPlayer _animationPlayer = null!;
	[Export] private AnimationTree _animationTree = null!;

	[Export, ExportGroup("Effects")]
	private EffectSpawner _deathEffectSpawner = null!;

	[ExportGroup("Components")]
	[Export] private HealthComponent _healthComponent = null!;
	[Export] private MovementComponent _movementCompoment = null!;
	[Export] private PathfindingComponent _pathfindingComponent = null!;

	private double _delta;
	private Vector2 _knockback = Vector2.Zero;

    private State _currentState = State.Idle;

	private AnimationNodeStateMachinePlayback _currentAnimationState = null!;

	private BehaviourTree _behaviourTree = new();

#region States
    private enum State
    {
        Idle, Fly, Attack
    }

    private void IdleState()
    {
        _currentAnimationState?.Travel("Idle");
    }

    private void FlyState()
    {
        _pathfindingComponent.MovementTarget = _player.GlobalTransform.Origin;

        var currentAgentPosition = GlobalTransform.Origin;
        Vector2 nextPathPosition = _pathfindingComponent.GetNextPathPosition();
        var direction = (nextPathPosition - currentAgentPosition).Normalized();

        _movementCompoment.Move(_delta, direction);
    }

    private void AttackState()
    {
        _currentAnimationState?.Travel("Attack");
    }
#endregion

    private IAiActionNode Behaviour()
    {
        var selector = new SelectorNode("Chase player");
        // this is so unstable 
        // and i don't know why... sometimes it's work, sometimes not
        var seq = new SequenceNode("Left to right");

        var chasePlayer = new Leaf("Chase player", () =>
                    {
                        if (CanAttack(_player.GlobalTransform.Origin, this.GlobalTransform.Origin, _weapon.AttackDistance))
                        {
                            GD.Print("player was caught");
                            return AiActionStatus.Success;
                        }
                        _currentState = State.Fly;
                        return AiActionStatus.Running;
                    });
        Leaf attackPlayer = new Leaf("Attack player", () =>
                        {
                            if (!CanAttack(_player.GlobalTransform.Origin, this.GlobalTransform.Origin, _weapon.AttackDistance))
                            {
                                GD.Print("attack failed");
                                return AiActionStatus.Failure;
                            }
                            _currentState = State.Attack;
                            return AiActionStatus.Success;
                        });
        _ = selector.Add(attackPlayer).Add(chasePlayer);
        return selector;
    //    return seq.Add(chasePlayer).Add(selector);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool CanAttack(Vector2 playersOrigin, Vector2 batsOrigin, float weaponAttackDistance)
    {
        return (batsOrigin - playersOrigin).LengthSquared() <= weaponAttackDistance * weaponAttackDistance;
    }

#endregion
#region Godot
    public override void _Ready()
	{
		_currentAnimationState = (AnimationNodeStateMachinePlayback)(_animationTree?.Get("parameters/playback"));

		_hurtbox.AreaEntered += _OnHurtbox_AreaEntered;
		_healthComponent.OnDeath += (Node killer) =>
		{
			_deathEffectSpawner?.Spawn();
			QueueFree();
		};

		_movementCompoment.OnMove += (direction) =>
		{
			_animationTree?.Set("parameters/Attack/blend_position", direction);
			_animationTree?.Set("parameters/Idle/blend_position", direction);
			_animationTree?.Set("parameters/Fly/blend_position", direction);
			_currentAnimationState?.Travel("Fly");
		};
		_ = _behaviourTree.Add(Behaviour());

	}

    public override void _PhysicsProcess(double delta)
    {
        _delta = delta;
        _knockback = _knockback.MoveToward(Vector2.Zero, (float)(_friction * delta));
        Velocity = _knockback;

        if (!_behaviourTree.IsEmpty)
            _ = _behaviourTree.Execute();

        switch (_currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Fly:
                FlyState();
                break;
            case State.Attack:
                AttackState();
                break;
        }
    }

    public void _OnHurtbox_AreaEntered(Node area)
    {
        if (area is Weapon weapon)
        {
            _knockback = weapon.KnockbackVector * _knockbackConst;
            int damage = (int)DiceRoller.Roll(weapon.DamageDice);
            _healthComponent.TakeDamage(damage, area, this);
            GD.Print(damage);
        }
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

    public void _OnAttackAnimationFinished()
    {
        _currentState = State.Idle;
    }
#endregion
}
