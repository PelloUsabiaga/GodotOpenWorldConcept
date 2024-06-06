using Godot;
using System;

public partial class NavigationComponent : Node3D
{
    [Export]
    public HumanCharacter humanCharacter;
    
    [Signal]
    public delegate void TargetReachedEventHandler();
    
    [Signal]
    public delegate void NewVelocityAvailableEventHandler(Vector3 newVelocity);

    public NavigationAgent3D navigationAgent;

    public bool isNavigationActive { get;set; } = false;

    public override void _Ready()
    {
        base._Ready();
        this.navigationAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
    
        this.navigationAgent.VelocityComputed += this.OnNavigationAgent3dVelocityComputed;
        this.navigationAgent.NavigationFinished += this.OnNavigationFinished;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (this.isNavigationActive)
        {
            Vector3 nextPathPosition = this.navigationAgent.GetNextPathPosition();
            Vector3 newVelocity = this.GlobalPosition.DirectionTo(nextPathPosition) * this.humanCharacter.movementSpeed;
            this.navigationAgent.Velocity = newVelocity;
        }
    }


    private void OnNavigationFinished()
    {
        if (this.isNavigationActive)
        {
            this.isNavigationActive = false;
            EmitSignal(SignalName.TargetReached);
        }
    }
    private void OnNavigationAgent3dVelocityComputed(Vector3 safeVelocity)
    {
        if (this.isNavigationActive)
        {
            Vector3 newVelocity = this.humanCharacter.targetCharacterBody.Velocity.MoveToward(safeVelocity, 0.25f);
            
            EmitSignal(SignalName.NewVelocityAvailable, newVelocity);
        }
    }

    public void StopOnCurrentPosition(){
        this.navigationAgent.TargetPosition = this.humanCharacter.GlobalPosition;
    }

    public void UpdateTargetPosition(Vector3 position){
        this.isNavigationActive = true;
        this.navigationAgent.TargetPosition = position;
    }
}
