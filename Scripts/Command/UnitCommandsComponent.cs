using ActionRPG.Scripts.Command;
using Godot;
using System;

public partial class UnitCommandsComponent : Node2D
{
#region Private members
    [Export] private MoveCommand _moveCommand = null!;
#endregion

#region Properties
    ///<summary>
    /// Current selected command that will be executed
    /// By default that command is MoveCommand
    ///</summary>
    public UnitCommand SelectedCommand { get; private set; }
#endregion

#region Public members
    ///<summary>
    /// Executing SelectedCommand
    ///</summary>
    public void Execute()
    {
        SelectedCommand?.Execute();
    }
#endregion

#region Godot stuff
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SelectedCommand = _moveCommand;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
#endregion
}
