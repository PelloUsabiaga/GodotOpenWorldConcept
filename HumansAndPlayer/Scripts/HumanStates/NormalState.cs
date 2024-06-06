using Godot;
using System;

public partial class NormalState : HumanState
{
    public NormalState(HumanCharacter humanCharacter)
    {
        if (humanCharacter.isWounded)
        {
            this.CanRun = false;
            this.CanJump = false;
        }
        else
        {
            this.CanRun = true;
            this.CanJump = true;
        }
    }

    public override bool CanMove { get; protected set; } = true; 
    public override bool CanRun { get; protected set; } = true;
    public override bool CanJump { get; protected set; } = true;

    public override bool CanAttack { get; protected set; } = true;
    public override bool CanReload { get; protected set; } = true;
    public override bool CanDefend { get; protected set; } = true;
    
    public override bool CanInteract { get; protected set; } = true;
    public override bool CanSpeak { get; protected set; } = true;
    public override bool CanTake { get; protected set; } = true;

    public override HumanState Update(double delta, HumanCharacter humanCharacter)
    {   
        if (humanCharacter.isWounded)
        {
            this.CanRun = false;
            this.CanJump = false;
        }
        else
        {
            this.CanRun = true;
            this.CanJump = true;
        }
        return this;
    }

    public override HumanState HandleInput(InputAction InputAction)
    {
        return this;
    }
}