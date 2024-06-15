using Godot;
using System;
using System.Collections.Generic;

public partial class HumanCharacterAnimator : Node3D
{
    private AnimationTree animationTree;

    [Export]
    public HumanCharacter humanCharacter;

    [Export]
    public string selectedMeshName;

    private Skeleton3D skeleton3D;
    private BoneAttachment3D boneAttachment3D;

    private HumanState previousHumanState;

    private WeaponVisual equipedWeaponVisual = null;


    public override void _Ready()
    {
        base._Ready();
        
        this.animationTree = GetNode<AnimationTree>("AnimationTree");

        this.skeleton3D = GetNode<Skeleton3D>("Armature/Skeleton3D");
        this.boneAttachment3D = GetNode<BoneAttachment3D>("Armature/Skeleton3D/BoneAttachment3D");

        this.humanCharacter.AttackPerformed += this.OnAttackPerformed;

        Godot.Collections.Array<Node> meshes = this.skeleton3D.GetChildren();

        foreach (Node mesh in meshes)
        {
            if (mesh.Name != this.selectedMeshName && mesh.Name != "BoneAttachment3D")
            {
                this.skeleton3D.RemoveChild(mesh);
                mesh.QueueFree();
            }
            else
            {
                ((Node3D)mesh).Visible = true;
            }
        }

        this.previousHumanState = this.humanCharacter.humanState;

        this.animationTree.Set("parameters/StateMachineAttack/conditions/shot", true);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        foreach (Node weapon in this.boneAttachment3D.GetChildren())
        {
            if (weapon.Name == this.humanCharacter.weaponSystem.EquipedWeapon.VisualName)
            {
                this.equipedWeaponVisual = (WeaponVisual) weapon;
                ((Node3D)weapon).Visible = true;
            }
            else
            {
                ((Node3D)weapon).Visible = false;
            }
        }

        if (this.humanCharacter.isDead)
        {
            this.animationTree.Set("parameters/BlendDead/blend_amount", 1);
        }
        else
        {
            this.animationTree.Set("parameters/BlendDead/blend_amount", 0);
        }
        
        if (this.humanCharacter.targetCharacterBody.Velocity.Length() > 0.1)
        {
            this.animationTree.Set("parameters/StateMachineMovement/conditions/walking", true);
            this.animationTree.Set("parameters/StateMachineMovement/conditions/idle", false);
            this.animationTree.Set("parameters/StateMachineBodyTop/Shootgun/conditions/idle", false);
            this.animationTree.Set("parameters/StateMachineBodyTop/Shootgun/conditions/walk", true);
        }
        else
        {
            this.animationTree.Set("parameters/StateMachineMovement/conditions/walking", false);
            this.animationTree.Set("parameters/StateMachineMovement/conditions/idle", true);
            this.animationTree.Set("parameters/StateMachineBodyTop/Shootgun/conditions/idle", true);
            this.animationTree.Set("parameters/StateMachineBodyTop/Shootgun/conditions/walk", false);
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

        if (this.humanCharacter.weaponSystem.EquipedWeapon is SimpleShot)
        {
            this.animationTree.Set("parameters/BlendTopBody/blend_amount", 1);
        }
        else
        {
            this.animationTree.Set("parameters/BlendTopBody/blend_amount", 0);
        }

        if (!(this.previousHumanState is AttackingState) && this.humanCharacter.humanState is AttackingState)
        {
            this.animationTree.Set("parameters/OneShot/request",  (int)AnimationNodeOneShot.OneShotRequest.Fire);
        }

        this.previousHumanState = this.humanCharacter.humanState;
    }


    public void OnAttackPerformed()
    {
        if (this.equipedWeaponVisual != null)
        {
            this.equipedWeaponVisual.PlayAttackEffect();
        }
    }
}
