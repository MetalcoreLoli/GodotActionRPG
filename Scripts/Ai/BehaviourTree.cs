using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActionRPG.Scripts.Ai;
public class BehaviourTree : IAiActionNode
{
    public string Name { get; init; } = "root";

    public bool IsEmpty => Children.Count == 0;
    public List<IAiActionNode> Children { get; }
    public AiActionStatus Status { get; private set; }
    public BehaviourTree()
    {
        Children = new ();
    }

    public IAiActionNode Add(IAiActionNode node)
    {
        Children.Add(node);
        return this;
    }

    public override string ToString()
    {
        var res = new StringBuilder();
        static void toStringHelper(IAiActionNode node, int level, StringBuilder output)
        {
            _ = output.AppendLine($"{new string(' ', level)}{node.Name}");
            if (node.Children.Any())
            {
                foreach(IAiActionNode n in node.Children)
                {
                    toStringHelper(n, level + 1, output);
                }
            }
        }
        toStringHelper(this, 0, res);
        return res.ToString();
    }

    public AiActionStatus Execute()
    {
        Status = Children.FirstOrDefault().Execute();
        return Status;
    }
}

