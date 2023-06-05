using Godot;
using System;

public partial class Weapon: Area2D
{
    [Export] public Vector2 KnockbackVector { get; set; } = Vector2.Zero;
    [Export] public float AttackDistance { get; set; } = 2;
    [Export] public string DamageDice { get; set; }
}
