using System;
using System.Collections.Generic;

namespace ActionRPG.Scripts.Ai;
[Flags]
public enum AiActionStatus: byte
{
	Succeded = 1 << 0,
	Failure  = 1 << 1,
	Running   = 1 << 2,
}

public interface IAiActionNode
{
	string Name { get; init; }
	List<IAiActionNode> Children { get; }

	AiActionStatus Execute();
	IAiActionNode Add(IAiActionNode node);
}

