

using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Godot;
public partial class FollowToAiState : LocalAIState
{
    private Node3D target;
    private float distanceSquared;
    public LocalHumanAI localHumanAI { get; private set; }


    public FollowToAiState(Node3D target, float distance, LocalHumanAI localHumanAI)
    {
        this.target = target;
        this.localHumanAI = localHumanAI;
        this.distanceSquared = (float)Math.Pow(distance, 2);

        this.localHumanAI.targetHumanCharacter.NavigationTargetReached += this.NavigationFinishedHandler;
    }
    public override LocalAIState Update(double delta, LocalHumanAI localHumanAI)
    {
        if (this.localHumanAI.GlobalPosition.DistanceSquaredTo(this.target.GlobalPosition) > this.distanceSquared)
        {
            this.localHumanAI.targetHumanCharacter.HandleInput(new MoveToAction(this.target.GlobalPosition));
        }
        else
        {
            this.localHumanAI.targetHumanCharacter.HandleInput(new MoveToAction(localHumanAI.GlobalPosition));
        }
        return this;
    }

    private void NavigationFinishedHandler()
    {

    }
}