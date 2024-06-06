using Godot;
using System;

public abstract partial class HumanState : Node
{
    public virtual bool CanMove { get; protected set; } = true; 
    public virtual bool CanRun { get; protected set; } = true;
    public virtual bool CanJump { get; protected set; } = true;

    public virtual bool CanAttack { get; protected set; } = true;
    public virtual bool CanReload { get; protected set; } = true;
    public virtual bool CanDefend { get; protected set; } = true;
    
    public virtual bool CanInteract { get; protected set; } = true;
    public virtual bool CanSpeak { get; protected set; } = true;
    public virtual bool CanTake { get; protected set; } = true;

    public abstract HumanState Update(double delta, HumanCharacter humanCharacter);
    public abstract HumanState HandleInput(InputAction InputAction);
}