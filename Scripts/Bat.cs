using Godot;
using System;
using System.Linq;

public partial class Bat : CharacterBody2D
{
	#region Private Members

	[Export] private Area2D _hurtbox = null!;
	[Export] private float _friction = 200;
	[Export] private float _knockbackConst = 135;

	[Export, ExportGroup("Effects")]
	private EffectSpawner _deathEffectSpawner = null!;

	[Export] private Stats _stats = null!;

	private Vector2 _knockback = Vector2.Zero;

	#endregion
	#region Godot
	public override void _Ready()
	{
		_hurtbox.AreaEntered += _OnHurtbox_AreaEntered;
		_stats.Death += (Node killer) =>
		{
			_deathEffectSpawner?.Spawn();
			QueueFree();
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		_knockback = _knockback.MoveToward(Vector2.Zero, (float)(_friction * delta));
		Velocity = _knockback;
		_ = MoveAndSlide();
	}

	public void _OnHurtbox_AreaEntered(Node area)
	{
		if (area is Weapon weapon)
		{
			_knockback = weapon.KnockbackVector * _knockbackConst;
			_stats.Damage(weapon.Damage, area, this);
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
