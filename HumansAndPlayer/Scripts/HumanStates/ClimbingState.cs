using Godot;
using System;

public partial class ClimbingState : HumanState
{


    public override bool CanMove { get; protected set; } = true; 
    public override bool CanRun { get; protected set; } = false;
    public override bool CanJump { get; protected set; } = true;

    public override bool CanAttack { get; protected set; } = false;
    public override bool CanReload { get; protected set; } = false;
    public override bool CanDefend { get; protected set; } = false;
    
    public override bool CanInteract { get; protected set; } = false;
    public override bool CanSpeak { get; protected set; } = false;
    public override bool CanTake { get; protected set; } = false;

    public override HumanState Update(double delta, HumanCharacter humanCharacter)
    {
        return this;
    }

    public override HumanState HandleInput(InputAction InputAction)
    {
        return this;
    }
}