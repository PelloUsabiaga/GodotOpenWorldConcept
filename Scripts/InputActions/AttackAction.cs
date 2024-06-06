using Godot;
using System;

public class AttackAction : InputAction
{
    public override InputActionType ActionType { get; set; } = InputActionType.Attack;
    public DamageComponent targetDamageComponent;

    public AttackAction(DamageComponent targetDamageComponent)
    {
        this.targetDamageComponent = targetDamageComponent;
    }
}