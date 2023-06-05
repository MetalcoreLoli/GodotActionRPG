
using System;
using System.IO;
using System.Linq;
using Godot;

namespace ActionRPG.Scripts.Dices;

public static class DiceRoller
{
    public static long Roll(int count, int edges)
    {
        GD.Randomize();
        long sum = 0;
        for (var i = 0; i < count; i ++)
        {
            sum += GD.Randi() % edges + 1;
        }
        return sum;
    }

    public static long Roll(ReadOnlySpan<char> pattern)
    {
        if (pattern.Length != 3)
            throw new InvalidDataException("pattern was invalid");
        int mid = pattern.IndexOf('d');
        int count = int.Parse(pattern.Slice(0, mid));
        int edges = int.Parse(pattern.Slice(mid + 1, pattern.Length - mid - 1));
        GD.Print($"{count}d{edges}");
        return Roll(count, edges);
    }
}

