using Godot;
using GodotActionRpg.Scripts.Ai;
using System.Linq;
using System;

public partial class Bat : CharacterBody2D
{
	#region Private Members

	[Export] private Area2D _hurtbox = null!;
	[Export] private float _friction = 200;
	[Export] private float _knockbackConst = 135;


	[Export, ExportGroup("Effects")]
	private EffectSpawner _deathEffectSpawner = null!;

    [Export] private MovementComponent _movementCompoment = null!;

	[Export] private Stats _stats = null!;

    private double _delta;
    private Vector2 _knockback = Vector2.Zero;

    private BehaviourTree _behaviourTree = new();

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

        var goTo = new Leaf(() =>
                {
                    _movementCompoment.Move(_delta, new []{Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right}[GD.RandRange(0, 3)]);
                    return AiActionStatus.Succeded;
                })
        {
            Name = "goTo"
        };

        _ = _behaviourTree.Add(goTo);
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
