using System;
using Godot;

namespace ActionRPG.Scripts.Command;

public partial class UnitCommand: Node2D
{
    public State CurrentState { get; protected set; }
    public virtual void Execute()
    {
    }

    [Flags]
    public enum State
    {
        Success = 1 << 0,
        Failure = 1 << 1,
        Running = 1 << 2,
    }
}
