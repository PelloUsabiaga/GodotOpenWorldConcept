using Godot;
using System;

public partial class JumpingState : HumanState
{
    private float jumpSpeed;
    private double jumpTime;
    private double jumpingTime = 0;
    public bool jumped { get; set; } = false;

    private Vector3 initialVelocity;

    public JumpingState(float jumpSpeed, double jumpTime, HumanCharacter humanCharacter)
    {
        this.jumpSpeed = jumpSpeed;
        this.jumpTime = jumpTime;

        this.initialVelocity = humanCharacter.targetCharacterBody.Velocity;
    }

    public override bool CanMove { get; protected set; } = false; 
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
        if (humanCharacter.targetCharacterBody.IsOnFloor() && this.jumped){
            humanCharacter.targetCharacterBody.Velocity = Vector3.Zero;
            return new NormalState(humanCharacter);
        }

        this.jumpingTime += delta;
        if (this.jumpTime < this.jumpingTime && !this.jumped)
        {
            this.jumped = true;
            Vector3 newVelocity = this.initialVelocity;
            newVelocity.Y = this.jumpSpeed;
            humanCharacter.targetCharacterBody.Velocity = newVelocity;
        }
        
        
        if (this.jumped) 
        {
            Vector3 newVelocity = humanCharacter.targetCharacterBody.Velocity;
            newVelocity.Y -= (float)(9.8 * delta);
            humanCharacter.targetCharacterBody.Velocity = newVelocity;
        }
        else
        {
            humanCharacter.targetCharacterBody.Velocity = humanCharacter.targetCharacterBody.Velocity.MoveToward(Vector3.Zero, 0.25f);
        }

        return this;
    }

    public override HumanState HandleInput(InputAction InputAction)
    {
        return this;
    }
}