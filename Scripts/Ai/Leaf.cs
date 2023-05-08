using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ActionRPG.Scripts.Ai;
public class Leaf : IAiActionNode
{
    private readonly Func<AiActionStatus> _action;

    public Leaf(Func<AiActionStatus> action)
    {
        _action = action;
    }

    public string Name { get; init; }

    public List<IAiActionNode> Children { get; init; } = new List<IAiActionNode>();

    public IAiActionNode Add(IAiActionNode node)
    {
        Children.Add(node);
        GD.Print("dasdad: "+Children.Aggregate("", (acc, i) => acc + i + " "));
        return this;
    }

    public AiActionStatus Execute()
    {
        return _action();
    }
}

