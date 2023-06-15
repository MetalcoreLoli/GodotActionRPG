using ActionRPG.Scripts.Command;
using Godot;
using System;

public partial class UnitCommandsComponent : Node2D
{
#region Private members
    [Export] private MoveCommand _move = null!;
#endregion

#region Godot stuff
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
#endregion
}
