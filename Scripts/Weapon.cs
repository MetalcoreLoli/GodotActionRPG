using Godot;
using System;

public partial class Weapon: Area2D
{
    [Export] public Vector2 KnockbackVector { get; set; } = Vector2.Zero;
    [Export] public int Damage { get; set; } = 2;
}
