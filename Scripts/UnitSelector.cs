using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public partial class UnitSelector : Node2D
{
#region  Private members
    [Export] private bool _dragging = false;
    [Export] private Array<Dictionary> _selected = new();

    [Export] private UnitSelectorPainter _painter = null!;

    [Export] private Vector2 _dragStart = Vector2.Zero;
    [Export] private Vector2 _dragEnd = Vector2.Zero;
    private RectangleShape2D _selectRentangle = new ();
#endregion

#region Events

#endregion

#region Propeties
    public List<Unit> Selected { get; private set; }
#endregion

#region Godot stuff
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Selected = new();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == MouseButton.Left)
        {
            if (@event.IsPressed())
            {
                _selected = new();
                _dragging = true;
                _dragStart = buttonEvent.Position;
            }
            else if (_dragging)
            {
                _dragging = false;
                _selected.Clear();
                _painter.UpdateStatus(_dragStart, buttonEvent.Position, _dragging);
                var space = GetWorld2D().DirectSpaceState;
                _dragEnd = buttonEvent.Position;
                _selectRentangle.Size = new (Mathf.Abs(_dragEnd.X - _dragStart.X), Mathf.Abs(_dragEnd.Y - _dragStart.Y));
                //
                var query = new PhysicsShapeQueryParameters2D
                {
                    Transform = new Transform2D(0, (_dragEnd + _dragStart) / 2),
                    Shape = _selectRentangle,
                    CollisionMask = 128
                };
                _selected = space.IntersectShape(query);
                foreach (Dictionary unit in _selected)
                {
                    var coll = (Unit)unit["collider"];
                    GD.Print(coll);
                    Selected.Add(coll);
                }

            }
        }
        if (_dragging && @event is InputEventMouseMotion motion)
        {
            _painter.UpdateStatus(_dragStart, motion.Position, _dragging);
        }
    }
#endregion
}
