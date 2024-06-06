

public partial class NoneAIState : LocalAIState
{
    public override LocalAIState Update(double delta, LocalHumanAI localHumanAI)
    {
        return this;
    }
}