using Godot;
using System;
using System.Linq;

public partial class Player : Node2D
{
#region Private members
    [Export] private UnitSelector _unitSelector = null!;

    private void ExecuteUnitCommand()
    {
        foreach (var unit in _unitSelector.Selected.OfType<Unit>())
        {
            unit.CommandsComponent.Execute();
        }
    }
#endregion

#region Godot stuff
    public override void _UnhandledInput(InputEvent @event)
    {
        // TODO: change later
        if (@event is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == MouseButton.Left)
        {
            ExecuteUnitCommand();
        }
    }
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
