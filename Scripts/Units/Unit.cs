
using Godot;

public partial class Unit : CharacterBody2D
{
#region Private members
    [Export] private bool _selected = false;
#endregion

#region Events
    [Signal] public delegate void OnSelectingEventHandler(Unit unit);
    [Signal] public delegate void OnDeselectingEventHandler(Unit unit);
#endregion

#region Public members
    public virtual void Select()
    {
        _selected = true;
        _ = EmitSignal(nameof(OnSelecting), this);
    }

    public virtual void Deselect()
    {
        _selected = false;
        _ = EmitSignal(nameof(OnDeselecting), this);
    }
#endregion

#region Properties
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
