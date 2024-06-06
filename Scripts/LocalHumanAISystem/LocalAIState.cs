

using Godot;

public abstract partial class LocalAIState : Node
{
    public abstract LocalAIState Update(double delta, LocalHumanAI localHumanAI);
}