

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using Godot;
public partial class FightingWithTeamAIState : LocalAIState
{
    protected ITeamMember _targetTeamMember;
    protected ITeamMember targetTeamMember 
    { 
        get
        {
            return _targetTeamMember;
        } 
        set
        {
            if (value != null)
            {
                this.lastUncheckedVisualizationPosition = value.node3D.GlobalPosition;
            }
            _targetTeamMember = value;
        }
    }

    protected Vector3? lastUncheckedVisualizationPosition { get; set; } = null;    public GameLogic gameLogic { get; set; }

    public int targetTeamID { get; set; }

    public LocalHumanAI localHumanAI { get; set; }

    private LocalAIState subState { get; set; }

    private Random random;

    private PhysicsDirectSpaceState3D spaceState;

    private double currentTargetUpdateWaitTime;
    private double timeSinceLastNewTarget = 0;

    private double currentAttackUpdateWaitTime;
    private double timeSinceLastAttackUpdate = 0;


    private float maxVisivilityDistanceSquared;

    public FightingWithTeamAIState(int targetTeamID, LocalHumanAI localHumanAI, GameLogic gameLogic, float maxVisivilityDistance=100)
    {
        this.targetTeamID = targetTeamID;
        this.gameLogic = gameLogic;
        this.localHumanAI = localHumanAI;
        this.maxVisivilityDistanceSquared = maxVisivilityDistance * maxVisivilityDistance;

        this.spaceState = this.localHumanAI.targetHumanCharacter.GetWorld3D().DirectSpaceState;

        this.random = new Random(Guid.NewGuid().GetHashCode());

        this.currentTargetUpdateWaitTime = this.GetSomeRandomWaitTime(2.5, 3.5);
        this.currentAttackUpdateWaitTime = this.GetSomeRandomWaitTime(1, 1.5);

        this.localHumanAI.targetHumanCharacter.NavigationTargetReached += this.NavigationFinishedHandler;
    }

    private ITeamMember GetNewTarget()
    {
        this.timeSinceLastNewTarget = 0;
        this.currentTargetUpdateWaitTime = this.GetSomeRandomWaitTime(2.5, 3.5);

        List<ITeamMember> targetTeamMembers = this.gameLogic.teamsCharacterRegister.teamIdToMembers[this.targetTeamID];
        if (targetTeamMembers == null || targetTeamMembers.Count == 0)
        {
            this.targetTeamMember = null;
            return null;
        }
        ITeamMember closestTarget = null;
        float distanceSquaredToClosest = float.MaxValue;
        for (int i = 0; i < targetTeamMembers.Count; i++) 
        {
            ITeamMember targetTeamMember_i = targetTeamMembers[i];
            Node3D targetNode3D_i = targetTeamMember_i.node3D;
            if (IsInstanceValid(targetNode3D_i) && targetNode3D_i.IsInsideTree())
            {
                float distanceSquaredTo_i = this.localHumanAI.GlobalPosition.DistanceSquaredTo(targetNode3D_i.GlobalPosition);
                if (!this.IsTargetVisible(targetNode3D_i, distanceSquaredTo_i))
                {
                    continue;
                }
                if (closestTarget == null)
                {
                    closestTarget = targetTeamMember_i;
                    distanceSquaredToClosest = distanceSquaredTo_i;
                }
                else
                {
                    if (distanceSquaredTo_i < distanceSquaredToClosest)
                    {
                        closestTarget = targetTeamMember_i;
                        distanceSquaredToClosest = distanceSquaredTo_i;
                    }
                }
            }
        }
        return closestTarget;
    }

    private void UpdateTarget(double delta)
    {
        this.timeSinceLastNewTarget += delta;
        if (this.timeSinceLastNewTarget > this.currentTargetUpdateWaitTime)
        {
            this.targetTeamMember = this.GetNewTarget();
        }
    }

    private void UpdateCheckIfAttack(double delta)
    {
        this.timeSinceLastAttackUpdate += delta;

        if (this.timeSinceLastAttackUpdate > this.currentAttackUpdateWaitTime && 
        this.targetTeamMember != null && IsInstanceValid(this.targetTeamMember.node3D) && this.targetTeamMember.node3D.IsInsideTree())
        {
            this.timeSinceLastAttackUpdate = 0;
            this.currentAttackUpdateWaitTime = this.GetSomeRandomWaitTime(1, 1.5);

            GD.Print("Checking if attack");

            if (this.localHumanAI.GlobalPosition.DistanceTo(this.targetTeamMember.node3D.GlobalPosition) 
                < this.localHumanAI.targetHumanCharacter.weaponSystem.EquipedWeapon.AttackDistance)
            {
                GD.Print("Attack order!");
                this.localHumanAI.targetHumanCharacter.HandleInput(new AttackAction(this.targetTeamMember.GetDamageComponent()));
            }
        }
    }


    public override LocalAIState Update(double delta, LocalHumanAI localHumanAI)
    {
        this.UpdateTarget(delta);
        this.UpdateCheckIfAttack(delta);


        if (this.targetTeamMember != null && IsInstanceValid(this.targetTeamMember.node3D) && this.targetTeamMember.node3D.IsInsideTree()) 
        {
            this.localHumanAI.targetHumanCharacter.HandleInput(new MoveToAction(this.targetTeamMember.node3D.GlobalPosition));
            this.subState = null;
        }
        else if (this.targetTeamMember == null)
        {
            if (this.lastUncheckedVisualizationPosition != null)
            {
                Vector3 inertia = ((Vector3)this.lastUncheckedVisualizationPosition - this.localHumanAI.targetHumanCharacter.GlobalPosition).Normalized();
                this.subState = new WalkingAroundAIState((Vector3)this.lastUncheckedVisualizationPosition, this.localHumanAI, changeCentralPositionRandomly: true, inertia: inertia);
                this.lastUncheckedVisualizationPosition = null;
            } 
        }
        if (this.subState != null)
        {
            this.subState = this.subState.Update(delta, this.localHumanAI);
        }
        return this;
    }

    private void NavigationFinishedHandler()
    {

    }

    private double GetSomeRandomWaitTime(double maximum = 6, double minimum = 3)
    {
        return this.random.NextDouble() * (maximum - minimum) + minimum;
    }

    private bool IsTargetVisible(Node3D target, float distanceSquaredToTarget)
    {   
        if (!IsInstanceValid(target) || !target.IsInsideTree())
        {
            return false;
        }
        if (!(target is ITargeteable))
        {
            return false;
        }
        if (distanceSquaredToTarget > this.maxVisivilityDistanceSquared)
        {
            return false;
        }

        ITargeteable targeteable = (ITargeteable)target;

        PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(this.localHumanAI.targetHumanCharacter.GlobalPosition, 
                                                        target.GlobalPosition);
        Godot.Collections.Dictionary collidedElements = this.spaceState.IntersectRay(query);
        try
        {
            return (CollisionObject3D)collidedElements["collider"] == targeteable.collisionObject3D;
        }
        catch
        {
            return false;
        }
    }
}