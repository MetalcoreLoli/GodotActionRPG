using Godot;
using System;

public partial class HealthComponent : Node
{
#region Private members
	private int _maxHealth = 10;
	private int _health = 10;
#endregion
#region Public members

#region Events
	[Signal] public delegate void OnDeathEventHandler(Node killer);
	[Signal] public delegate void OnTakeDamageEventHandler(int damage, Node from, Node to);
#endregion

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
		private set
		{
			if (_health == value) return;
			_health = value;
		}
	}

	public void TakeDamage(int damage, Node from, Node to)
	{
		_ = EmitSignal(nameof(OnTakeDamage), damage, from, to);
		Health -= damage;
		if (Health <= 0)
		{
			Die(from);
		}
	}

	public void Die(Node killer)
	{
		_ = EmitSignal(nameof(OnDeath), killer);
		Health = 0;
	}
#endregion
}
