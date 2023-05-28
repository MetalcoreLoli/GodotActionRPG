using Godot;
using System.Linq;
using System;
using System.Collections.Generic;
using ActionRPG.Scripts.Ai;

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

	private AnimationNodeStateMachinePlayback _currentAnimationState = null!;

	private BehaviourTree _behaviourTree = new();

	private IAiActionNode Behaviour()
	{
        var selector = new SelectorNode("Chase player");
        // this is so unstable 
        // and i don't know why... sometimes it's work, sometimes not
        var seq = new SequenceNode("Left to right");

        var chasePlayer = new Leaf("Chase player", () =>
                    {
                        GD.Print("chasing player");
                        var dist = (this.GlobalTransform.Origin - _player.GlobalTransform.Origin).LengthSquared();
                        if (dist <= _weapon.AttackDistance * _weapon.AttackDistance)
                        {
                            GD.Print("player was caught");
                            return AiActionStatus.Success;
                        }

                        _pathfindingComponent.MovementTarget = _player.GlobalTransform.Origin;

                        var currentAgentPosition = GlobalTransform.Origin;
                        Vector2 nextPathPosition = _pathfindingComponent.GetNextPathPosition();
                        var direction = (nextPathPosition - currentAgentPosition).Normalized();

                        _movementCompoment.Move(_delta, direction);
                        return AiActionStatus.Running;
                    });
        Leaf attackPlayer = new Leaf("Attack player", () =>
                        {
                            var dist = (this.GlobalTransform.Origin - _player.GlobalTransform.Origin).LengthSquared();
                            if (dist > _weapon.AttackDistance * _weapon.AttackDistance)
                            {
                                GD.Print("attack failed");
                                return AiActionStatus.Failure;
                            }
                            GD.Print("attacking player");
                            _currentAnimationState?.Travel("Attack");
                            return AiActionStatus.Success;
                        });
        _ = selector.Add(attackPlayer).Add(chasePlayer);
        return selector;
    //    return seq.Add(chasePlayer).Add(selector);
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
	}

	public void _OnHurtbox_AreaEntered(Node area)
	{
		if (area is Weapon weapon)
		{
			_knockback = weapon.KnockbackVector * _knockbackConst;
			_healthComponent.TakeDamage(weapon.Damage, area, this);
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
#endregion
}
