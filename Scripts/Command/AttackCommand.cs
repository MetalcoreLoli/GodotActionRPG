using ActionRPG.Scripts.Command;
using Godot;
using System;
using System.Linq;

public partial class AttackCommand : UnitCommand
{
#region Private members
    [Export] private Vector2 _mousePosition = Vector2.Zero;
    [Export] private Node2D _node = null!;

    private bool IsThereAnyUnitAt(Vector2 location, out Unit unit)
    {
        unit = null;
        var space = _node.GetWorld2D().DirectSpaceState;
        RectangleShape2D selectRentangle = new()
        {
            Size = new(10, 10)
        };

        var query = new PhysicsShapeQueryParameters2D
        {
            Transform = new Transform2D(0, location / 2),
            Shape = selectRentangle,
            CollisionMask = 128
        };

        var units = space.IntersectShape(query);
        if (!units.Any())
        {
            return false;
        }

        unit = (Unit)units.First()["collider"];
        return true;
    }
    #endregion

#region Public
    public override void Execute()
    {
        if (!IsThereAnyUnitAt(_mousePosition, out var unit))
            return;
        GD.Print(unit);
    }
#endregion

#region Godot stuff

    public override void _Input(InputEvent @event)
    {
        // TODO: change later
        if (@event is InputEventMouseButton buttonEvent && buttonEvent.ButtonIndex == MouseButton.Left)
        {
            _mousePosition = GetViewport().GetMousePosition();
        }
    }

#endregion
}
