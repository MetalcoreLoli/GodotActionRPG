using Godot;
using System;

public partial class StatsComponent : Node
{
#region Private members
    private int _intellect;
    private int _charism;
    private int _consitution;
    private int _dexterity;
    private int _wisdom;
    private int _strength;

    private int CalculateModificator(int value)
    {
        return value == 0 || value - 10 == 0? 0: (value - 10) / 2;
    }

    private void Set(ref int desitination, int value)
    {
        if (desitination == value) return;
        desitination = value;
    }
#endregion

#region Public members
    [ExportGroup("Stats")]
    [Export] public int Intellect
    {
        get => _intellect;
        set
        {
            Set(ref _intellect, value);
            IntellectMod = CalculateModificator(_intellect);
        }
    }
    [Export] public int Charism
    {
        get => _charism;
        set
        {
            Set(ref _charism, value);
            CharismMod = CalculateModificator(_charism);
        }
    }
    [Export] public int Consitution
    {
        get => _consitution;
        set
        {
            Set(ref _consitution, value);
            ConsitutionMod = CalculateModificator(_consitution);
        }
    }
    [Export] public int Dexterity
    {
        get => _dexterity;
        set
        {
            Set(ref _dexterity, value);
            DexterityMod = CalculateModificator(_dexterity);
        }
    }
    [Export] public int Wisdom
    {
        get => _wisdom;
        set
        {
            Set(ref _wisdom, value);
            WisdomMod = CalculateModificator(_wisdom);
        }
    }
    [Export] public int Strength
    {
        get => _strength;
        set
        {
            Set(ref _strength, value);
            StrengthMod = CalculateModificator(_strength);
        }
    }

    [ExportGroup("Modificators")]
    [Export] public int IntellectMod { get; private set; }
    [Export] public int CharismMod { get; private set; }
    [Export] public int ConsitutionMod { get; private set; }
    [Export] public int DexterityMod { get; private set; }
    [Export] public int WisdomMod { get; private set; }
    [Export] public int StrengthMod { get; private set; }
#endregion
}
