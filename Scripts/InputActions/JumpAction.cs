using Godot;
using System;

public class JumpAction : InputAction
{
    public override InputActionType ActionType { get; set; } = InputActionType.Jump;

    public JumpAction()
    {
    }
}