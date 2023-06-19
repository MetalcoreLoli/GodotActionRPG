
using Godot;

public partial class Unit : CharacterBody2D
{
#region Private members
    [Export] private UnitCommandsComponent _unitCommandsComponent = null!;

    [Export] private bool _selected = false;
#endregion

#region Public members
    public virtual void Select()
    {
        _selected = true;
    }

    public virtual void Deselect()
    {
        _selected = true;
    }
#endregion

#region Properties
    public UnitCommandsComponent CommandsComponent => _unitCommandsComponent;

    public bool Selected => _selected;
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
