
using System;
using System.IO;
using Godot;

namespace ActionRPG.Scripts.Dices;

public static class DiceRoller
{
    public static long D20 => Roll(1, 20);
    public static long D12 => Roll(1, 12);
    public static long D8 => Roll(1, 8);
    public static long D6 => Roll(1, 6);
    public static long D4 => Roll(1, 4);

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
        return Roll(count, edges);
    }
}

