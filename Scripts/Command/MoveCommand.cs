using Godot;
using System;

namespace ActionRPG.Scripts.Command;
public partial class MoveCommand : UnitCommand
{
#region Private members
    [Export] private MovementComponent _moveComponent = null!;
#endregion

#region Properties
    [Export] public Vector2 Destiantion { get; set; } = Vector2.Zero;
#endregion

#region Public members
    public override void Execute()
    {
    }
#endregion
}
