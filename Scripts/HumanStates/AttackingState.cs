using Godot;
using System;

public partial class AttackingState : HumanState
{
    private double attackTime;
    private double makeAttackAtTime;
    private double attackingTime = 0;

    private HumanCharacter humanCharacter;
    private AttackAction attackAction;
    private bool attackMade = false;

    public AttackingState(float attackTime, float makeAttackAtTime, 
                            HumanCharacter humanCharacter, AttackAction attackAction)
    {
        if (attackTime < makeAttackAtTime)
        {
            throw new ArgumentException("AttackTime can not be smaller that makeAttackAtTime, it whould never attack.");
        }
        this.attackTime = attackTime;
        this.makeAttackAtTime = makeAttackAtTime;
        this.humanCharacter = humanCharacter;
        this.attackAction = attackAction;
    }

    public override bool CanMove { get; protected set; } = true; 
    public override bool CanRun { get; protected set; } = false;
    public override bool CanJump { get; protected set; } = false;

    public override bool CanAttack { get; protected set; } = false;
    public override bool CanReload { get; protected set; } = false;
    public override bool CanDefend { get; protected set; } = false;
    
    public override bool CanInteract { get; protected set; } = false;
    public override bool CanSpeak { get; protected set; } = false;
    public override bool CanTake { get; protected set; } = false;

    public override HumanState Update(double delta, HumanCharacter humanCharacter)
    {
        this.attackingTime += delta;
        if (this.attackingTime > this.makeAttackAtTime && !this.attackMade)
        {
            this.attackMade = true;
            this.humanCharacter.weaponSystem.TryAttackToDamageComponent(this.attackAction.targetDamageComponent);
        }
        if (this.attackingTime > this.attackTime)
        {
            return new NormalState(humanCharacter);
        }
        else
        {
            return this;
        }
    }

    public override HumanState HandleInput(InputAction InputAction)
    {
        return this;
    }
}