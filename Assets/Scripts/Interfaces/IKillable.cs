using System;

public interface IKillable
{
    event EventHandler Killed;

    int MaxHealth { get; }
    int Health { get; }   
}
