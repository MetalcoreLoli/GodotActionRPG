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

	[Export, ExportGroup("Effects")]
	private EffectSpawner _deathEffectSpawner = null!;

	[Export, ExportGroup("Components")]
	private MovementComponent _movementCompoment = null!;

	[Export, ExportGroup("Components")]
	private HealthComponent _healthComponent = null!;

	private double _delta;
	private Vector2 _knockback = Vector2.Zero;

	private BehaviourTree _behaviourTree = new();

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

        // this is so unstable 
        // and i don't know why... sometimes it's work, sometimes not
        var seq = new SequenceNode("Left to right");
        _ = seq
            .Add(new Leaf("go to right", () =>
                    {
                        _movementCompoment.Move(_delta, Vector2.Up);
                        return AiActionStatus.Success;
                    }))
            .Add(new Leaf("go to left", () =>
                    {
                        _movementCompoment.Move(_delta, Vector2.Left);
                        return AiActionStatus.Success;
                    }));

        _ = _behaviourTree.Add(seq);

    }

	public override void _PhysicsProcess(double delta)
	{
		_delta = delta;
		_knockback = _knockback.MoveToward(Vector2.Zero, (float)(_friction * delta));
		Velocity = _knockback;
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
