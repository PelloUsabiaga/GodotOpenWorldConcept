using Godot;
using System;
using System.Collections.Generic;

public delegate void DamagedCsEventHandler();

public interface IDamageable
{
    public void NotifyHealthChanged(int newHealth);

    public Attack ApplyDamageModifiersToAttack(Attack attack);

    public event DamagedCsEventHandler DamagedCs;
}