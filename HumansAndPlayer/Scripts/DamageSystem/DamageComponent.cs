using Godot;
using System;

public delegate void AttackReceivedCsEventHandler(Attack receivedAttack);

public partial class DamageComponent : Node3D
{
    [Export]
    public Node damageableInterfaceNode;

    public event AttackReceivedCsEventHandler attackReceived;
    private DamageableInterface damageableInterface;
    
    private int _health;
    public int health 
    { 
        get
        {
            return this._health;            
        } 
        private set
        {
            this._health = value;
            this.HealthChanged();
        } 
    }
    public int maxHealth { get; private set; } = 1;

    public override void _Ready()
    {
        base._Ready();
        this.damageableInterface = (DamageableInterface) this.damageableInterfaceNode;
        this.health = this.maxHealth;
    }

    public void MakeAttack(Attack incommingAttack)
    {
        Attack incomingAttackWithModifiers = this.damageableInterface.ApplyDamageModifiersToAttack(incommingAttack);
        this.health -= incomingAttackWithModifiers.Damage;
        this.attackReceived?.Invoke(incomingAttackWithModifiers);
    }

    private void HealthChanged()
    {
        this.damageableInterface.NotifyHealthChanged(this.health);
    }


}
