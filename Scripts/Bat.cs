using Godot;
using System;
using System.Linq;

public partial class Bat : CharacterBody2D
{
    #region Private Members

    [Export] private Area2D _hurtbox = null!;
    [Export] private float _friction = 200;
    [Export] private float _knockbackConst = 135;
    [Export] private EffectSpawner _deathEffectSpawner = null!;

    private Vector2 _knockback = Vector2.Zero;

    #endregion
    #region Godot
    public override void _Ready()
    {
        _hurtbox.AreaEntered += _OnHurtbox_AreaEntered;
    }

    public override void _PhysicsProcess(double delta)
	{
        _knockback = _knockback.MoveToward(Vector2.Zero, (float)(_friction * delta));
        Velocity = _knockback;
        _ = MoveAndSlide();
	}

    public void _OnHurtbox_AreaEntered(Area2D area)
    {
        GD.Print(area.GetOverlappingAreas());
        _deathEffectSpawner.Spawn();
        QueueFree();
    }
    #endregion
}
