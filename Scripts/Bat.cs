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

    [Export, ExportGroup("Effects")]
    private EffectSpawner _deathEffectSpawner = null!;

    [ExportGroup("Components")]
    [Export] private HealthComponent _healthComponent = null!;
    [Export] private MovementComponent _movementCompoment = null!;
    [Export] private PathfindingComponent _pathfindingComponent = null!;

    private double _delta;
    private Vector2 _knockback = Vector2.Zero;

    private BehaviourTree _behaviourTree = new();

    private IAiActionNode Behaviour()
    {
        // this is so unstable 
        // and i don't know why... sometimes it's work, sometimes not
        var seq = new SequenceNode("Left to right");
        _ = seq
            .Add(new Leaf("Chase player", () =>
                    {
                        _pathfindingComponent.MovementTarget = _player.GlobalTransform.Origin;

                        var currentAgentPosition = GlobalTransform.Origin;
                        Vector2 nextPathPosition = _pathfindingComponent.GetNextPathPosition();
                        var direction = (nextPathPosition - currentAgentPosition).Normalized();

                        _movementCompoment.Move(_delta, direction);
                        if ((this.GlobalTransform.Origin - _player.GlobalTransform.Origin).Length() <= 5.0f)
                        {
                            return AiActionStatus.Success;
                        }
                        return AiActionStatus.Running;
                    }))
            .Add(new Leaf("Attack player", () =>
                        {
                            GD.Print("Bat attacking player");
                            return AiActionStatus.Success;
                        }));
        return seq;
    }

#endregion
#region Godot
    public override void _Ready()
    {
        _hurtbox.AreaEntered += _OnHurtbox_AreaEntered;
        _healthComponent.OnDeath += (Node killer) =>
        {
            _deathEffectSpawner?.Spawn();
            QueueFree();
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
