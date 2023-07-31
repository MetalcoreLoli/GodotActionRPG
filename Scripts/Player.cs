using Godot;

public partial class Player : Node2D
{
#region Private members

    private bool _dragging = false;

    [Export] private UnitSelector _unitSelector = null!;
    [Export, ExportGroup("Component")]
    private UnitCommandsComponent _unitCommandsComponent = null!;

    private void ExecuteUnitCommand()
    {
        foreach (var unit in _unitSelector.Selected)
        {
            unit.CommandsComponent.Execute();
        }
    }

#endregion

#region Godot stuff
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion && motion.ButtonMask == MouseButtonMask.Left)
        {
            if (motion.IsPressed())
                _dragging = true;
        }
        else
        {
            _dragging = false;
        }
        // TODO: change later
        if (@event is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == MouseButton.Left)
        {
            if (buttonEvent.IsPressed() && !_dragging)
            {
                ExecuteUnitCommand();
            }
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
