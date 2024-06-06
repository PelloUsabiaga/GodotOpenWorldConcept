using Godot;
using System;

public enum InputActionType
{
    MoveTo,
    MoveWASD,
    Jump,
    Attack
}

public abstract class InputAction
{
    public virtual InputActionType ActionType { get; set; }
}