

using System;
using System.Runtime.CompilerServices;
using Godot;
public partial class WalkingAroundAIState : LocalAIState
{
    protected Vector3 centralPosition;
    protected float distanceOrder;
    protected LocalHumanAI localHumanAI;

    private double stopTime;
    private double currentStopWaitTime;

    public WalkingAroundAIState(Vector3 centralPosition, float distanceOrder, LocalHumanAI localHumanAI)
    {
        this.centralPosition = centralPosition;
        this.distanceOrder = distanceOrder;
        this.localHumanAI = localHumanAI;

        this.stopTime = 0;
        this.currentStopWaitTime = this.GetSomeRandomWaitTime();
        this.localHumanAI.targetHumanCharacter.NavigationTargetReached += this.NavigationFinishedHandler;
    }
    public override LocalAIState Update(double delta, LocalHumanAI localHumanAI)
    {
        if (this.localHumanAI.targetHumanCharacter.targetCharacterBody.Velocity.LengthSquared() < 0.1)
        {
            this.stopTime += delta;
        }
        if (this.stopTime > this.currentStopWaitTime)
        {
            this.stopTime = 0;
            Random random = new Random();
            Vector3 randomHorizontal = new Vector3((float)random.NextDouble() * this.distanceOrder - this.distanceOrder/2, 0, 
                                                (float)random.NextDouble() * this.distanceOrder - this.distanceOrder/2);
            Vector3 targetPosition = this.localHumanAI.targetHumanCharacter.GlobalPosition + randomHorizontal 
                                    + 0.3f*(this.centralPosition - this.localHumanAI.targetHumanCharacter.GlobalPosition);
 
            this.localHumanAI.targetHumanCharacter.HandleInput(new MoveToAction(targetPosition));
        }
        return this;
    }

    private void NavigationFinishedHandler()
    {
        this.currentStopWaitTime = this.GetSomeRandomWaitTime();
    }

    private double GetSomeRandomWaitTime(double maximum = 6, double minimum = 3)
    {
        Random random = new Random(Guid.NewGuid().GetHashCode());
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
}