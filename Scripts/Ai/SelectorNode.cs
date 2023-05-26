using System.Collections.Generic;
using Godot;

namespace ActionRPG.Scripts.Ai;

public struct SelectorNode : IAiActionNode
{
	private int _currentChild = 0;

	public SelectorNode(string name = "SelectorNode")
	{
		Children = new ();
		Name = name;
	}

	public List<IAiActionNode> Children { get; }
	public string Name { get; init; }

	public IAiActionNode Add(IAiActionNode node)
	{
		GD.Print($"node '{node.Name}' was added to selector '{Name}'");
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

		if (childStatus is AiActionStatus.Success)
		{
			_currentChild = 0;
			return AiActionStatus.Success;
		}


		_currentChild++;
		if (_currentChild >= Children.Count)
		{
			_currentChild = 0;
			return AiActionStatus.Failure;
		}
		return AiActionStatus.Running;
	}
}


