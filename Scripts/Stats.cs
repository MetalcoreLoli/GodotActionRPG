using Godot;
using System;

public partial class Stats : Node
{
    [Export] public int MaxHealth { get; set; } = 10;
    [Export] public int Health { get; set; } = 10;
}
