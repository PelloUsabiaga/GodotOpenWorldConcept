using Godot;
using System;

public class MoveWASDAction : InputAction
{
    public Vector2 movementDirection;
    public double deltaTime;
    public override InputActionType ActionType { get; set; } = InputActionType.MoveWASD;

    public MoveWASDAction(Vector2 movementDirection, double deltaTime)
    {
        this.movementDirection = movementDirection;
        this.deltaTime = deltaTime;
    }
}