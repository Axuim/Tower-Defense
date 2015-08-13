using System;

public interface IAmKillable
{
    event EventHandler Killed;

    int MaxHealth { get; }
    int Health { get; }

    void TakeDamage(int amount);
}
