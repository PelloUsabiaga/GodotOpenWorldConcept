using Godot;
using System;

public class MoveToAction : InputAction
{
    public override InputActionType ActionType { get; set; } = InputActionType.MoveTo;
    public Vector3 targetPosition;

    public MoveToAction(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}