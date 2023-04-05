using Godot;
using System;

public partial class Stats : Node
{
    #region Private members
    private int _maxHealth = 10;
    private int _health = 10;

    private void OnTakeDamage(int damage, Node from, Node to)
    {
        _ = EmitSignal(nameof(TakeDamage), damage, from, to);
    }

    private void OnDeath(Node killer)
    {
        _ = EmitSignal(nameof(Death), killer);
    }
    #endregion

    #region Public members
    [Signal] public delegate void DeathEventHandler(Node killer);
    [Signal] public delegate void TakeDamageEventHandler(int damage, Node from, Node to);

    [Export]
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            if (_maxHealth == value) return;
            _maxHealth = value;
        }
    }

    [Export]
    public int Health
    {
        get => _health;
        set
        {
            if (_health == value) return;
            _health = value;
        }
    }

    [Export] public string Name { get; set; }

    public void Damage(int damage, Node from, Node to)
    {
        OnTakeDamage(damage, from, to);
        Health -= damage;
        if (Health <= 0)
        {
            Die(from);
        }
    }

    public void Die(Node killer)
    {
        OnDeath(killer);
        Health = 0;
        QueueFree();
    }
    #endregion

}
