using Godot;
using System;

public partial class ReloadingState : HumanState
{
    private double reloadTime;
    private double reloadingTime = 0;

    [Signal]
    public delegate void ReloadCompletedEventHandler();

    public ReloadingState(float reloadTime)
    {
        this.reloadTime = reloadTime;
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
        this.reloadingTime += delta;
        if (this.reloadingTime > this.reloadTime)
        {
            EmitSignal(SignalName.ReloadCompleted);
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