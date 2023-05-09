using System.Collections.Generic;
using System.Linq;
using Godot;

namespace ActionRPG.Scripts.Ai;

public struct SequenceNode : IAiActionNode
{
	private int _currentChild;

	public string Name { get; init; }

	public List<IAiActionNode> Children {get;}
	public SequenceNode(string name)
	{
		Children = new();
        Name = name;
        _currentChild = 0;
	}

	public IAiActionNode Add(IAiActionNode node)
	{
		GD.Print($"node '{node.Name}' was added to sequence");
		Children.Add(node);
		return this;
	}

	public AiActionStatus Execute()
	{
		var childStatus = Children[_currentChild].Execute();
		if (childStatus is AiActionStatus.Running)
		{
			return AiActionStatus.Running;
		}
		else if (childStatus is AiActionStatus.Failure)
		{
			return AiActionStatus.Failure;
		}

		_currentChild++;
		if (_currentChild >= Children.Count)
		{
			_currentChild = 0;
			return AiActionStatus.Succeded;
		}
		return AiActionStatus.Running;
	}
}

