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

    [Export] private NavigationAgent2D _navAgent = null!;
    [Export] private Player _player = null!; // TODO: remove later 
    [Export] private Vector2 _movementTargetPosition = Vector2.Zero;

    [Export, ExportGroup("Effects")]
    private EffectSpawner _deathEffectSpawner = null!;

    [Export, ExportGroup("Components")]
    private MovementComponent _movementCompoment = null!;

    [Export, ExportGroup("Components")]
    private HealthComponent _healthComponent = null!;

    private double _delta;
    private Vector2 _knockback = Vector2.Zero;

    private BehaviourTree _behaviourTree = new();

    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        // Now that the navigation map is no longer empty, set the movement target./
        MovementTarget = _movementTargetPosition;
    }

    private Vector2 MovementTarget
    {
        get => _navAgent.TargetPosition;
        set => _navAgent.TargetPosition = value;
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

        // this is so unstable 
        // and i don't know why... sometimes it's work, sometimes not
        var seq = new SequenceNode("Left to right");
        _ = seq
            .Add(new Leaf("Chase player", () =>
                    {
                        MovementTarget = _player.GlobalTransform.Origin;

                        var currentAgentPosition = GlobalTransform.Origin;
                        Vector2 nextPathPosition = _navAgent.GetNextPathPosition();
                        var direction = (nextPathPosition - currentAgentPosition).Normalized();

                        _movementCompoment.Move(_delta, direction);
                        if ((this.GlobalTransform.Origin - _player.GlobalTransform.Origin).Length() <= 1.0f)
                        {
                            return AiActionStatus.Success;
                        }
                        return AiActionStatus.Running;
                    }));

        _ = _behaviourTree.Add(seq);

        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        _navAgent.PathDesiredDistance = 4.0f;
        _navAgent.TargetDesiredDistance = 4.0f;
        // Make sure to not await during _Ready.
        Callable.From(ActorSetup).CallDeferred();
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
