using Godot;
using System;

public partial class WeaponHitbox : Area2D
{
    public Vector2 KnockbackVector { get; set; } = Vector2.Zero;
}
