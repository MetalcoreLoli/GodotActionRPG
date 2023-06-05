using Godot;
using System;

public partial class StatsComponent : Node
{
#region Private members
#endregion

#region Public members
    public Stat Intellect { get; set; }
    public Stat Charism { get; set; }
    public Stat Consitution { get; set; }
    public Stat Dexterity { get; set; }
    public Stat Wisdom { get; set; }
    public Stat Strength { get; set; }

    public struct Stat
    {
        private int _value = default;

        public int Value
        {
            readonly get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                Modificator = _value == 0? 0: (_value - 10) / 2;
            }
        }

        public int Modificator { get; private set; } = default;
        public Stat(int value)
        {
            Value = value;
        }

        public static implicit operator Stat(int value) => new (value);
    }
#endregion
}
