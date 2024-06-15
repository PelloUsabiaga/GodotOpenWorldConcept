

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

    private bool changeCentralPositionRandomly;
    private Vector3? inertia;

    private Random random;

    public WalkingAroundAIState(Vector3 centralPosition, LocalHumanAI localHumanAI, float distanceOrder = 14, bool changeCentralPositionRandomly=false, Vector3? inertia=null)
    {
        this.centralPosition = centralPosition;
        this.distanceOrder = distanceOrder;
        this.localHumanAI = localHumanAI;
        this.changeCentralPositionRandomly = changeCentralPositionRandomly;
        this.inertia = inertia;

        this.random = new Random(Guid.NewGuid().GetHashCode());

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
            if (this.changeCentralPositionRandomly)
            {
                this.centralPosition += this.GetRandomHorizontalVector(this.distanceOrder * 2);
                if (this.inertia != null)
                {
                    this.centralPosition += new Vector3(((Vector3)inertia).X, 0, ((Vector3)inertia).Z)*this.distanceOrder;
                    this.inertia = this.inertia/2;
                }
            }
            this.stopTime = 0;
            Vector3 randomHorizontal = this.GetRandomHorizontalVector(this.distanceOrder);
            Vector3 targetPosition = this.localHumanAI.targetHumanCharacter.GlobalPosition + randomHorizontal 
                                    + 1/this.distanceOrder*(this.centralPosition - this.localHumanAI.targetHumanCharacter.GlobalPosition);
 
            this.localHumanAI.targetHumanCharacter.HandleInput(new MoveToAction(targetPosition));
        }
        return this;
    }

    private Vector3 GetRandomHorizontalVector(float sizeOrder)
    {
        Vector3 randomHorizontal = new Vector3((float)random.NextDouble() * sizeOrder - sizeOrder/2, 0, 
                                                (float)random.NextDouble() * sizeOrder - sizeOrder/2);
        return randomHorizontal;
    }

    private void NavigationFinishedHandler()
    {
        this.currentStopWaitTime = this.GetSomeRandomWaitTime();
    }

    private double GetSomeRandomWaitTime(double maximum = 6, double minimum = 3)
    {
        return this.random.NextDouble() * (maximum - minimum) + minimum;
    }
}