using Godot;
using System;

public partial class HumanCharacterAnimator : Node3D
{
    private AnimationTree animationTree;

    [Export]
    public HumanCharacter humanCharacter;


    public override void _Ready()
    {
        base._Ready();
        this.animationTree = GetNode<AnimationTree>("AnimationTree");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        
        if (this.humanCharacter.targetCharacterBody.Velocity.Length() > 0.1)
        {
            this.animationTree.Set("parameters/StateMachineMovement/conditions/walking", true);
            this.animationTree.Set("parameters/StateMachineMovement/conditions/idle", false);
        }
        else
        {
            this.animationTree.Set("parameters/StateMachineMovement/conditions/walking", false);
            this.animationTree.Set("parameters/StateMachineMovement/conditions/idle", true);
        }

        if (this.humanCharacter.humanState is JumpingState)
        {
            this.animationTree.Set("parameters/StateMachineMovement/conditions/jumping", true);
            this.animationTree.Set("parameters/StateMachineMovement/conditions/not_jumping", false);
            if (((JumpingState) this.humanCharacter.humanState).jumped)
            {
                this.animationTree.Set("parameters/StateMachineMovement/conditions/already_jumping", true);
            }
            else
            {
                this.animationTree.Set("parameters/StateMachineMovement/conditions/already_jumping", false);
            }
        }
        else
        {
            this.animationTree.Set("parameters/StateMachineMovement/conditions/jumping", false);
            this.animationTree.Set("parameters/StateMachineMovement/conditions/not_jumping", true);
        }
    }
}
